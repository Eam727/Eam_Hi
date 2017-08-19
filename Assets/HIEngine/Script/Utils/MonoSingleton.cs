﻿using UnityEngine;
using System.Collections;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static string rootName = "MonoSingleton";
    private static GameObject monoSingletionRoot;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (monoSingletionRoot == null)
            {
                monoSingletionRoot = GameObject.Find(rootName);
                if (monoSingletionRoot == null)
                {
                    monoSingletionRoot = new GameObject(rootName);
                    DontDestroyOnLoad(monoSingletionRoot);
                }
                if (monoSingletionRoot == null) Debug.Log("please create a gameobject named " + rootName);
            }
            if (instance == null)
            {
                instance = monoSingletionRoot.GetComponent<T>();
                if (instance == null) instance = monoSingletionRoot.AddComponent<T>();
            }
            return instance;
        }
    }

}