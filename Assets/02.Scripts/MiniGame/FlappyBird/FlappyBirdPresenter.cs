using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdPresenter : MonoBehaviour
{
    [SerializeField] private FlappyBird model;
    [SerializeField] private FlappyBirdView view;

    void Awake()
    {
        model.Score.OnValueChanged += (val) =>
        {
            view.UpdateUI(val);
        };
        model.IsGameOver.OnValueChanged += (val) =>
        {
            view.GameOver();
        };
    }
}
