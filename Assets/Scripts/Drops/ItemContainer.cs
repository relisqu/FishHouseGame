using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Drops {
    //[RequireComponent(typeof(BoxCollider))]
    public class ItemContainer : MonoBehaviour
    {
        [SerializeField] List<Transform> containerPlaces;
        [SerializeField] int capacity;
        public IngredientType ingredientType;

        List<EntityDrop> entityDrops;

        BoxCollider boxCollider;
        void Start()
        {
            entityDrops = new();
            return;
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            return;
            EntityDrop drop;

            if (other.gameObject.TryGetComponent<EntityDrop>(out drop))
            {
                if (drop.IngredientType == ingredientType)
                    PlaceItem(drop);
            }
            
        }

        public bool PlaceItem(EntityDrop entity)
        {
            if (entityDrops.Contains(entity))
                return true;

            if (entityDrops.Count >= capacity)
                return false;
            if (entityDrops.Count == 0)
            {
                entity.SetParent(nextPosition());
                entity.MoveToParent();
                entityDrops.Add(entity);
                return true;
            }
            entity.SetParent(nextPosition());
            entity.MoveToParent();
            entityDrops.Add(entity);

            return true;
        }


        private Transform nextPosition()
        {
            if (entityDrops.Count < containerPlaces.Count)
            {
                return containerPlaces[entityDrops.Count];
            }

            return entityDrops[entityDrops.Count % containerPlaces.Count].upperPoint;
            
        }

        public EntityDrop Remove()
        {
            var drop = entityDrops[entityDrops.Count - 1];
            entityDrops.Remove(drop);
            return drop;

        }

        
    }

     
}
