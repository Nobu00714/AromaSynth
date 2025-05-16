using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCodeGenerator : MonoBehaviour
{
    public RawImage qrImageUI;
    private string qrText = "Hello, World!";
    public Texture2D GenerateQRCode(string text)
    {
        // QRコードの生成
        var qrWriter = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 256,
                Width = 256,
                Margin = 1
            }
        };

        // ビットマップ作成
        var color32 = qrWriter.Write(text);
        Texture2D qrTexture = new Texture2D(256, 256);
        qrTexture.SetPixels32(color32);
        qrTexture.Apply();

        return qrTexture;
    }
    public void updateQR(string newText)
    {
        qrText = newText;
        qrImageUI.texture = GenerateQRCode(qrText);
    }
}
