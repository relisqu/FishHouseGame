using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRecipeGenerator : MonoBehaviour
{
    [SerializeField] RandomRecipesDescription description;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<IngredientType> recipe = GenerateRecipe();
            PrintRecipe(recipe);
        }
    }

    private void PrintRecipe(List<IngredientType> Ingredients)
    {
        string output = "";
        foreach (var ingr in Ingredients)
        {
            output += ingr.ToString() + " ";
        }

        Debug.Log(output);
    }

    public List<IngredientType> GenerateRecipe()
    {
        RandomRecipeData recipeData = description.randomRecipesDescription[GenerateTemplateNumber()];

        int amountOfIngredients =
            Random.Range(recipeData.IngredientAmountRange.x, recipeData.IngredientAmountRange.y + 1);
        List<IngredientType> Ingredients = new();
        List<IngredientType> avaibleIngridiens = new();

        foreach (var Ingredient in recipeData.Ingredients)
        {
            avaibleIngridiens.Add(Ingredient.Ingredient);
        }

        foreach (var Ingredient in recipeData.Ingredients)
        {
            if (Ingredient.Probability < 1)
                continue;

            if (!avaibleIngridiens.Contains(Ingredient.Ingredient))
                continue;

            if (Ingredient.ExcludedCombo != null)
            {
                avaibleIngridiens.RemoveAll(
                    (IngredientType type) => { return Ingredient.ExcludedCombo.Contains(type); });
            }


            Ingredients.AddRange(TakeRandomlyIngredient(Ingredient.Probability, Ingredient.Ingredient));
        }

        while (true)
        {
            if (avaibleIngridiens.Count <= 0)
                break;

            if (Ingredients.Count >= amountOfIngredients)
                break;

            if (avaibleIngridiens.Count == 1)
            {
                Ingredients.AddRange(TakeRandomlyIngredient(
                    amountOfIngredients - Ingredients.Count,
                    avaibleIngridiens[0]));
            }

            int index = Random.Range(0, avaibleIngridiens.Count);
            RandomIngredientData data = GetIngredientData(avaibleIngridiens[index], recipeData.Ingredients);
            if (data != null)
            {
                var amount = TakeRandomlyIngredient(data.Probability, data.Ingredient);

                if (amount.Count <= 0)
                    continue;

                Ingredients.AddRange(amount);
                avaibleIngridiens.RemoveAll((IngredientType type) => { return data.ExcludedCombo.Contains(type); });
            }
        }

        Debug.Log("Name: "+recipeData.name+" time to cook: "+recipeData.TimeToCook+ "s" );
        return Ingredients;
    }

    private RandomIngredientData GetIngredientData(IngredientType type, List<RandomIngredientData> datas)
    {
        foreach (var data in datas)
        {
            if (data.Ingredient == type)
                return data;
        }

        return null;
    }

    private List<IngredientType> TakeRandomlyIngredient(float p, IngredientType IngredientType)
    {
        int amount = (int)(p - (p % 1));
        if ((p % 1) > Random.Range(0f, 1f))
        {
            amount++;
        }

        List<IngredientType> Ingredients = new();
        for (int i = 0; i < amount; i++)
        {
            Ingredients.Add(IngredientType);
        }

        return Ingredients;
    }


    private int GenerateTemplateNumber()
    {
        return Random.Range(0, description.randomRecipesDescription.Count);
    }
}