using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetConnectingM5Text : MonoBehaviour
{
    public TextMeshProUGUI ConnectingM5Text;
    public BLEwithM5 BLE;
    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    void Update()
    {
        ConnectingM5Text.text = BLE.DeviceName;
    }
}
