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
        private List<Item> currentDrops = new List<Item>();
        public List<CookingRecipe> Recipes = new List<CookingRecipe>();
        public Slider TimerSlider;
        private Animator _animator;

        [SerializeField] Transform spawnPosition;

        private float _triggerTimer;

        private void Start()
        {
            TimerSlider.gameObject.SetActive(false);
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_triggerTimer > 0f)
            {
                _triggerTimer -= Time.deltaTime;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (isCooking) return;
            if (Input.GetKey(KeyCode.E) && _triggerTimer <= 0f)
            {
                if (other.gameObject.TryGetComponent(out PlayerBagPack pack))
                {

                    var item = pack.GetItem(0);
                    Debug.Log(item);
                    if (item != null)
                    {
                        if (item.CurItemType == ItemType.Meal)
                            return;
                        pack.RemoveItem(item);
                        item.gameObject.SetActive(false);
                        currentDrops.Add(item);
                        _triggerTimer = 0.3f;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
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
            if (isCooking && other.gameObject.TryGetComponent(out PlayerBagPack pack))
            {
                StopCooking();
            }
        }

        bool isCooking;

        private void StopCooking()
        {
            if (isCooking)
            {
                StopAllCoroutines();
            }
            _animator.Play("Idle");
        }

        public bool CheckIfRecipeOk(CookingRecipe recipe)
        {
            var list = new List<Item>(currentDrops);
            var recipeCopy = new List<ItemType>(recipe.Ingredients);

            foreach (var ingredient in recipe.Ingredients)
            {
                var foundItem = false;
                foreach (var curItem in list)
                {
                    if (curItem.CurItemType == ingredient)
                    {
                        foundItem = true;
                        list.Remove(curItem);
                        break;
                    }
                }

                if (!foundItem) return false;
            }

            return true;
        }

        private void TryStartCooking()
        {
            foreach (var rec in Recipes)
            {
                bool hasIngredients = true;
                if (!CheckIfRecipeOk(rec)) continue;
                CurrentRecipe = rec;
                Debug.Log("Cooking " + rec.name);
                StartCoroutine(StartCooking(rec, rec.TimeToCook));
                return;
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
            _animator.Play("Cooking");
            TimerSlider.gameObject.SetActive(true);
            var timer = time;
            while (timer > 0)
            {
                isCooking = true;
                timer -= Time.deltaTime;
                TimeLeftToCook = timer;
                TimerSlider.value = 1 - TimeLeftToCook / rec.TimeToCook;
                yield return null;
            }

            TimerSlider.gameObject.SetActive(false);
            GenerateMeal(rec.ResultingMeal);
            isCooking = false;
            yield return null;
            _animator.Play("Idle");
        }

        private void GenerateMeal(Meal meal)
        {
            Instantiate(meal, spawnPosition.position, spawnPosition.rotation);
            isCooking = false;
            _animator.Play("Idle");
            foreach (var item in currentDrops)
            {
                Destroy(item.gameObject);
            }

            CurrentRecipe = null;
            TimeLeftToCook = 0;
            _triggerTimer = 0;
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