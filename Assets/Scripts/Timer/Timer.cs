using System;
using UnityEngine;

public class Timer {
    TimerComponent tc;

    public Timer(GameObject go, Action action, float time, int repetitions = 1) {
        tc = go.AddComponent<TimerComponent>();

        tc.ActionToDo = action;
        tc.TimeToWait = time;
        tc.RepetitionsNumber = repetitions;
    }

    public void StopTimer()
    {
        tc.TimeToWait = 100000;
        tc.RepetitionsNumber = 0;
    }
}