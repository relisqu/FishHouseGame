using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Additional;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Recipe : MonoBehaviour
{
    private RecipeData data;

    private float remainingSeconds;

    [Header("Slider")] public Slider Slider;
    public Image SliderImage;

    public Gradient TimeColor;


    public IngredientImage IngredientPrefab;
    public Transform IngredientTransform;

    public Image MealImage;


    public static Action FailedRecipe;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetTimer(float timer)
    {
        remainingSeconds = timer;
    }

    private IEnumerator StartTimer()
    {
        var defaultTimer = remainingSeconds;
        MealImage.sprite = data.MealIcon;
        SpawnIngredients();
        while (remainingSeconds > 0)
        {
            Slider.value = remainingSeconds / defaultTimer;
            SliderImage.color = TimeColor.Evaluate(1 - Slider.value);
            remainingSeconds -= Time.deltaTime;
            yield return null;
        }

        Complete();
        FailedRecipe?.Invoke();

        CameraShake.Instance.ShakeCamera(3.5f, 0.2f);
        yield return null;
    }

    void SpawnIngredients()
    {
        var items = data.Ingredients;

        foreach (var i in items)
        {
            i.CurAmount = Random.Range(i.MinAmount, i.MaxAmount);
            for (int j = 0; j < i.CurAmount; j++)
            {
                var ingredient = Instantiate(IngredientPrefab, IngredientTransform);
                ingredient.GetImage().sprite = i.IngredientItem.IngredientIcon;
            }
        }
    }

    void Complete()
    {
        _animator.SetTrigger("Complete");
        Destroy(gameObject, 0.3f);
    }

    public void SetRecipeData(RecipeData recipe, float diffCoeff)
    {
        SetTimer(recipe.TimeToCook * diffCoeff);
        data = recipe;
        StartCoroutine(StartTimer());
    }

    public RecipeData GetData()
    {
        return data;
    }

    public void SetCompleted()
    {
        StopAllCoroutines();
        Complete();
    }
}