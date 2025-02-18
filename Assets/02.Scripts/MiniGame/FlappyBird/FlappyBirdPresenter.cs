using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdPresenter : MonoBehaviour
{
    [SerializeField] private FlappyBird model;
    [SerializeField] private FlappyBirdView view;

    void Awake()
    {
        model.score.OnValueChanged += (val) =>
        {
            view.UpdateUI(val);
        };
    }
}
