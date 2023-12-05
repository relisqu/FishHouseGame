using System;
using DefaultNamespace;
using DefaultNamespace.EnemyAI;
using DefaultNamespace.UI;
using UnityEngine;

namespace Drops
{
    public class ServingTable : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            Debug.Log(other.name);
            if (other.TryGetComponent(out PlayerBagPack pack) && Input.GetKeyDown(KeyCode.E))
            {
                var meal = pack.HasItems(ItemType.Meal);
                Debug.Log(meal);
                if (meal != null)
                {
                    SearchForRecipes(meal, pack);
                }
            }
        }

        private void SearchForRecipes(Item meal, PlayerBagPack pack)
        {
            var recipes = RecipeGenerator.Instance.GetRecipes();
            foreach (var r in recipes)
            {
                Debug.Log(r.GetData().Meal.Type);
                if (r.GetData().Meal.Type == meal.GetComponent<Meal>().Type)
                {
                    RecipeGenerator.Instance.CompleteRecipe(r);
                    pack.RemoveItem(meal);
                    Destroy(meal.gameObject);
                }

                return;
            }
        }
    }
}