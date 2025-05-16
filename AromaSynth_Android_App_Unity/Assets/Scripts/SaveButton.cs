using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButton : MonoBehaviour
{
    public TMP_InputField recipeNameInputField;
    public void OnClick()
    {
        AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
        RecipeManager recipeManager = GameObject.Find("RecipeManager").GetComponent<RecipeManager>();
        if (string.IsNullOrWhiteSpace(recipeNameInputField.text))
        {
            Debug.Log("Please enter a recipe name.");
        }
        else
        {
            // recipeを保存して保存ウィンドウを閉じる
            recipeManager.AddAndSaveRecipe(recipeNameInputField.text, aromaEqualizer.output_ms_data);
            this.transform.parent.gameObject.SetActive(false);
            recipeNameInputField.text = ""; // 入力フィールドをクリア
        }
    }
}
