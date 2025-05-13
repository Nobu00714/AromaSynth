using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBack : MonoBehaviour
{
    public BLEwithM5 BLE;
    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }

    public void Back()
    {
        SceneManager.LoadScene(BLE.beforeSceneName);
    }
}
