using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalImageGetter : MonoBehaviour
{
    public RawImage image;

    public void GetImage(int maxSize)
    {
        if(NativeGallery.IsMediaPickerBusy())
        {
            Debug.Log("Image getter is busy");
            return;
        }
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path : " + path);
            if(path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if(texture == null){
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                Destroy(image.texture);
                image.texture = texture;
            }
        });
    }

    void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     GetImage(512);
        // }
    }
}
