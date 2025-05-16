using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadButton : MonoBehaviour
{
    public GameObject cameraWindow;
    public void OnClick()
    {
        cameraWindow.SetActive(true);
    }
}
