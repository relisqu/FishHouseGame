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

        private Transform _parent;

        void Start()
        {
            _startPosition = transform.position;
        }

        TweenerCore<Vector3, Vector3, VectorOptions> _scaleTweenerCore;

        public void SetStartPosition(Vector3 position)
        {
            _startPosition = position;
            Debug.Log("Start tween");
            if(IsPicked) return;
            _scaleTweenerCore = transform.DOScale(transform.localScale * (1 - IdleScaleAmplitude), IdleScaleSpeed)
                .SetLoops(-1, LoopType.Yoyo);
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

        public void SetParent(PlayerBagPack playerBagPack)
        {
            IsPicked = true;
            _scaleTweenerCore.Kill();
            Debug.Log("Kill tween");
            Debug.Log(DOTween.Kill(this));
            DOTween.Kill(_scaleTweenerCore);

        }

        public void SetParent(Transform parentTransform)
        {
            _parent = parentTransform;
            transform.parent = parentTransform;
        }
    }
}