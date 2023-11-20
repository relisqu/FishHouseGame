using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Sword : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.GetComponent<Health>().TakeDamage(1, Health.DamageType.BySword);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.GetComponent<Health>().TakeDamage(1, Health.DamageType.BySword);
            }
        }
    }
}