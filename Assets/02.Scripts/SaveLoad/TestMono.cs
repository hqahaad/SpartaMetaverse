using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMono : MonoBehaviour, IBind<PositionData>
{
    public PositionData data;

    public void Bind(PositionData t)
    {
        data = t;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Saver!");
            SaveLoader.Instance.Save(data);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Load!");
            //data = SaveLoader.Instance.Load(data.name);
        }
    }
}

[System.Serializable]
public class PositionData : GameData, ISavable
{
    public float x = 1;
    public float y;
}

public interface ISavable
{

}

public interface IBind<T> where T : ISavable
{
    void Bind(T t);
}