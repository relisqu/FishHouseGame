using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    private RecipeData data;

    private float remainingSeconds;

    private void Start()
    {
    }

    public void SetTimer(float timer)
    {
        remainingSeconds = timer;
    }

    private IEnumerator StartTimer()
    {
        while (remainingSeconds > 0)
        {
            remainingSeconds -= Time.deltaTime;
            yield return null;
        }
        
        yield return null;
    }

    void Complete()
    {
    }

    public void SetRecipeData(RecipeData recipe, float diffCoeff)
    {
        SetTimer(recipe.TimeToCook * diffCoeff);
        data = recipe;
        StartCoroutine(StartTimer());
    }
}