using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EstimateTasteFromText : MonoBehaviour
{
    public GPTTasteEstimator gptTasteEstimator;
    public TMP_InputField inputText;
    public GameObject GO_Loading;

    public void OnClick()
    {
        string data_text = inputText.text;
        gptTasteEstimator.textTasteEstimation(data_text);
        GO_Loading.SetActive(true);
    }
}
