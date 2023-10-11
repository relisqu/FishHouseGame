using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    [SerializeField] GameObject IngridientCell;
    public RectTransform rectTransform;
    List<GameObject> ingridientCells;
    public List<IngredientType> ingridientTypes;

    public void CreateRecipe(List<IngredientType> ingridients, bool stack, RandomRecipeData description)
    {
        ingridientTypes = ingridients;
        rectTransform = GetComponent<RectTransform>();
        ingridientCells = new();


        if (stack)
        {
            List<IngredientType> uniqueIngridients = new();
            Dictionary<IngredientType, int> uniqueIngridientsWithAmount = new();
            foreach (IngredientType ingridient in ingridients)
            {
                if (uniqueIngridients.Contains(ingridient))
                {
                    uniqueIngridientsWithAmount[ingridient] += 1;
                }
                else
                {
                    uniqueIngridients.Add(ingridient);
                    uniqueIngridientsWithAmount.Add(ingridient, 1);
                }
            }

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
                rectTransform.sizeDelta.y + IngridientCell.GetComponent<RectTransform>().rect.height *
                (uniqueIngridientsWithAmount.Count - 1));
            rectTransform.position -= new Vector3(0,
                IngridientCell.GetComponent<RectTransform>().rect.height * uniqueIngridientsWithAmount.Count, 0);


            foreach (KeyValuePair<IngredientType, int> kv in uniqueIngridientsWithAmount)
            {
                CreateIngridient(kv.Key, kv.Value);
            }
        }

        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
                rectTransform.sizeDelta.y +
                IngridientCell.GetComponent<RectTransform>().rect.height * (ingridients.Count - 1));
            rectTransform.position -= new Vector3(0,
                IngridientCell.GetComponent<RectTransform>().rect.height * ingridients.Count, 0);
            foreach (IngredientType ingridient in ingridients)
            {
                CreateIngridient(ingridient, 0);
            }
        }
    }

    private void CreateIngridient(IngredientType ingridient, int amount)
    {
        var cell = Instantiate(IngridientCell, transform);
        cell.SetActive(true);

        float height = IngridientCell.GetComponent<RectTransform>().rect.height;
        //cell.transform.Translate(Vector3.down * ingridientCells.Count * height, Space.World);

        if (ingridientCells.Count == 0)
        {
            cell.transform.localPosition = new Vector3(0, -height + rectTransform.rect.height / 2, 0);
        }
        else
        {
            cell.transform.localPosition = ingridientCells[ingridientCells.Count - 1].transform.localPosition +
                                           new Vector3(0, -height, 0);
        }

        UnityEngine.UI.Text text = cell.GetComponentInChildren<UnityEngine.UI.Text>();
        if (amount > 0)
        {
            text.text = ingridient.ToString() + " - " + amount;
        }
        else
        {
            text.text = ingridient.ToString();
        }

        ingridientCells.Add(cell);
    }
}