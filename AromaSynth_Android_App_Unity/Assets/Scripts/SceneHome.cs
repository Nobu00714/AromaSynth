using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHome : MonoBehaviour
{
    public string scenename;
    void Start()
    {
        SceneManager.LoadScene(scenename);
    }
}
