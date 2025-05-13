using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImageButton : MonoBehaviour
{
    public LocalImageGetter localImageGetter;

    public void OnClick()
    {
        localImageGetter.GetImage(512);
    }
}