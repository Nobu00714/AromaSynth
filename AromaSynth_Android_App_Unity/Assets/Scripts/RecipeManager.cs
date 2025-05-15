using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RecipeData {
    public string recipeName;
    public int[] values = new int[20];
}

[System.Serializable]
public class RecipeCollection {
    public List<RecipeData> recipes = new List<RecipeData>();
}

public class RecipeManager : MonoBehaviour
{
    private string fileName = "recipes.json";
    [System.NonSerialized] public RecipeCollection recipeCollection = new RecipeCollection();
    // ファイルのパスを取得
    private string GetFilePath() {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
    // レシピを追加して保存
    public void AddAndSaveRecipe(string name, int[] values) {
        RecipeData recipe = new RecipeData();
        recipe.recipeName = name;
        recipe.values = values;

        LoadAllRecipes(); // 現在の全データを読み込んでから追加
        recipeCollection.recipes.Add(recipe);
        SaveAllRecipes();
    }
    // 全レシピ保存
    private void SaveAllRecipes() {
        string json = JsonUtility.ToJson(recipeCollection, true);
        File.WriteAllText(GetFilePath(), json);
        Debug.Log("レシピ保存済み");
    }
    // 全レシピ読み込み
    public void LoadAllRecipes() {
        string path = GetFilePath();
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            recipeCollection = JsonUtility.FromJson<RecipeCollection>(json);
        } else {
            recipeCollection = new RecipeCollection(); // 初期化
        }
    }
    // レシピの取得（インデックス指定）
    public RecipeData GetRecipe(int index) {
        LoadAllRecipes();
        if (index >= 0 && index < recipeCollection.recipes.Count) {
            return recipeCollection.recipes[index];
        } else {
            Debug.LogWarning("指定インデックスのレシピが存在しません");
            return null;
        }
    }
}
