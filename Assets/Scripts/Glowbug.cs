using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowbug : MonoBehaviour
{
    //static Glowbug lastGlowbugToFlash;
    static HashSet<Glowbug> allGlowbugs = new HashSet<Glowbug>();

    [Header("Component References")]
    [SerializeField]
    private GlowbugRangeFinder _rangeFinder;
    public GlowbugRangeFinder rangeFinder => _rangeFinder;
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
    private Gradient lineRendererStepsGradient;
    private int lineRendererStepsToMax => allGlowbugs.Count;
    [SerializeField]
    private float animationFlashDelayPercentage = 0.25f;

    [Header("Internal Flash Properties")]
    private float flashFrequency;
    private float initialFlashDelay;
    private float _lastFlashTime;
    public float lastFlashTime => _lastFlashTime;
    private Color lineStartColor => lineRendererStepsGradient.Evaluate((float)connectionChainIndex / lineRendererStepsToMax);
    private Color lineEndColor => lineRendererStepsGradient.Evaluate((float)(connectionChainIndex + 1)/ lineRendererStepsToMax);

    [Header("Connection Properties")]
    private Glowbug connectedGlowbug;
    private float connectedFlashTime = -1;
    private float connectionAngle = 0;
    private int connectionChainIndex = 0;
    [SerializeField, Min(0)]
    private int minChainedConnectionsToProgress = 2;

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
        allGlowbugs.Add(this);
    }



    private void OnDisable()
    {
        TimeManager.onTimeAdjusted -= OnTimeAdjusted;
        allGlowbugs.Remove(this);
    }

    private void Update()
    {
        if (!initialized) return;
        
        if (withinLineRendererTime)
        {
            if (lineRendererDisplayTime > 0)
            {
                float t = (Time.time - _lastFlashTime) / lineRendererDisplayTime;
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

        Glowbug lastGlowbugToFlash = GetLastGlowbugToFlashInRange();

        // If another Glowbug has flashed previously to this one
        if (lastGlowbugToFlash)
        {
            float timeSinceLastFlash = _lastFlashTime - lastGlowbugToFlash._lastFlashTime; // The amount of time since the last flash
            float connectedAngle = Vector3.SignedAngle(Vector3.forward, lastGlowbugToFlash.transform.position - transform.position, Vector3.up); // The signed angle, from Vector3.forward, between this Glowbug and the last one
            float angleDifference = 0; // The absolute difference in angle between this Glowbug's connection to the last one, and the last one's connection to the one before it

            if (lastGlowbugToFlash.connectionChainIndex > 0) // If the last Glowbug was in a chain with at least one other - IE, it has a meaningful connectionAngle
            {
                angleDifference = Mathf.Abs(connectedAngle - lastGlowbugToFlash.connectionAngle);
            }

            if (angleDifference <= maxConnectionAngle && timeSinceLastFlash < maxConnectionTime) // There's a connection
            {
                connectedGlowbug = lastGlowbugToFlash;
                connectedFlashTime = lastGlowbugToFlash._lastFlashTime;
                connectionAngle = connectedAngle;
                
                connectionChainIndex = lastGlowbugToFlash.connectionChainIndex + 1;

                float chainImpact = (1 / (allGlowbugs.Count - 2)) * flashFrequency; // If all Glowbugs are in a chain, then time progresses by 1 second per second.
                if (connectionChainIndex > 1)
                    TimeManager.AdjustTime(chainImpact);

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, lastGlowbugToFlash.transform.position + Vector3.up * 0.1f);
                lineRenderer.startColor = Color.clear;
                lineRenderer.endColor = Color.clear;
                lineRenderer.enabled = shouldShowLineRenderer;
            }
            else // There's no connection
            {
                connectedGlowbug = null;
                connectedFlashTime = -1;
                connectionAngle = 0;
                TimeManager.AdjustTime(-TimeManager.time);
                connectionChainIndex = 0;
            }
        }

        //lastGlowbugToFlash = this;

        string text = "";
        foreach (Glowbug glowbug in FindObjectsOfType<Glowbug>())
        {
            text += " " + glowbug.name + ": " + glowbug.connectionChainIndex + "\n";
        }
        FindObjectOfType<TMPro.TextMeshProUGUI>().text = text;
    }

    private void OnTimeAdjusted()
    {
        lineRenderer.enabled = shouldShowLineRenderer;
    }

    private Glowbug GetLastGlowbugToFlashInRange()
    {
        Glowbug lastToFlash = null;
        float lastTime = 0;

        foreach (Glowbug glowbug in rangeFinder.glowbugsInRange)
        {
            if (glowbug == this) continue;

            float time = glowbug._lastFlashTime;
            if (!lastToFlash || time > lastTime)
            {
                lastToFlash = glowbug;
                lastTime = time;
            }
        }

        return lastToFlash;
    }
}
