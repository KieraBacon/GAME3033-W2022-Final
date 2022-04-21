using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHand : MonoBehaviour
{
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        time %= 60;

        transform.rotation = Quaternion.AngleAxis((int)time * 6, Vector3.up);
    }
}
