using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeScrollViewManager : MonoBehaviour
{
    public GameObject RecipeButtonPrefab;
    private List<GameObject> recipeButtonObjectList = new List<GameObject>();
    private RecipeManager recipeManager;
    public void updateScrollView()
    {
        Transform content = this.transform.Find("Viewport").Find("Content");
        foreach(Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }
        RecipeManager recipeManager = GameObject.Find("RecipeManager").GetComponent<RecipeManager>();
        recipeManager.LoadAllRecipes();
        for (int i = 0; i < recipeManager.recipeCollection.recipes.Count; i++)
        {
            GameObject recipeButton = Instantiate(RecipeButtonPrefab, this.transform.Find("Viewport").Find("Content"));
            recipeButtonObjectList.Add(recipeButton);
            recipeButton.transform.Find("Button Text").GetComponent<TMPro.TextMeshProUGUI>().text = recipeManager.recipeCollection.recipes[i].recipeName;
            recipeButton.GetComponent<RecipeButton>().recipe_output_ms_data = recipeManager.recipeCollection.recipes[i].values;
        }
    }
}
