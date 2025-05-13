using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasteEqualizer : MonoBehaviour
{
    public float[] taste_output_ms_data = new float[5];
    public GPTTasteEstimator gptTasteEstimator;
    public float graphMaxMl;
    public float spoonCapcity;

    public float[] tastes_output_substances_data = new float[5];
    private float[] tastes_graphData = new float[5];
    private float[] taste_substance_in_bottles = {0.012f * 42f, 0.086f * 42f, 0.032f * 42f, 0.04f * 42f, 0.008f * 42f};
    private float[] taste_substance_in_drop = {0.012f, 0.086f, 0.032f, 0.04f, 0.008f};
    private float pump_ms_unit = 700f;

    public RectTransform[] tasteGraphs_rectTransform = new RectTransform[5];
    public int tasteGraphSizeY = 500;

    public void updateEqualizer_Slider(int tasteNum, float value)
    {
        //ドラッグによる移動幅が同じになるように調整して加算
        tastes_output_substances_data[tasteNum] += value * taste_substance_in_drop[tasteNum];
        if(tastes_output_substances_data[tasteNum]<0)
        {
            tastes_output_substances_data[tasteNum] = 0;
        }
        if(tastes_output_substances_data[tasteNum]>taste_substance_in_bottles[tasteNum]/(5/graphMaxMl))
        {
            tastes_output_substances_data[tasteNum] = taste_substance_in_bottles[tasteNum]/(5/graphMaxMl);
        }
        tastes_graphData[tasteNum] = tastes_output_substances_data[tasteNum] / taste_substance_in_bottles[tasteNum];
        tasteGraphs_rectTransform[tasteNum].sizeDelta = new Vector2(100,tastes_graphData[tasteNum] * tasteGraphSizeY * (5/graphMaxMl));
        updateOutputData_ms();
    }

    public void updateEqualizer_GPTEstimation()
    {
        for(int i=0; i<5; i++)
        {
            //4ml当たりの味覚物質量（出力量）に変換
            tastes_output_substances_data[i] = gptTasteEstimator.tasteData[i]*(spoonCapcity/1000);
            
            //ボトル内の味覚物質量に占める割合に変換
            tastes_graphData[i] = tastes_output_substances_data[i] / taste_substance_in_bottles[i];

            //定数をかけて棒グラフの高さに代入（5をかけるとボトル1/5の中の出力量として赤が表示される．）
            tasteGraphs_rectTransform[i].sizeDelta = new Vector2(100,tastes_graphData[i] * tasteGraphSizeY * (5/graphMaxMl));
        }
        updateOutputData_ms();
    }

    public void updateEqualizer_Receipe(float receipe_salt, float receipe_sweet, float receipe_sour, float receipe_umami, float receipe_bitter)
    {
        tastes_output_substances_data[0] = receipe_salt;
        tastes_output_substances_data[1] = receipe_sweet;
        tastes_output_substances_data[2] = receipe_sour;
        tastes_output_substances_data[3] = receipe_umami;
        tastes_output_substances_data[4] = receipe_bitter;
        for(int i=0; i<5; i++)
        {
            //ボトル内の味覚物質量に占める割合に変換
            tastes_graphData[i] = tastes_output_substances_data[i] / taste_substance_in_bottles[i];

            //定数をかけて棒グラフの高さに代入（5をかけるとボトル1/5の中の出力量として赤が表示される．）
            tasteGraphs_rectTransform[i].sizeDelta = new Vector2(100,tastes_graphData[i] * tasteGraphSizeY * (5/graphMaxMl));
        }
        updateOutputData_ms();
    }

    public void updateOutputData_ms()
    {
        // 各味のポンプ駆動時間を計算
        // salt_output_ms_data = (int)(tastes_output_substances_data[0]/salt_in_drop * pump_ms_unit);
        // sweet_output_ms_data = (int)(tastes_output_substances_data[1]/sweet_in_drop * pump_ms_unit);
        // sour_output_ms_data = (int)(tastes_output_substances_data[2]/sour_in_drop * pump_ms_unit);
        // umami_output_ms_data = (int)(tastes_output_substances_data[3]/umami_in_drop * pump_ms_unit);
        // bitter_output_ms_data = (int)(tastes_output_substances_data[4]/bitter_in_drop * pump_ms_unit);

        for(int i=0; i<5; i++)
        {
            taste_output_ms_data[i] = (int)(tastes_output_substances_data[i]/taste_substance_in_drop[i] * pump_ms_unit);
        }
    }
}
