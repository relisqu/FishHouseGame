using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Health : MonoBehaviour
    {
        [SerializeField] public int MaxHealth = 100;
        private float currentHealth;

        public bool isAlive;
        public Action<DamageType> OnDeath;

        private void Start()
        {
            isAlive = true;
            currentHealth = MaxHealth;
        }

        private bool isTakingDamage;

        public void TakeDamage(float damageAmount, DamageType type)
        {
            if(!isAlive) return;
            
            currentHealth -= damageAmount;

            if (currentHealth <= 0 )
            {
                Die(type);
            }
            else if (!isTakingDamage)
            {
                isTakingDamage = true;
                transform.DOPunchScale(Vector3.up, 0.2f).OnComplete(() => { isTakingDamage = false; });
            }
        }

        public enum DamageType
        {
            ByFire,
            BySword
        }

        private void Die(DamageType type)
        {
            OnDeath.Invoke(type);
            isAlive = false;
            transform.DOScale(Vector3.zero, 0.2f);
        }
    }
}