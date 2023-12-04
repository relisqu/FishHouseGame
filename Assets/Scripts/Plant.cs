using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> Renderers = new();
        [SerializeField] private List<Sprite> GrowSprites = new();
        [SerializeField] private float GrowSpeed;
        private int _currentPhase;

        private void Start()
        {
        }

        private void Update()
        {
            if(!CanGrow()) return;
            if (_growingTimer > 0)
            {
                _growingTimer -= Time.deltaTime;
                if (_growingTimer <= 0)
                {
                    Grow();
                }
            }
        }

        public bool CanGrow()
        {
            return _currentPhase < GrowSprites.Count;
        }

        public void ResetGrowth()
        {
            _currentPhase = 0;
            foreach (var r in Renderers)
            {
                r.sprite = GrowSprites[_currentPhase];
            }

            ResetGrowTimer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Sword sword))
            {
                ResetGrowth();
            }
        }

        public void Grow()
        {
            if (_currentPhase < GrowSprites.Count)
            {
                _currentPhase++;
            }

            foreach (var r in Renderers)
            {
                r.sprite = GrowSprites[_currentPhase];
            }
        }

        public void ResetGrowTimer()
        {
            _growingTimer = 20 / GrowSpeed;
        }

        private float _growingTimer;
    }
}