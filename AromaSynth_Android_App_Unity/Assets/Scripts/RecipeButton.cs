using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeButton : MonoBehaviour
{
    public int[] recipe_output_ms_data = new int[20];
    public void OnClick()
    {
        AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
        for (int i = 0; i < recipe_output_ms_data.Length; i++)
        {
            // aromaEqualizer.output_ms_data[i] = recipe_output_ms_data[i];
            // グラフの高さをレシピに合わせて調整
            float graphX = aromaEqualizer.AromaObjects[i].transform.Find("Graph").GetComponent<RectTransform>().sizeDelta.x;
            float backgroundY = aromaEqualizer.AromaObjects[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta.y;
            float maximumOutput = aromaEqualizer.maximumOutput;
            aromaEqualizer.AromaObjects[i].transform.Find("Graph").GetComponent<RectTransform>().sizeDelta = new Vector2(graphX, recipe_output_ms_data[i] / maximumOutput * backgroundY);
        }
        GameObject.Find("LoadWindow").SetActive(false);
    }
}
