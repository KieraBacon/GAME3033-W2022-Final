using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static Dictionary<GameObject, float> timeEffects = new Dictionary<GameObject, float>();

    private static bool isPaused = false;
    public static bool Paused { get => isPaused; set { isPaused = value; SetTimeScale(); } }

    public static void AdjustTime(GameObject key, float value)
    {
        timeEffects.Add(key, value);
        SetTimeScale();
    }

    public static void RestoreTime(GameObject key)
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
    }
}
