using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingridient Prefabs Description", menuName = "Configs/Ingridient Prefabs Description", order = 0)]
public class IngridientPrefabs : SerializedScriptableObject
{
    [field: SerializeField] public Dictionary<IngredientType, Drops.EntityDrop> prefabs;
}
