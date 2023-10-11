using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Recipes Description", menuName = "Configs/ Recipes Description", order = 0)]
public class RecipesDescription: SerializedScriptableObject
{
    [field: SerializeField] public List<RecipeData> recipesDescription { get; private set; }
}
        


public class RecipeData
{
    [field: SerializeField] public string name { get; set; }
    [field: SerializeField] public string description { get; set; }
    [field: SerializeField, Tooltip("In Seconds")] public float TimeToCook { get; set; }
    [field: SerializeField] public List<IngredientData> Ingredients { get; set; }

    


}

public class IngredientData
{
    [field: SerializeField] public IngredientType Ingredient { get; set; }
    [field: SerializeField] public int Amount { get; set; }
}

public enum IngredientType {
    None,
    Fish,
    Bun,
    Rice,
    FriedFish,

}