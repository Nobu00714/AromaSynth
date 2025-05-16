using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadButton : MonoBehaviour
{
    public GameObject uploadWindow;
    public void OnClick()
    {
        // 必要なコンポーネントを取得
        AromaEqualizer aromaEqualizer = GameObject.Find("Aroma Equalizer").GetComponent<AromaEqualizer>();
        QRCodeGenerator qrCodeGenerator = GameObject.Find("QRCodeManager").GetComponent<QRCodeGenerator>();

        // 現状のUIに表示されているデータを出力情報としてQRコードを生成
        string data = string.Join(",", aromaEqualizer.output_ms_data);
        qrCodeGenerator.updateQR(data);

        // ウィンドウを表示
        uploadWindow.SetActive(true);
    }
}
