using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendDataButton : MonoBehaviour
{
    public BLEwithM5 BLE;
    public Text sendTextDisplay;
    public AromaEqualizer aromaEqualizer;
    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    public void OnClick()
    {
        if (BLE != null)
        {
            // tasteEqualizerのミリ秒データをM5に送信するためにカンマ区切りstringに変換
            string sendData = "";
            for (int i = 0; i < aromaEqualizer.output_ms_data.Length; i++)
            {
                sendData += aromaEqualizer.output_ms_data[i];
                if (i != aromaEqualizer.output_ms_data.Length - 1)
                {
                    sendData += ",";
                }
            }
            // BLEでデータを送信
            BLE.SendByte(sendData);
            // 送信データのテキストを画面上で更新
            sendTextDisplay.text = sendData;
        }
        else
        {
            Debug.LogError("BLE is null");
        }
    }
}
