using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindow : MonoBehaviour
{
    public GameObject window;
    public void OnClick()
    {
        window.SetActive(true);
    }
}
