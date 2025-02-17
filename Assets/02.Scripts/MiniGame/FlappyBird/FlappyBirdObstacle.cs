using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdObstacle : MonoBehaviour
{
    public event Action<GameObject> OnUpdate = delegate { };

    private GameObject[] _obstacles = new GameObject[2];

    void Awake()
    {
        _obstacles[0] = transform.GetChild(0).gameObject;
        _obstacles[1] = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        OnUpdate?.Invoke(this.gameObject);
    }

    public void Spawn(float interval)
    {
        gameObject.SetActive(true);

        var intervalHarf = interval * 0.5f;

        _obstacles[0].transform.localPosition += new Vector3(0f, intervalHarf, 0f);
        _obstacles[1].transform.localPosition += new Vector3(0f, -intervalHarf, 0f);
    }

}
