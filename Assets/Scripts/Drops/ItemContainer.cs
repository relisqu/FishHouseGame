using System;
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
        private Interactable _text;

        void Start()
        {
            _text = GetComponentInChildren<Interactable>();
            _ingredients = new();
        }

        private void Update()
        {
            if (Capacity >= _ingredients.Count)
            {
                if ( _ingredients.Count > 0)
                {
                    _text.SetText("Press E to stack ingredients\n Press F to get ingredients");
                }
                else
                {
                    _text.SetText("Press E to stack ingredients");
                }
            }
            else
            {
                _text.SetText("Press F to get ingredients");
            }
        
            _triggerTimer -= Time.deltaTime;
        }

        private float _triggerTimer;
        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKey(KeyCode.E) && _triggerTimer<=0f)
            {
                if (other.gameObject.TryGetComponent(out PlayerBagPack pack))
                {
                    var item = pack.HasItems(IngredientType.name);
                    if (item)
                    {
                        _triggerTimer = 0.3f;
                        TryPlaceItem(item);
                    }
                }
            }

            if (Input.GetKey(KeyCode.F) && _ingredients.Count > 0 && _triggerTimer<=0f)
            {
                if (other.gameObject.TryGetComponent(out PlayerBagPack pack) && pack.HasSpace())
                {
                    _triggerTimer = 0.3f;
                    pack.PlaceItem(Remove());
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