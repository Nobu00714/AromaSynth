using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatusTextChanger : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public BLEwithM5 BLE;

    void Start()
    {
        statusText.text = "Status";
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    void Update()
    {
        statusText.text = BLE.statusString;
    }
}
