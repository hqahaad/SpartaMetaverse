using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlappyBirdView : MonoBehaviour
{
    private TMP_Text _text;

    void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void UpdateUI(float value)
    {
        _text.text = "Score : " + value.ToString();
    }
}
