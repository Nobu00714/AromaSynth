using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButton : MonoBehaviour
{
    public TextMeshProUGUI recipeNameText;
    public void OnClick()
    {
        AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
        RecipeManager recipeManager = GameObject.Find("RecipeManager").GetComponent<RecipeManager>();
        string recipeName = recipeNameText.text;
        if (recipeName == "")
        {
            Debug.Log("Please enter a recipe name.");
        }
        else
        {
            // recipeを保存して保存ウィンドウを閉じる
            recipeManager.AddAndSaveRecipe(recipeName, aromaEqualizer.output_ms_data);
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
