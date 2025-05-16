using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeButton : MonoBehaviour
{
    public int[] recipe_output_ms_data = new int[20];
    public void OnClick()
    {
        AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
        aromaEqualizer.updateGraphByData(recipe_output_ms_data);
        GameObject.Find("LoadWindow").SetActive(false);
    }
}
