using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        private InputHandler _input;

        [SerializeField] private bool RotateTowardMouse;

        [SerializeField] private float MovementSpeed;
        [SerializeField] private float RotationSpeed;
        [SerializeField] private float DashForce;
        
        [SerializeField] private ParticleSystem DashParticles;
        [SerializeField] private Rigidbody Rigidbody;

        [SerializeField] private Camera Camera;

        [SerializeField] LayerMask layerMask;
        [SerializeField] AudioClip dashSound;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            _input = GetComponent<InputHandler>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0)
                return;
            var targetVector = new Vector3(_input.InputVector.x, Rigidbody.velocity.y, _input.InputVector.y);
            var speed = MovementSpeed * Time.deltaTime;
            
            var movementVector = targetVector; 
            _animator.SetBool("IsRunning", targetVector.magnitude>0.01f);
            if (Input.GetKeyDown(KeyCode.Space) && targetVector.magnitude>0.01f)
            {
                Rigidbody.velocity= DashForce*targetVector.normalized;
                DashParticles.Play();
                GetComponent<AudioSource>().PlayOneShot(dashSound);
            }
            else
            {
                movementVector = MoveTowardTarget(targetVector,speed);
            }

            if (!RotateTowardMouse)
            {
                RotateTowardMovementVector(movementVector);
            }

            if (RotateTowardMouse)
            {
                RotateFromMouseVector();
            }
        }

        private void RotateFromMouseVector()
        {
            Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f, layerMask))
            {
                var target = hitInfo.point;
                target.y = transform.position.y;
                transform.LookAt(target);
            }
        }

        private Vector3 MoveTowardTarget(Vector3 targetVector, float speed)
        {
            targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
            var targetPosition = transform.position + targetVector.normalized * speed;
            transform.position = targetPosition;
            return targetVector;
        }

        private void RotateTowardMovementVector(Vector3 movementDirection)
        {
            if (movementDirection.magnitude == 0)
            {
                return;
            }

            var rotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
        }
    }
}