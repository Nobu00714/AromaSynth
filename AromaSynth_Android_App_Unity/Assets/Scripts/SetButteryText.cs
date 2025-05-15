using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetButteryText : MonoBehaviour
{
    public TextMeshProUGUI butteryText;
    public BLEwithM5 BLE;
    public int data;

    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BLE != null)
        {
            if (BLE._dataBytes != null)
            {
                data = BLE._dataBytes[0];
                if (data <= 100)
                {
                    butteryText.text = data.ToString();
                }
            }
        }
    }
}
