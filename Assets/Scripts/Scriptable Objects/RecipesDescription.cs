using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.EnemyAI;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "New Recipes Description", menuName = "Configs/ Recipes Description", order = 0)]
public class RecipesDescription: SerializedScriptableObject
{
    [field: SerializeField] public List<RecipeData> recipesDescription { get; private set; }

    public RecipeData GetRandomRecipe()
    {
        var randomIdx = Random.Range(0,recipesDescription.Count);
        return recipesDescription[randomIdx];
    }

}
        


public class RecipeData
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField, Tooltip("In Seconds")] public float TimeToCook { get; set; }
    [field: SerializeField] public List<Ingredient> Ingredients { get; set; }

    public override string ToString()
    {
        return Name+"\n"+ Description+". Time to cook: "+TimeToCook+"." + Ingredients.ToString();
    }
}

public class Ingredient
{
    [field: SerializeField] public Item IngredientItem { get; set; }
    [field: SerializeField] public int MinAmount { get; set; }
    [field: SerializeField] public int MaxAmount { get; set; }
}
