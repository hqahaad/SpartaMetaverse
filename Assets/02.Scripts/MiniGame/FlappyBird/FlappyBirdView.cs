using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlappyBirdView : MonoBehaviour
{
    private TMP_Text tmpText;

    void Awake()
    {
        tmpText = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void UpdateUI(float value)
    {
        tmpText.text = "Score : " + value.ToString();
    }
}
