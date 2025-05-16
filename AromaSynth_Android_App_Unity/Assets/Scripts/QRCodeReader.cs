using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using UnityEngine.Android;


public class QRCodeReader : MonoBehaviour
{
    public GameObject cameraWindow;
    public RawImage cameraView;
    private WebCamTexture camTexture;
    private bool isScanning = false;
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }
    public void ActivateScanCamera()
    {
        
        if (!isScanning)
        {
            // カメラ起動
            camTexture = new WebCamTexture();
            cameraView.texture = camTexture;
            // cameraView.GetComponent<RectTransform>().sizeDelta = new Vector2(camTexture.width*100f,camTexture.height*100f);
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
            string scanData = result?.Text;

            // QRコードの内容を20個の数値データに分割し、Aroma Equalizerのグラフの大きさに適用
            AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
            // データをカンマで分割
            string[] QRdataStringArray = scanData.Split(',');
            int[] QRdataIntArray = new int[QRdataStringArray.Length];
            // AromaEqualizerのoutput_ms_dataに変換
            for (int i = 0; i < QRdataStringArray.Length; i++)
            {
                QRdataIntArray[i] = int.Parse(QRdataStringArray[i]);
            }
            aromaEqualizer.updateGraphByData(QRdataIntArray);

            if (result != null)
            {
                // 一度読み取ったら停止
                CancelInvoke(nameof(ScanQR));
                camTexture.Stop();
                isScanning = false;
                cameraWindow.SetActive(false);
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