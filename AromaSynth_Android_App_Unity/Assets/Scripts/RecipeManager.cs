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
    public GameObject RecipeButtonPrefab;
    public GameObject recipeButtonParent;
    private string fileName = "recipes.json";
    [System.NonSerialized] public RecipeCollection recipeCollection = new RecipeCollection();
    // ファイルのパスを取得
    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
    // レシピを追加して保存
    public void AddAndSaveRecipe(string name, int[] values)
    {
        RecipeData recipe = new RecipeData();
        recipe.recipeName = name;
        recipe.values = values;

        LoadAllRecipes(); // 現在の全データを読み込んでから追加
        recipeCollection.recipes.Add(recipe);
        SaveAllRecipes();
    }
    // 全レシピ保存
    private void SaveAllRecipes()
    {
        string json = JsonUtility.ToJson(recipeCollection, true);
        File.WriteAllText(GetFilePath(), json);
        Debug.Log("レシピ保存済み");
    }
    // 全レシピ読み込み
    public void LoadAllRecipes()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            recipeCollection = JsonUtility.FromJson<RecipeCollection>(json);
        }
        else
        {
            recipeCollection = new RecipeCollection(); // 初期化
        }
    }
    // レシピの一覧表示の更新
    public void updateScrollView()
    {
        // 既存のボタンを削除
        foreach (Transform child in recipeButtonParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // レシピのデータをJSONファイルから読み込む
        LoadAllRecipes();
        // レシピの数に合わせてスクロールエリアのサイズを調整
        recipeButtonParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, recipeCollection.recipes.Count * 150); // 高さをレシピの数に応じて調整
        // 新しいボタンを生成
        for (int i = 0; i < recipeCollection.recipes.Count; i++)
        {
            GameObject recipeButton = Instantiate(RecipeButtonPrefab, recipeButtonParent.transform);
            recipeButton.transform.Find("RecipeButton").Find("Button Text").GetComponent<TMPro.TextMeshProUGUI>().text = recipeCollection.recipes[i].recipeName;
            recipeButton.transform.Find("RecipeButton").GetComponent<RecipeButton>().recipe_output_ms_data = recipeCollection.recipes[i].values;
        }
    }
    // レシピの取得（インデックス指定）
    public RecipeData GetRecipe(int index)
    {
        LoadAllRecipes();
        if (index >= 0 && index < recipeCollection.recipes.Count)
        {
            return recipeCollection.recipes[index];
        }
        else
        {
            Debug.LogWarning("指定インデックスのレシピが存在しません");
            return null;
        }
    }
    // レシピの削除
    public void ClearAllRecipes()
    {
        RecipeCollection emptyData = new RecipeCollection(); // 空のリスト
        string json = JsonUtility.ToJson(emptyData, true);
        File.WriteAllText(GetFilePath(), json);

        Debug.Log("保存されたレシピデータをすべてクリアしました");
    }
    //　レシピの削除（名前指定）
    public void DeleteRecipeByName(string nameToDelete)
    {
        string path = GetFilePath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("レシピファイルが存在しません");
            return;
        }

        string json = File.ReadAllText(path);
        RecipeCollection collection = JsonUtility.FromJson<RecipeCollection>(json);

        int beforeCount = collection.recipes.Count;

        // 条件に一致しないものだけ残す
        collection.recipes.RemoveAll(r => r.recipeName == nameToDelete);

        // 上書き保存
        string newJson = JsonUtility.ToJson(collection, true);
        File.WriteAllText(path, newJson);

        int afterCount = collection.recipes.Count;
        Debug.Log($"{beforeCount - afterCount} 件のレシピ（{nameToDelete}）を削除しました");

        // スクロールビューの更新
        updateScrollView();
    }
}
