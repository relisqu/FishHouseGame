using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.EnemyAI;
using UnityEngine;


namespace Drops
{
    //[RequireComponent(typeof(BoxCollider))]
    public class ItemContainer : MonoBehaviour
    {
        [SerializeField] List<Transform> containerPlaces;
        [SerializeField] private int Capacity;
        [SerializeField] private Item IngredientType;

        List<Item> _ingredients;

        Collider _collider;

        void Start()
        {
            _ingredients = new();
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (other.gameObject.TryGetComponent(out Item drop) )
                {
                    if (drop.name.StartsWith(IngredientType.name))
                        TryPlaceItem(drop);
                }
            }

            
        }

        public bool TryPlaceItem(Item item)
        {
            if (_ingredients.Contains(item))
                return true;

            if (_ingredients.Count >= Capacity)
                return false;

            item.SetParent(GetNextPosition());
            item.transform.localPosition = Vector3.zero;
            _ingredients.Add(item);
            PlayerBagPack.Instance.RemoveItem(item);
            return true;
        }


        private Transform GetNextPosition()
        {
            if (_ingredients.Count < containerPlaces.Count)
            {
                return containerPlaces[_ingredients.Count];
            }

            return _ingredients[_ingredients.Count % containerPlaces.Count].transform;
        }

        public Item Remove()
        {
            var drop = _ingredients[^1];
            _ingredients.Remove(drop);
            return drop;
        }
    }
}