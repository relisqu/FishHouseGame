using DefaultNamespace;
using DefaultNamespace.EnemyAI;
using DG.Tweening;
using UnityEngine;

namespace Drops
{
    public class ItemDrop : MonoBehaviour
    {
        public DropTable CurDropTable;
        [SerializeField] private Vector2 JumpMovement;
        [SerializeField] private float JumpTime;


        public void DropItem(Health.DamageType deathReason, int amount)
        {
            var itemData = CurDropTable.Drops[deathReason];
            if (itemData == null) return;
            SpawnItem(itemData, amount);
        }

        public void SpawnItem(Drop itemDrop, int amount)
        {
            if (itemDrop.DropItem == null) return;
            for (int i = 0; i < amount; i++)
            {
                var obj = Instantiate(itemDrop.DropItem, transform.position, transform.rotation);
                var angle = Quaternion.AngleAxis((float)i / amount * 360f, Vector3.up) * Vector3.right;
                obj.transform.DOLocalJump(transform.position +Vector3.up*0.6f+ angle * JumpMovement.x, JumpMovement.y, 3, JumpTime)
                    .OnComplete(() => { obj.SetStartPosition(obj.transform.position); });
                obj.transform.localScale *= 0.001f;
                obj.transform.DOScale(obj.transform.localScale * 1000f, JumpTime);
            }
        }
    }
}

