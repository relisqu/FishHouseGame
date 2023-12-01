using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Drops;
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
        [Range(0, 1)] [SerializeField] private float IdleScaleAmplitude;
        private Vector3 _startPosition;
        private Vector3 _startScale;

        private Transform _parent;
        private State currentState =State.Idle;

        public enum State { Idle, Picked, Stored }

        void Start()
        {
            _startPosition = transform.position;
        }

        TweenerCore<Vector3, Vector3, VectorOptions> _scaleTweenerCore;

        public void SetStartPosition(Vector3 position)
        {
            _startPosition = position;
            _startScale = transform.localScale;
            if(currentState!= State.Idle) return;
            _scaleTweenerCore = transform.DOScale(transform.localScale * (1 - IdleScaleAmplitude), IdleScaleSpeed)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            

            if (currentState== State.Idle)
            {
                var yDelta = Mathf.Sin(Time.time * IdleMovementSpeed) * Time.deltaTime * IdleMovementAmplitude;
                transform.position += Vector3.up * yDelta;
                transform.Rotate(0f, IdleRotationSpeed, 0f, Space.Self);
            }
        }

        public void SetParent(PlayerBagPack playerBagPack)
        {
            currentState = State.Picked;
            _scaleTweenerCore.Kill();
            transform.localScale = _startScale;

        }

        public State GetState()
        {
            return currentState;
        }

        public void SetParent(Transform parentTransform)
        {
            currentState = State.Stored;
            _parent = parentTransform;
            transform.parent = parentTransform;
        }
    }
}