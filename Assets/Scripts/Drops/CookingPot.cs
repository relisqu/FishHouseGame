using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Additional;
using DefaultNamespace.EnemyAI;
using DefaultNamespace.Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Drops
{
    public class CookingPot : MonoBehaviour
    {
        private List<Item> currentDrops;
        public List<CookingRecipe> Recipes;
        public Slider TimerSlider;


        private float _triggerTimer;

        private void Update()
        {
        }

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKey(KeyCode.E) && _triggerTimer <= 0f)
            {
                if (other.gameObject.TryGetComponent(out PlayerBagPack pack))
                {
                    var item = pack.GetItem(0);
                    if (item != null)
                    {
                        currentDrops.Add(item);
                        _triggerTimer = 0.3f;
                    }
                }
            }

            if (Input.GetKeyDown("F"))
            {
                TryStartCooking();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (isCooking && other.gameObject.TryGetComponent(out PlayerBagPack pack))
            {
                ContinueRecipe();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            StopCooking();
        }

        bool isCooking;

        private void StopCooking()
        {
            if (isCooking)
            {
                StopAllCoroutines();
            }
        }

        public bool CheckRecipes(CookingRecipe recipe)
        {
            var list = new List<Item>(currentDrops);
            var hasIngredients = false;
            foreach (var ingredient in recipe.Ingredients)
            {
                foreach (var curItem in list)
                {
                    if (curItem.name.StartsWith(ingredient.name))
                    {
                        list.Remove(curItem);
                        hasIngredients = true;
                    }
                    else
                    {
                    }
                }

                if (!hasIngredients) return false;
            }
        }

        private void TryStartCooking()
        {
            foreach (var rec in Recipes)
            {
                bool hasIngredients = true;
                foreach (var ingredient in currentDrops)
                {
                    if (!rec.Ingredients.Contains(ingredient))
                    {
                        hasIngredients = false;
                    }
                }

                if (!hasIngredients) continue;
                CurrentRecipe = rec;
                StartCoroutine(StartCooking(rec, rec.TimeToCook));
                break;
            }

            FailRecipe();
        }

        public void ContinueRecipe()
        {
            StartCoroutine(StartCooking(CurrentRecipe, TimeLeftToCook));
        }

        private float TimeLeftToCook;
        private CookingRecipe CurrentRecipe;

        private IEnumerator StartCooking(CookingRecipe rec, float time)
        {
            TimerSlider.gameObject.SetActive(true);
            var timer = time;
            while (timer > 0)
            {
                isCooking = true;
                timer -= Time.deltaTime;
                TimeLeftToCook = timer;
                TimerSlider.value = 1 - (rec.TimeToCook / TimeLeftToCook);
            }

            TimerSlider.gameObject.SetActive(false);
            GenerateMeal(rec.ResultingMeal);
            isCooking = false;
            yield return null;
        }

        private void GenerateMeal(Meal meal)
        {
            Instantiate(meal);
        }

        private void FailRecipe()
        {
            CleanCookSpot();
        }

        private void CleanCookSpot()
        {
            currentDrops.Clear();
            CameraShake.Instance.ShakeCamera(10f, 0.3f);
        }
    }
}