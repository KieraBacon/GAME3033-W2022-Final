using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowbug : MonoBehaviour
{
    static Glowbug lastGlowbugToFlash;

    [Header("Component References")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Animator animator;
    private int FlashAnimationTriggerHash = Animator.StringToHash("Flash");

    [Header("Flash Properties")]
    [SerializeField, Min(0)]
    private float flashFrequencyMin = 1.0f;
    [SerializeField, Min(0)]
    private float flashFrequencyVariance = 0.0f;
    [SerializeField, Min(0)]
    private float initialFlashDelayVariance = 5.0f;
    [SerializeField, Min(0)]
    private float maxConnectionAngle = 22.5f;
    [SerializeField, Min(0)]
    private float maxConnectionTime = 1.1f;
    [SerializeField, Min(0)]
    private float lineRendererDisplayTime = 0.1f;
    [SerializeField]
    private Color lineRendererColorMin;
    [SerializeField]
    private Color lineRendererColorMax;
    [SerializeField]
    private int lineRendererStepsToMax;
    [SerializeField]
    private float animationFlashDelayPercentage = 0.25f;

    [Header("Internal Flash Properties")]
    private float flashFrequency;
    private float initialFlashDelay;
    private float lastFlashTime;
    private Color lineStartColor => Lerp(lineRendererColorMin, lineRendererColorMax, (float)chainIndex / lineRendererStepsToMax);
    private Color lineEndColor => Lerp(lineRendererColorMin, lineRendererColorMax, (float)(chainIndex + 1) / lineRendererStepsToMax);

    [Header("Connection Properties")]
    private Glowbug connectedGlowbug;
    private float connectedFlashTime = -1;
    private float connectionAngle = 0;
    private int chainIndex = 0;

    private bool withinConnectionTime => connectedFlashTime > 0 && lastFlashTime - connectedFlashTime < maxConnectionTime;
    private bool withinLineRendererTime => Time.time < lastFlashTime + lineRendererDisplayTime;
    private bool shouldShowLineRenderer =>
        TimeManager.slowPercent >= 0.5f &&
        withinConnectionTime &&
        withinLineRendererTime;

    private bool initialized;

    private void OnEnable()
    {
        TimeManager.onTimeAdjusted += OnTimeAdjusted;
    }

    private void OnDisable()
    {
        TimeManager.onTimeAdjusted -= OnTimeAdjusted;
    }

    private void Update()
    {
        if (!initialized) return;
        
        if (withinLineRendererTime)
        {
            if (lineRendererDisplayTime > 0)
            {
                float t = (Time.time - lastFlashTime) / lineRendererDisplayTime;
                t -= animationFlashDelayPercentage;
                if (t < 0)
                    t *= -1;

                Color startColor = lineStartColor;
                startColor.a = Mathf.Lerp(startColor.a, 0, t);
                lineRenderer.startColor = startColor;

                Color endColor = lineStartColor;
                endColor.a = Mathf.Lerp(endColor.a, 0, t);
                lineRenderer.endColor = endColor;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (Time.time > lastFlashTime + flashFrequency)
        {
            Flash();
        }
    }

    public void Randomize()
    {
        flashFrequency = flashFrequencyMin + Random.Range(0, flashFrequencyVariance);
        initialFlashDelay = Random.Range(0, initialFlashDelayVariance);
        lastFlashTime = Time.time + initialFlashDelay;
        initialized = true;
    }

    private void Flash()
    {
        lastFlashTime = Time.time;
        animator.SetTrigger(FlashAnimationTriggerHash);
        if (lastGlowbugToFlash)
        {
            float timeSinceLastFlash = lastFlashTime - lastGlowbugToFlash.lastFlashTime;
            float connectedAngle = 0;
            float angleDifference = 0;

            if (lastGlowbugToFlash.connectedGlowbug)
            {
                connectedAngle = Vector3.SignedAngle(Vector3.forward, lastGlowbugToFlash.gameObject.transform.position - transform.position, Vector3.up);
                if (lastGlowbugToFlash.chainIndex > 0)
                    angleDifference = Mathf.Abs(connectedAngle - lastGlowbugToFlash.connectionAngle);
            }

            if (angleDifference <= maxConnectionAngle && timeSinceLastFlash < maxConnectionTime) // There's a connection
            {

                connectedGlowbug = lastGlowbugToFlash;
                connectedFlashTime = lastGlowbugToFlash.lastFlashTime;
                connectionAngle = connectedAngle;
                chainIndex = lastGlowbugToFlash.chainIndex + 1;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, lastGlowbugToFlash.transform.position + Vector3.up * 0.1f);
                lineRenderer.startColor = Color.clear;
                lineRenderer.endColor = Color.clear;
                //lineRenderer.startColor = Lerp(lineRendererColorMin, lineRendererColorMax, (float)chainIndex / lineRendererStepsToMax);
                //lineRenderer.endColor = Lerp(lineRendererColorMin, lineRendererColorMax, (float)(chainIndex + 1) / lineRendererStepsToMax);
                lineRenderer.enabled = shouldShowLineRenderer;

                if (lastGlowbugToFlash.chainIndex == 0)
                    Debug.Log(Time.time + " chainIndex: " + chainIndex);
            }
            else // There's no connection
            {
                connectedGlowbug = null;
                connectedFlashTime = -1;
                connectionAngle = 0;
                chainIndex = 0;
            }
        }

        lastGlowbugToFlash = this;
    }

    private Color Lerp(Color a, Color b, float t)
    {
        return new Color(
            Mathf.Lerp(a.r, b.r, t),
            Mathf.Lerp(a.g, b.g, t),
            Mathf.Lerp(a.b, b.b, t),
            Mathf.Lerp(a.a, b.a, t));
    }

    private void OnTimeAdjusted()
    {
        lineRenderer.enabled = shouldShowLineRenderer;
    }
}
