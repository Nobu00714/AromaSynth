using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSaveWindow : MonoBehaviour
{
    public GameObject saveWindow;
    public void OnClick()
    {
        saveWindow.SetActive(true);
    }
}
