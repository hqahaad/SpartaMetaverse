using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
            }

            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).Name + "Created";
                instance = go.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake() => Initialize();

    protected virtual void Initialize()
    {
        if (!Application.isPlaying)
            return;

        transform.SetParent(null);

        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            enabled = true;
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }
}
