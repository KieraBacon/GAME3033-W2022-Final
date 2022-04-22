using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    public delegate void TimeEffectEventHandler();
    public static event TimeEffectEventHandler onTimeAdjusted;

    private static Dictionary<GameObject, float> timeEffects = new Dictionary<GameObject, float>();

    private static bool isPaused = false;
    public static bool Paused { get => isPaused; set { isPaused = value; SetTimeScale(); } }
    public static float slowPercent => 1 - Time.timeScale;

    private static float _time;
    public static float time => _time;

    public static void AdjustTime(float delta)
    {
        _time += delta;
    }

    public static void ResetTime()
    {
        _time = 0;
    }

    public static void AdjustTimeScale(GameObject key, float value)
    {
        timeEffects.Add(key, value);
        SetTimeScale();
    }

    public static void RestoreTimeScale(GameObject key)
    {
        timeEffects.Remove(key);
        SetTimeScale();
    }

    private static void SetTimeScale()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            return;
        }

        float adjusted = 1;
        foreach (float value in timeEffects.Values)
            adjusted *= value;
        Time.timeScale = adjusted;

        Debug.Log("Timescale: " + Time.timeScale);
        onTimeAdjusted?.Invoke();
    }
}
