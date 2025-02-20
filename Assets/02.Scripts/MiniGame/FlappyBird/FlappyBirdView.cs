using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlappyBirdView : MonoBehaviour
{
    private TMP_Text scoreTMP;
    private TMP_Text timerTMP;
    private GameObject gameOverBoard;
    private CountdownTimer countdownTimer = new(delayTime);


    private const float delayTime = 4;

    void Awake()
    {
        scoreTMP = transform.GetChild(0).GetComponent<TMP_Text>();
        timerTMP = transform.GetChild(1).GetComponent<TMP_Text>();
        gameOverBoard = transform.GetChild(2).gameObject;

    }

    void Start()
    {
        Time.timeScale = 0f;

        countdownTimer.OnTimerStop += () =>
        {
            timerTMP.gameObject.SetActive(false);
            Time.timeScale = 1f;
        };
        countdownTimer.Start();
        countdownTimer.OnTimerUpdate += () =>
        {
            if (timerTMP == null)
                return;

            timerTMP.text = countdownTimer.TimerTime.ToString("0");
        };
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void UpdateUI(float value)
    {
        if (scoreTMP == null)
            return;

        scoreTMP.text = "Score : " + value.ToString();
    }

    public void GameOver()
    {
        gameOverBoard.SetActive(true);
    }
}
