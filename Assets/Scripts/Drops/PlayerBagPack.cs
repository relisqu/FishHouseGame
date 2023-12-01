using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.EnemyAI;
using Sirenix.Utilities;
using UnityEngine;

namespace Drops
{
    public class PlayerBagPack : MonoBehaviour
    {
        [SerializeField] private float DistanceBetweenItems;
        [SerializeField] private float ItemsSpeed;
        [SerializeField] private int MaxCapacity = 3;
        private List<Item> _drops;

        public static PlayerBagPack Instance;
        Collider boxCollider;


        void Start()
        {
            Instance = this;
            _drops = new();
            boxCollider = GetComponent<Collider>();
            boxCollider.isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
            MoveItems();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.TryGetComponent(out Item _));
            if (other.gameObject.TryGetComponent(out Item item) && item.GetState() == Item.State.Idle &&
                _drops.Count < MaxCapacity)
            {
                PlaceItem(item);
            }
        }

        public List<Item> PopItems()
        {
            var list = new List<Item>(_drops);
            _drops.Clear();
            return list;
        }

        private void PlaceItem(Item item)
        {
            if (_drops.Contains(item))
                return;

            _drops.Add(item);
            item.SetParent(this);
        }

        private void MoveItems()
        {
            if (_drops.Count == 0)
                return;

            var direction1 = transform.position - _drops[0].transform.position;
            if (direction1.sqrMagnitude >= DistanceBetweenItems * DistanceBetweenItems)
            {
                var movDir = direction1.normalized * (ItemsSpeed * Time.deltaTime);
                _drops[0].transform.Translate(movDir, Space.World);
            }

            for (int i = 1; i < _drops.Count; i++)
            {
                var direction = _drops[i - 1].transform.position - _drops[i].transform.position;
                if (direction.sqrMagnitude < DistanceBetweenItems * DistanceBetweenItems)
                    continue;

                var movDir = direction.normalized * (ItemsSpeed * Time.deltaTime);
                _drops[i].transform.Translate(movDir, Space.World);
            }
        }

        public void RemoveItem(Item item)
        {
            _drops.Remove(item);
        }
    }
}