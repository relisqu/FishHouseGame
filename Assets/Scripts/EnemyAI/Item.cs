using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.EnemyAI
{
    public class Item : MonoBehaviour
    {
        private bool IsPicked;
        [SerializeField] private float IdleRotationSpeed;
        [SerializeField] private float IdleMovementSpeed;
        [SerializeField] private float IdleMovementAmplitude;
        [SerializeField] private float IdleScaleSpeed;
        [Range(0,1)][SerializeField] private float IdleScaleAmplitude;
        private Vector3 _startPosition;

        void Start()
        {
            _startPosition = transform.position;
        }

        public void SetStartPosition(Vector3 position)
        {
            _startPosition = position;
            transform.DOScale(transform.localScale * (1-IdleScaleAmplitude), IdleScaleSpeed).SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            if (!IsPicked)
            {
                var yDelta = Mathf.Sin(Time.time * IdleMovementSpeed) * Time.deltaTime * IdleMovementAmplitude;
                transform.position += Vector3.up * yDelta;
                transform.Rotate(0f, IdleRotationSpeed, 0f, Space.Self);
            }
        }
    }
}