using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUI : MonoBehaviour
{

    [SerializeField] AnimationCurve speedOverTime;
    [SerializeField] Transform OpenedPosition;
    [SerializeField] Transform ClosedPosition;
    [SerializeField] GameObject openButton;
    [SerializeField] GameObject closeButton;
    [SerializeField] GameObject RecipeField;
    [SerializeField] float RecipesGap;
    [SerializeField] float mouseScrollSpeed;
    


    [SerializeField] GameObject recipePrefab;

    Dictionary<int, Recipe> recipes;
    [SerializeField] bool CollapseIngridients;

    Vector3 targetPosiyion;
    bool _isOpen;
    float _distance;

    

    // Start is called before the first frame update
    void Start()
    {
        recipes = new();
        _distance = (ClosedPosition.position - OpenedPosition.position).magnitude;
    }

    // Update is called once per frame
    /* void Update()
     {
         Move();
         if (_isOpen)
             Scroll();
     } 
     public void AddRecipe(List<IngredientType> ingridients, RandomRecipeData description, int ID)
     {
         var recipe = Instantiate(recipePrefab, RecipeField.transform).GetComponent<Recipe>();
         recipe.CreateRecipe(ingridients, CollapseIngridients, description);
         recipe.transform.localPosition = Vector3.zero;
         if(recipes.Count == 0)
         {
             
         }
         else
         {
             recipe.transform.localPosition = 
                 recipes[recipes.Count - 1].transform.localPosition +
                 recipes[recipes.Count - 1].rectTransform.sizeDelta.y * Vector3.down +
                 Vector3.down * RecipesGap;
         }    
         recipes.Add(recipes.Count, recipe);
     }
 
     public void DeleteRecipe(int ID)
     {
         recipes.Remove(ID);
     }
 
     private void Scroll()
     {
         float input = Input.GetAxis("Mouse ScrollWheel");
         RecipeField.transform.Translate(Vector3.down * input * mouseScrollSpeed);
 
     }
 
     private void Move()
     {
         Vector3 direction = (targetPosiyion - transform.position);
 
         if (direction.magnitude <= 1)
         {
             return;
         }
         float speed = speedOverTime.Evaluate(direction.magnitude / _distance);
 
         transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
 
     }
 
     public void ChangeState()
     {
         if (_isOpen)
             CloseMenu();
         else
             OpenMenu();
         _isOpen = !_isOpen; 
     }
 
     private void OpenMenu()
     {
         openButton.SetActive(false);
         closeButton.SetActive(true);
         targetPosiyion = OpenedPosition.position;
     }
 
     private void CloseMenu()
     {
         openButton.SetActive(true);
         closeButton.SetActive(false);
         targetPosiyion = ClosedPosition.position;
     }
     */
}
