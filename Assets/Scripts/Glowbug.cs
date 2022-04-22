using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowbug : MonoBehaviour
{
    static Glowbug lastGlowbugToFlash;

    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField, Min(0)]
    private float lineRendererDisplayTime;
    [SerializeField]
    private Animator animator;
    private int FlashAnimationTriggerHash = Animator.StringToHash("Flash");
    [SerializeField, Min(0)]
    private float flashFrequencyMin;
    [SerializeField, Min(0)]
    private float flashFrequencyVariance;
    [SerializeField, Min(0)]
    private float initialFlashDelayVariance;
    [SerializeField, Min(0)]
    private float maxConnectionTime;

    private float flashFrequency;
    private float initialFlashDelay;
    private float _lastFlashTime;
    public float lastFlashTime => _lastFlashTime;
    private float connectedFlashTime = -1;
    private bool withinConnectionTime => connectedFlashTime > 0 && _lastFlashTime - connectedFlashTime < maxConnectionTime;
    private bool withinLineRendererTime => Time.time < _lastFlashTime + lineRendererDisplayTime;
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
        
        if (!withinLineRendererTime)
            lineRenderer.enabled = false;

        if (Time.time > _lastFlashTime + flashFrequency)
        {
            Flash();
        }
    }

    public void Randomize()
    {
        flashFrequency = flashFrequencyMin + Random.Range(0, flashFrequencyVariance);
        initialFlashDelay = Random.Range(0, initialFlashDelayVariance);
        _lastFlashTime = Time.time + initialFlashDelay;
        initialized = true;
    }

    private void Flash()
    {
        _lastFlashTime = Time.time;
        animator.SetTrigger(FlashAnimationTriggerHash);
        if (lastGlowbugToFlash)
        {
            float timeSinceLastFlash = _lastFlashTime - lastGlowbugToFlash.lastFlashTime;

            if (timeSinceLastFlash < maxConnectionTime)
            {
                connectedFlashTime = lastGlowbugToFlash.lastFlashTime;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, lastGlowbugToFlash.transform.position + Vector3.up * 0.1f);
                lineRenderer.enabled = shouldShowLineRenderer;
            }
        }

        lastGlowbugToFlash = this;
    }

    private void OnTimeAdjusted()
    {
        lineRenderer.enabled = shouldShowLineRenderer;
    }
}
