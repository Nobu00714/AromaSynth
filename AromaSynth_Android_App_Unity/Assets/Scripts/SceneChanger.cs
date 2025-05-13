using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public BLEwithM5 BLE;
    public string sceneName = "";
    void Start()
    {
        BLE = GameObject.Find("BLE Manager").gameObject.GetComponent<BLEwithM5>();
    }
    public void OnClick()
    {
        BLE.beforeSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
