using UnityEngine;
using System.Collections;

namespace DefaultNamespace.EnemyAI
{
    public class WanderingAI : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f; // Speed of the AI movement
        [SerializeField] private float rotationSpeed = 2f; // Speed of the AI rotation
        [SerializeField] private AudioClip wanderingSound;

        private Vector3 targetPosition; // Destination where AI is currently moving towards

        private Vector3 spawnPosition;
        private Health _health;

        private void Start()
        {
            StartCoroutine(Sound());
            _health = GetComponent<Health>();
            spawnPosition = transform.position;
            // Set initial target position randomly
            targetPosition = GetRandomPosition();
        }

        private void Update()
        {
            if (!_health.isAlive) return;

            // Check if AI has reached the target position
            if (ReachedTargetPosition())
            {
                // Set a new random target position
                targetPosition = GetRandomPosition();
            }

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Rotate towards the target position
            RotateTowards(targetPosition);
        }

        private bool ReachedTargetPosition()
        {
            // Check if AI has reached the target position within certain distance
            return Vector3.Distance(transform.position, targetPosition) < 0.1f;
        }

        private void RotateTowards(Vector3 target)
        {
            // Calculate the direction to the target
            Vector3 direction = (target - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                // Calculate the rotation angle towards the target
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly rotate towards the target rotation
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private Vector3 GetRandomPosition()
        {
            // Generate a random position within a defined range
            float randomX = Random.Range(spawnPosition.x - 10f, spawnPosition.x + 10f);
            float randomZ = Random.Range(spawnPosition.z - 10f, spawnPosition.z + 10f);
            return new Vector3(randomX, 0.6f, randomZ);
        }

        IEnumerator Sound()
        {
            yield return new WaitForSeconds(Random.Range(0f, 2f));
            GetComponent<AudioSource>().clip = wanderingSound;
            GetComponent<AudioSource>().Play();
        }
    }
}