using System;
using System.Collections.Generic;
using DefaultNamespace.EnemyAI;
using Drops;
using UnityEngine;
using Random = UnityEngine.Random;

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
            ResetGrowTimer();
            
            foreach (var r in Renderers)
            {
                r.sprite = GrowSprites[_currentPhase];
            }
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
                if (GrowSprites.Count - _currentPhase <= 2)
                {
                    var currentAmount = Random.Range(1,2*(3- GrowSprites.Count + _currentPhase));
                    GetComponent<ItemDrop>().DropItem(Health.DamageType.BySword,currentAmount);
                }
                ResetGrowth();

            }
        }

        public void Grow()
        {
            if (_currentPhase < GrowSprites.Count-1)
            {
                _currentPhase++;
            }

            foreach (var r in Renderers)
            {
                r.sprite = GrowSprites[_currentPhase];
            }

            ResetGrowTimer();
        }

        public void ResetGrowTimer()
        {
            _growingTimer = 20 / GrowSpeed;
        }

        private float _growingTimer;
    }
}