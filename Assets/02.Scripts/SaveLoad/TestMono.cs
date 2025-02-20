using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMono : MonoBehaviour, IBind<PositionData>
{
    public PositionData data;

    void Awake()
    {
        SaveLoader.Instance.Save();
    }

    public void Bind(PositionData t)
    {
        data = t;
    }

    void Update()
    {
        
    }
}