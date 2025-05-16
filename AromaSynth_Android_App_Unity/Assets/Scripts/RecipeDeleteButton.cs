using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeDeleteButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    public void OnClick()
    {
        RecipeManager recipeManager = GameObject.Find("RecipeManager").GetComponent<RecipeManager>();
        // recipeを削除
        recipeManager.DeleteRecipeByName(recipeNameText.text);
    }
}
