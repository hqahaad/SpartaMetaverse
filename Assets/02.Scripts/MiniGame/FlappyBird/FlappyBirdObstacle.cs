using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdObstacle : MonoBehaviour
{
    public event Action<GameObject> OnUpdate = delegate { };

    private GameObject[] obstacles = new GameObject[2];
    private Vector2[] originPos = new Vector2[2];

    void Awake()
    {
        obstacles[0] = transform.GetChild(0).gameObject;
        obstacles[1] = transform.GetChild(1).gameObject;

        originPos[0] = new Vector2(obstacles[0].transform.localPosition.x, obstacles[0].transform.localPosition.y);
        originPos[1] = new Vector2(obstacles[1].transform.localPosition.x, obstacles[1].transform.localPosition.y);
    }

    void Update()
    {
        OnUpdate?.Invoke(this.gameObject);
    }

    void OnDisable()
    {
        obstacles[0].transform.localPosition = originPos[0];
        obstacles[1].transform.localPosition = originPos[1];
    }

    public void Spawn(float interval)
    {
        gameObject.SetActive(true);

        var intervalHarf = interval * 0.5f;

        obstacles[0].transform.localPosition += new Vector3(0f, intervalHarf, 0f);
        obstacles[1].transform.localPosition += new Vector3(0f, -intervalHarf, 0f);
    }
}
