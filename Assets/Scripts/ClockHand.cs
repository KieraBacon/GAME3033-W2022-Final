using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHand : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private int WiggleAnimationTriggerHash = Animator.StringToHash("Wiggle");
    private int timeLastUpdate;

    private void Update()
    {
        int time = (int)(TimeManager.time % 60);

        if (time != timeLastUpdate)
        {
            animator.SetTrigger(WiggleAnimationTriggerHash);
            transform.rotation = Quaternion.AngleAxis(time * 6, Vector3.up);
        }

        timeLastUpdate = time;
    }
}
