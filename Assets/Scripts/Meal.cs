﻿using UnityEngine;

namespace DefaultNamespace
{
    public class Meal : MonoBehaviour
    {
        public MealType Type;
    }
}

public enum MealType
{
    Sashimi, 
    Sushi,
    Ramen, 
    NoodlesWithFish, 
    Rolls, 
    RiceBowl, 
    FishRiceBowl,
    BaoBun,
}