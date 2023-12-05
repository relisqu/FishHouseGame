using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class RecipeGenerator : MonoBehaviour
    {
        public int MaxRecipesCount;
        [Range(0.2f, 3f)] public float DifficultyCoeff;
        public float RecipesDelay;
        [SerializeField] private RecipesDescription Recipes;
        [SerializeField] private Recipe RecipePrefab;
        private List<Recipe> _currentRecipes = new List<Recipe>();
        [SerializeField] public Transform RecipesTransform;

        public static RecipeGenerator Instance;

        void Start()
        {
            Instance = this;
            StartCoroutine(GenerateRecipes());
        }

        public List<Recipe> GetRecipes()
        {
            return _currentRecipes;
        }


        private IEnumerator GenerateRecipes()
        {
            var curNumRecipes = 0;
            while (curNumRecipes < MaxRecipesCount)
            {
                curNumRecipes++;
                var recipe = Recipes.GetRandomRecipe();
                var data = Instantiate(RecipePrefab, RecipesTransform);
                _currentRecipes.Add(data);
                data.SetRecipeData(recipe, DifficultyCoeff);

                yield return new WaitForSeconds(RecipesDelay);
            }
        }

        public void RemoveRecipe(Recipe recipe)
        {
            _currentRecipes.Remove(recipe);
        }

        public void CompleteRecipe(Recipe recipe)
        {
            recipe.SetCompleted();
            RemoveRecipe(recipe);
        }
    }
}