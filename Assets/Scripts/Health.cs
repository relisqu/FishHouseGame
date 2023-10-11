using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Health : MonoBehaviour
    {
        [SerializeField] public int MaxHealth = 100;
        private float currentHealth;

        private void Start()
        {
            currentHealth = MaxHealth;
        }

        public void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                Die();
            }
            else if(!isTakingDamage)
            {
                isTakingDamage = true;
                transform.DOPunchScale(Vector3.up, 0.2f). OnComplete(() =>
                {
                    isTakingDamage = false;
                });
            }
        }

        private bool isTakingDamage;
        private void Die()
        {
            transform.DOScale(Vector3.zero, 0.2f);
        }
    }
}