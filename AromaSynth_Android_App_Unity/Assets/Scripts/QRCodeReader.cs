using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using UnityEngine.Android;


public class QRCodeReader : MonoBehaviour
{
    public string scanData;
    public RawImage cameraView;
    private WebCamTexture camTexture;
    private bool isScanning = false;
    public void ActivateScanCamera()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        if (!isScanning)
        {
            // カメラ起動
            camTexture = new WebCamTexture();
            cameraView.texture = camTexture;
            cameraView.material.mainTexture = camTexture;
            camTexture.Play();

            // スキャン開始
            InvokeRepeating(nameof(ScanQR), 1f, 0.5f);  // 0.5秒間隔でスキャン
        }
    }
    void ScanQR()
    {
        if (isScanning || camTexture.width <= 16) return;  // カメラ初期化待ち

        isScanning = true;

        try
        {
            // カメラ画像取得
            Color32[] pixels = camTexture.GetPixels32();
            int width = camTexture.width;
            int height = camTexture.height;

            // ZXingでQRコードデコード
            var reader = new BarcodeReader
            {
                AutoRotate = false,
                TryInverted = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new[] { BarcodeFormat.QR_CODE }
                }
            };
            ZXing.Result result = reader.Decode(pixels, width, height);
            scanData = result?.Text;

            if (result != null)
            {
                CancelInvoke(nameof(ScanQR));  // 一度読み取ったら停止
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("QR decode error: " + e.Message);
        }

        isScanning = false;
    }
    public void StopScan()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
            CancelInvoke(nameof(ScanQR));
            isScanning = false;
        }
    }
    void OnDestroy()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
        }
    }

}