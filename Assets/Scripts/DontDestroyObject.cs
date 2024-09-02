using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private static DontDestroyObject Instance = null;

    private void Awake()
    {
        if(Instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        Instance = this;

        var obj = FindObjectsOfType<PlayerHealth>();
        if (obj.Length == Constants.DEFAULT_NUMBER_1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
