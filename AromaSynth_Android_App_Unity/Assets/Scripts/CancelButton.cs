using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    public void OnClick()
    {
        targetObject.SetActive(false);
    }
}
