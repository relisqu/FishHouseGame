using System.Collections.Generic;
using DefaultNamespace.EnemyAI;
using UnityEngine;

namespace DefaultNamespace.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "CookingRecipe", menuName = "Config/CookingRecipe", order = 0)]
    public class CookingRecipe : ScriptableObject
    {
        public List<ItemType> Ingredients;
        public float TimeToCook;
        public Meal ResultingMeal;
    }
}