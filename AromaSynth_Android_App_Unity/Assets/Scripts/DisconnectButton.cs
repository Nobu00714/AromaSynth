using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectButton : MonoBehaviour
{
    public BLEwithM5 BLE;
    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    public void onClick()
    {
        BLE.SetState(BLEwithM5.States.Disconnect, 0.1f);
    }
}
