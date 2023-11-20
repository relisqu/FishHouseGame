using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.EnemyAI;
using UnityEngine;

namespace Drops
{
    public class DropSystem : MonoBehaviour
    {
        // Start is called before the first frame update

        [SerializeField] List<ItemContainer> containers;

        [SerializeField] ItemContainer trashBin;

        //[SerializeField] IngridientPrefabs ingridientPrefabs;
        public List<EntityDrop> avaibleOutDoorDrops;


        void Start()
        {
            // dropPrefabs = ingridientPrefabs.prefabs;
        }

        // Update is called once per frame
        void Update()
        {
        }
/*
        public void GenerateDrop(IngredientType ingredient, Transform position)
        {
            if (!dropPrefabs.ContainsKey(ingredient))
                return;

            var drop = Instantiate(dropPrefabs[ingredient], position.position, new Quaternion(0, 0, 0, 0));
            avaibleOutDoorDrops.Add(drop);
        }

        private void SendDrop(Item drop)
        {
            foreach(ItemContainer container in containers)
            {
                if (container.PlaceItem(drop))
                    return;
            }

            trashBin.PlaceItem(drop);
            
        }


        private void OnTriggerEnter(Collider other)
        {
            PlayerBagPack bagPack;
            if(other.gameObject.TryGetComponent<PlayerBagPack>(out bagPack))
            {
                foreach (var item in bagPack.PopItems())
                    SendDrop(item);
            }
        }
    }*/
    }
}