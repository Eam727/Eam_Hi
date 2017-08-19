using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HIEngine;
using UnityEngine.Profiling;

public class test : MonoBehaviour
{
    int a = 0;
    void Update()
    {
        if (Input.GetMouseButton(0)&&a==0)
        {
            a = 1;
            hia("models/monster/dog/hound.prefab");
            
        }
        if (Input.GetMouseButton(1) && a == 1)
        {
            a = 0;
            hia("ui/hi1.prefab");

        }
    }
    private void hia(string name)
    {
        Action<UnityEngine.Object> action = (obj) => {
            GameObject a = obj as GameObject;
            a.name = "h";
            Debug.Log("Yes!");
        };
        Profiler.BeginSample("Eamhia");
        ResourceManager.Instance.InstantiateABAssetAsync(name,action);
        Profiler.EndSample();
    }
}
