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

        private void Update()
        {
            foreach (var r in _currentRecipes)
            {
                if (r != null) continue;
                _currentRecipes.Remove(r);
                break;

            }
        }

        private IEnumerator GenerateRecipes()
        {
            var curNumRecipes = 0;
            while (curNumRecipes < MaxRecipesCount)
            {
                foreach (var r in _currentRecipes)
                {
                    if (r != null) continue;
                    _currentRecipes.Remove(r);
                    break;
                    
                }
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
            Debug.Log("Recipe is deleted: " + _currentRecipes.Remove(recipe));
        }

        public void CompleteRecipe(Recipe recipe)
        {

            RemoveRecipe(recipe);
            recipe.SetCompleted();
        }
    }
}