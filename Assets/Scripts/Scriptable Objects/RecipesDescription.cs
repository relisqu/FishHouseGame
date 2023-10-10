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
        
        
public class Parameters
{
    [SerializeField] public Dictionary<string, GameObject> prefabs;
    [SerializeField] public Dictionary<string, string> parameters;
}

public class RecipeData
{
    [field: SerializeField] public string name { get; set; }
    [field: SerializeField] public string description { get; set; }
    [field: SerializeField] public List<IngridientData> Ingridients { get; set; }

    
}

public class IngridientData
{
    [field: SerializeField] IngridientType Ingridient;
    [field: SerializeField] float TimeToCook;
    [field: SerializeField] int Amount;
}

public enum IngridientType {
    None,
    Fish,
    Bun,
    Rice,
    FriedFish,
}