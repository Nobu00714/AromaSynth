using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayReceivedData : MonoBehaviour
{
    public Text textDisplay;
    public int data;
    public BLEwithM5 BLE;

    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    void Update()
    {
        if(BLE._dataBytes!=null)
        {
            data = BLE._dataBytes[0];
            textDisplay.text = data.ToString();
        }
    }
}
