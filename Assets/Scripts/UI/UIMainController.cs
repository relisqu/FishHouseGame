using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainController : MonoBehaviour
{
    [SerializeField] RecipeUI recipeUI;
    [SerializeField] RandomRecipeGenerator randomRecipeGenerator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            recipeUI.ChangeState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var recipe = randomRecipeGenerator.GenerateRecipe();
            recipeUI.AddRecipe(recipe.Item2, recipe.Item1, 0);
        }
    }
}
