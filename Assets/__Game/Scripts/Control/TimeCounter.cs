using System;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private float remainingTime;
    private Action onTimeEnd;
    private bool isRunning;

    public bool IsRunning => isRunning;

    public void Run(Action action, float delay)
    {
        if (!isRunning)
        {
            onTimeEnd = action;
            remainingTime = delay;
            isRunning = true;
        }
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Excute(float deltaTime)
    {
        if (!isRunning) return;

        remainingTime -= deltaTime;
        if (remainingTime <= 0)
        {
            isRunning = false;
            onTimeEnd?.Invoke();
        }
    }
}

public enum EPlayerState
{
    Idle = 0,
    Moving = 1,
    Attack = 2
}