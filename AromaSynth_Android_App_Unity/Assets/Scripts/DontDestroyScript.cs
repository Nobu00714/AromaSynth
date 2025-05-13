using UnityEngine;

public class DontDestroyScript: MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}