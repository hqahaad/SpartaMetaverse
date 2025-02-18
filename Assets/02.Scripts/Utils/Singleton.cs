using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
            }

            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).Name + "Created";
                _instance = go.AddComponent<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake() => Initialize();

    protected virtual void Initialize()
    {
        if (!Application.isPlaying)
            return;

        transform.SetParent(null);

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            enabled = true;
        }
        else
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }
    }
}
