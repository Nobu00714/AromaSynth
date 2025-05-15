using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AromaEqualizer : MonoBehaviour
{
    public int[] output_ms_data = new int[20];
    public float maximumOutput;
    public GameObject[] AromaObjects = new GameObject[20];
    private float[] graphY = new float[20];
    private float backgroundY;
    void Start()
    {
        for(int i=0; i<AromaObjects.Length; i++)
        {
            AromaObjects[i] = this.transform.GetChild(i).gameObject;
        }
        backgroundY = AromaObjects[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta.y;
    }
    void Update()
    {
        // Update the output_ms_data for each aroma graph
        for (int i=0; i<AromaObjects.Length; i++)
        {
            if (AromaObjects[i] != null)
            {
                // グラフの高さに応じて出力ミリ秒を計算
                graphY[i] = AromaObjects[i].transform.Find("Graph").GetComponent<RectTransform>().sizeDelta.y;
                output_ms_data[i] = (int)(graphY[i]/backgroundY * maximumOutput);

                // 表示文字変更
                AromaObjects[i].transform.Find("Num").GetComponent<TextMeshProUGUI>().text = (output_ms_data[i]).ToString() + " ms";
            }
        }
    }
    // public void updateEqualizer_Slider(int tasteNum, float value)
    // {
    //     //ドラッグによる移動幅が同じになるように調整して加算
    //     tastes_output_substances_data[tasteNum] += value * taste_substance_in_drop[tasteNum];
    //     if(tastes_output_substances_data[tasteNum]<0)
    //     {
    //         tastes_output_substances_data[tasteNum] = 0;
    //     }
    //     if(tastes_output_substances_data[tasteNum]>taste_substance_in_bottles[tasteNum]/(5/graphMaxMl))
    //     {
    //         tastes_output_substances_data[tasteNum] = taste_substance_in_bottles[tasteNum]/(5/graphMaxMl);
    //     }
    //     tastes_graphData[tasteNum] = tastes_output_substances_data[tasteNum] / taste_substance_in_bottles[tasteNum];
    //     tasteGraphs_rectTransform[tasteNum].sizeDelta = new Vector2(100,tastes_graphData[tasteNum] * tasteGraphSizeY * (5/graphMaxMl));
    //     updateOutputData_ms();
    // }

    // public void updateEqualizer_GPTEstimation()
    // {
    //     for(int i=0; i<5; i++)
    //     {
    //         //4ml当たりの味覚物質量（出力量）に変換
    //         tastes_output_substances_data[i] = gptTasteEstimator.tasteData[i]*(spoonCapcity/1000);
            
    //         //ボトル内の味覚物質量に占める割合に変換
    //         tastes_graphData[i] = tastes_output_substances_data[i] / taste_substance_in_bottles[i];

    //         //定数をかけて棒グラフの高さに代入（5をかけるとボトル1/5の中の出力量として赤が表示される．）
    //         tasteGraphs_rectTransform[i].sizeDelta = new Vector2(100,tastes_graphData[i] * tasteGraphSizeY * (5/graphMaxMl));
    //     }
    //     updateOutputData_ms();
    // }

    // public void updateEqualizer_Receipe(float receipe_salt, float receipe_sweet, float receipe_sour, float receipe_umami, float receipe_bitter)
    // {
    //     tastes_output_substances_data[0] = receipe_salt;
    //     tastes_output_substances_data[1] = receipe_sweet;
    //     tastes_output_substances_data[2] = receipe_sour;
    //     tastes_output_substances_data[3] = receipe_umami;
    //     tastes_output_substances_data[4] = receipe_bitter;
    //     for(int i=0; i<5; i++)
    //     {
    //         //ボトル内の味覚物質量に占める割合に変換
    //         tastes_graphData[i] = tastes_output_substances_data[i] / taste_substance_in_bottles[i];

    //         //定数をかけて棒グラフの高さに代入（5をかけるとボトル1/5の中の出力量として赤が表示される．）
    //         tasteGraphs_rectTransform[i].sizeDelta = new Vector2(100,tastes_graphData[i] * tasteGraphSizeY * (5/graphMaxMl));
    //     }
    //     updateOutputData_ms();
    // }

    // public void updateOutputData_ms()
    // {
    //     // 各味のポンプ駆動時間を計算
    //     // salt_output_ms_data = (int)(tastes_output_substances_data[0]/salt_in_drop * pump_ms_unit);
    //     // sweet_output_ms_data = (int)(tastes_output_substances_data[1]/sweet_in_drop * pump_ms_unit);
    //     // sour_output_ms_data = (int)(tastes_output_substances_data[2]/sour_in_drop * pump_ms_unit);
    //     // umami_output_ms_data = (int)(tastes_output_substances_data[3]/umami_in_drop * pump_ms_unit);
    //     // bitter_output_ms_data = (int)(tastes_output_substances_data[4]/bitter_in_drop * pump_ms_unit);

    //     for(int i=0; i<5; i++)
    //     {
    //         taste_output_ms_data[i] = (int)(tastes_output_substances_data[i]/taste_substance_in_drop[i] * pump_ms_unit);
    //     }
    // }


}
