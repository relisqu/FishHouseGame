using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PlayerHealth : MonoBehaviour
    {
        public int Health;

        public Image HeartObj;

        public Transform HealthTransform;
        public Sprite DamagedHeart;

        public List<Image> HeartObjects = new();

        private void Start()
        {
            for (int i = 0; i < Health; i++)
            {
                HeartObjects.Add(Instantiate(HeartObj, HealthTransform));
            }

            Recipe.FailedRecipe += TakeDamage;
        }

        private void OnDestroy()
        {
            Recipe.FailedRecipe -= TakeDamage;
        }

        public void TakeDamage()
        {
            Health -= 1;
            if (Health > 0)
            {
                HeartObjects[Health].sprite = DamagedHeart;
            }
            else
            {
                Lose();
            }
        }

        public void Lose()
        {
            GameManager.Instance.Lose();
        }
    }
}