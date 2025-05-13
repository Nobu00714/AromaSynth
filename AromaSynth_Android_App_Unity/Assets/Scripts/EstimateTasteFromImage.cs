using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

public class EstimateTasteFromImage : MonoBehaviour
{
    public GPTTasteEstimator gptTasteEstimator;
    public RawImage foodImage;
    public GameObject GO_Loading;

    public void OnClick()
    {
        Texture2D sendingData = (Texture2D)(foodImage.texture);
        byte[] data_byte = createReadableTexture2D(sendingData).EncodeToJPG();
        string data_encoded = System.Convert.ToBase64String(data_byte);
        gptTasteEstimator.imageTasteEstimation(data_encoded);
        GO_Loading.SetActive(true);
    }

    Texture2D createReadableTexture2D(Texture2D texture2d)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
                    texture2d.width,
                    texture2d.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(texture2d, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D readableTextur2D = new Texture2D(texture2d.width, texture2d.height);
        readableTextur2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTextur2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);
        return readableTextur2D;
    }
}
