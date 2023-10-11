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
            List<IngridientType> recipe = GenerateRecipe();
            PrintRecipe(recipe);
        }
    }

    private void PrintRecipe(List<IngridientType> ingridients)
    {
        string output = "";
        foreach (var ingr in ingridients)
        {
            output += ingr.ToString() + " ";
        }
        Debug.Log(output);
    }

    public List<IngridientType> GenerateRecipe()
    {
        RandomRecipeData recipeData = description.randomRecipesDescription[GenerateTemplateNumber()];

        int amountOfIngridients = Random.Range(recipeData.IngridientAmountRange.x, recipeData.IngridientAmountRange.y + 1);
        List<IngridientType> ingridients = new();
        List<IngridientType> avaibleIngridiens = new();

        foreach (var ingridient in recipeData.Ingridients)
        {
            avaibleIngridiens.Add(ingridient.Ingridient);
        }

        foreach (var ingridient in recipeData.Ingridients)
        {
            if (ingridient.Probabilty < 1)
                continue;

            if (!avaibleIngridiens.Contains(ingridient.Ingridient))
                continue;

            avaibleIngridiens.RemoveAll((IngridientType type) => { return ingridient.ExcludedCombo.Contains(type); });

            ingridients.AddRange(TakeRandomlyIngridient(ingridient.Probabilty, ingridient.Ingridient));
            
        }

        while (true)
        {
            if (avaibleIngridiens.Count <= 0)
                break;

            if (ingridients.Count >= amountOfIngridients)
                break;

            if (avaibleIngridiens.Count == 1)
            {
                ingridients.AddRange(TakeRandomlyIngridient(
                    amountOfIngridients - ingridients.Count,
                    avaibleIngridiens[0]));
            }

            int index = Random.Range(0, avaibleIngridiens.Count);
            RandomIngridientData data = GetIngridientData(avaibleIngridiens[index], recipeData.Ingridients);
            if (data != null)
            {
                var amount = TakeRandomlyIngridient(data.Probabilty, data.Ingridient);

                if (amount.Count <= 0)
                    continue;

                ingridients.AddRange(amount);
                avaibleIngridiens.RemoveAll((IngridientType type) => { return data.ExcludedCombo.Contains(type); });
            }
        }

        return ingridients;
    }

    private RandomIngridientData GetIngridientData(IngridientType type, List<RandomIngridientData> datas)
    {
        foreach (var data in datas)
        {
            if (data.Ingridient == type)
                return data;
        }
        return null;
    }

    private List<IngridientType> TakeRandomlyIngridient(float p, IngridientType ingridientType)
    {
        int amount = (int)(p - (p % 1));
        if ((p % 1) > Random.Range(0f, 1f))
        {
            amount++;
        }

        List < IngridientType > ingridients= new();
        for (int i = 0; i < amount; i++)
        {
            ingridients.Add(ingridientType);
        }

        return ingridients;
    }


    private int GenerateTemplateNumber()
    {
        return Random.Range(0, description.randomRecipesDescription.Count);
    }

    
}
