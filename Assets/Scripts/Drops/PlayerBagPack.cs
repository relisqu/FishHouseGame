using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drops
{
    public class PlayerBagPack : MonoBehaviour
    {

        [SerializeField] float distanceBetweenItems;
        [SerializeField] float itemsSpeed;
        List<EntityDrop> entityDrops;
        

        BoxCollider boxCollider;


        void Start()
        {
            entityDrops = new();
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
            MoveItems();
        }

        private void OnTriggerEnter(Collider other)
        {
            EntityDrop drop;

            if (other.gameObject.TryGetComponent<EntityDrop>(out drop))
            {
                PlaceItem(drop);
            }

        }

        public List<EntityDrop> RemoveItems()
        {
            List<EntityDrop> newList = new(entityDrops);

            entityDrops = new();
            return newList;
        }

        private void PlaceItem(EntityDrop entity)
        {
            if (entityDrops.Contains(entity))
                return;

            if (entityDrops.Count == 0)
            {
                entityDrops.Add(entity);
                return;
            }
            entityDrops.Add(entity);
        }

        private void MoveItems()
        {
            if (entityDrops.Count == 0)
                return;

            for (int i = 0;i < entityDrops.Count; i ++)
            {
                if(i == 0)
                {
                    var direction1 = transform.position - entityDrops[0].transform.position;
                    if (direction1.sqrMagnitude < distanceBetweenItems * distanceBetweenItems)
                        continue;

                    entityDrops[i].transform.Translate(direction1.normalized * itemsSpeed * Time.deltaTime, Space.World);
                    continue;
                }

                var direction = entityDrops[i - 1].transform.position - entityDrops[i].transform.position;
                if (direction.sqrMagnitude < distanceBetweenItems * distanceBetweenItems)
                    continue;

                entityDrops[i].transform.Translate(direction.normalized * itemsSpeed * Time.deltaTime, Space.World);
                continue;

            }
        }
    }
}
