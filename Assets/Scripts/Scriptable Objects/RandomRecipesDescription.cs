using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Random Recipes Description", menuName = "Configs/Random Recipes Description", order = 0)]
public class RandomRecipesDescription : SerializedScriptableObject
{
    [field: SerializeField] public List<RandomRecipeData> randomRecipesDescription { get; private set; }
}



public class RandomRecipeData
{
    [field: SerializeField] public string name { get; set; }
    [field: SerializeField] public string description { get; set; }
    [field: SerializeField] public Vector2Int IngridientAmountRange { get; set; }
    [field: SerializeField, Tooltip("In Seconds")] public float TimeToCook { get; set; }
    [field: SerializeField] public List<RandomIngridientData> Ingridients { get; set; }




}

public class RandomIngridientData
{
    [field: SerializeField] public IngridientType Ingridient { get; set; }
    [field: SerializeField] public int Amount { get; set; }
    [field: SerializeField, Tooltip("p = 2.5 means, it 2 guarantied and third one is 50/50")] public float Probabilty { get; set; }

    [field: SerializeField] public List<IngridientType> ExcludedCombo { get; set; }
}
