using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class Timer
{
    protected float time;
    public float TimerTime => time;
    public event Action OnTimerStart = delegate { };
    public event Action OnTimerUpdate = delegate { };
    public event Action OnTimerStop = delegate { };

    public abstract void Start(bool ignoreTimeScale);

    protected void OnTimerStartInvoke()
    {
        OnTimerStart?.Invoke();
    }

    protected void OnTimerUpdateInvoke()
    {
        OnTimerUpdate?.Invoke();
    }

    protected void OnTimerStopInvoke()
    {
        OnTimerStop?.Invoke();
    }
}

public class CountdownTimer : Timer
{
    protected float internalTime;

    public CountdownTimer(float time)
    {
        this.time = time;
        internalTime = time;
    }

    public override void Start(bool ignoreTimeScale = true)
    {
        StartAsync(ignoreTimeScale).Forget();
    }

    protected async UniTask StartAsync(bool ignoreTimeScale = true)
    {
        OnTimerStartInvoke();

        while (time > 0f)
        {
            await UniTask.NextFrame(PlayerLoopTiming.Update);

            time -= ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            OnTimerUpdateInvoke();
        }

        OnTimerStopInvoke();
    }

    public void Reset(float newTime, bool ignoreTimeScale = true)
    {
        time = newTime;
    }

    public void Reset(bool ignoreTimeScale = true)
    {
        time = internalTime;
    }
}