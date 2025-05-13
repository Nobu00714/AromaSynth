using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingM5Number : MonoBehaviour
{
    public BLEwithM5 BLE;
    public TMP_Dropdown SelectM5DropDown;
    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }
    public void OnSelected()
    {
        BLE.DeviceName = SelectM5DropDown.options[SelectM5DropDown.value].text;
        BLE.StartProcess();
    }
}
