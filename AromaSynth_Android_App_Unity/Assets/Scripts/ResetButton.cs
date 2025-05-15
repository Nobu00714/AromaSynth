using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public void OnClick()
    {
        AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
        for (int i = 0; i < aromaEqualizer.AromaObjects.Length; i++)
        {
            GameObject graphObject = aromaEqualizer.AromaObjects[i].transform.Find("Graph").gameObject;
            graphObject.GetComponent<RectTransform>().sizeDelta = new Vector2(graphObject.GetComponent<RectTransform>().sizeDelta.x, 0);
        }
    }
}
