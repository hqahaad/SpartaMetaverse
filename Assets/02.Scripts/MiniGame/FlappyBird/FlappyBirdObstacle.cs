using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdObstacle : MonoBehaviour
{
    public event Action<GameObject> OnUpdate = delegate { };

    private GameObject[] _obstacles = new GameObject[2];
    private Vector2[] _originPos = new Vector2[2];

    void Awake()
    {
        _obstacles[0] = transform.GetChild(0).gameObject;
        _obstacles[1] = transform.GetChild(1).gameObject;

        _originPos[0] = new Vector2(_obstacles[0].transform.localPosition.x, _obstacles[0].transform.localPosition.y);
        _originPos[1] = new Vector2(_obstacles[1].transform.localPosition.x, _obstacles[1].transform.localPosition.y);
    }

    void Update()
    {
        OnUpdate?.Invoke(this.gameObject);
    }

    void OnDisable()
    {
        _obstacles[0].transform.localPosition = _originPos[0];
        _obstacles[1].transform.localPosition = _originPos[1];
    }

    public void Spawn(float interval)
    {
        gameObject.SetActive(true);

        var intervalHarf = interval * 0.5f;

        _obstacles[0].transform.localPosition += new Vector3(0f, intervalHarf, 0f);
        _obstacles[1].transform.localPosition += new Vector3(0f, -intervalHarf, 0f);
    }
}
