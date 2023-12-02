using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.EnemyAI
{
    [RequireComponent(typeof(Health))]
    public class DeathDropItem : SerializedMonoBehaviour
    {
        private Health _health;

        public DropTable CurDropTable;
        [SerializeField] private Vector2 JumpMovement;
        [SerializeField] private float JumpTime;

        private void Start()
        {
            _health = GetComponent<Health>();
            _health.OnDeath += DropItem;
        }

        private void OnDestroy()
        {
            _health.OnDeath -= DropItem;
        }

        public void DropItem(Health.DamageType deathReason)
        {
            var itemData = CurDropTable.Drops[deathReason];
            if (itemData == null) return;
            SpawnItem(itemData);
        }

        public void SpawnItem(Drop drop)
        {
            if (drop.MinDropAmount <= 0 || drop.DropItem == null) return;

            var curDropCount = Random.Range(drop.MinDropAmount, drop.MaxDropAmount);
            for (int i = 0; i < curDropCount; i++)
            {
                var obj = Instantiate(drop.DropItem, transform.position, transform.rotation);
                var angle = Quaternion.AngleAxis((float)i / curDropCount*360f, Vector3.up) * Vector3.right;
                obj.transform.DOLocalJump(transform.position + angle * JumpMovement.x, JumpMovement.y, 3, JumpTime)
                    .OnComplete(() => { obj.SetStartPosition(obj.transform.position); });
                obj.transform.localScale *= 0.001f;
                obj.transform.DOScale(obj.transform.localScale * 1000f, JumpTime);
            }
        }
    }

    
}