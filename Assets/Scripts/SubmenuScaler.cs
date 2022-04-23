using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Submenu))]
public class SubmenuScaler : MonoBehaviour
{
    private Submenu submenu;

    [SerializeField]
    private RectTransform panel;
    [SerializeField]
    private float preferredHeight = 400.0f;
    [SerializeField]
    private float transitionTime;
    private Coroutine scalingCoroutine = null;

    private void OnValidate()
    {
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, preferredHeight);
    }

    private void Awake()
    {
        submenu = GetComponent<Submenu>();
    }

    private void OnEnable()
    {
        submenu.onSubmenuStartedOpening += OnSubmenuStartedOpening;
        submenu.onSubmenuStartedClosing += OnSubmenuStartedClosing;
    }

    private void OnDisable()
    {
        submenu.onSubmenuStartedOpening -= OnSubmenuStartedOpening;
        submenu.onSubmenuStartedClosing -= OnSubmenuStartedClosing;
    }

    private void OnSubmenuStartedOpening(Submenu submenu)
    {
        SetHeight(preferredHeight);
    }

    private void OnSubmenuStartedClosing(Submenu submenu)
    {
        Debug.Log(this.gameObject.name + submenu.switcher.activeSubmenu.gameObject.name);
        Submenu openingSubmenu = submenu.switcher.activeSubmenu;
        SubmenuScaler otherScaler = openingSubmenu.GetComponent<SubmenuScaler>();
        if (!openingSubmenu)
        {
            SetHeight(preferredHeight);
            otherScaler?.SetHeight(preferredHeight);
        }
        else
        {
            if (otherScaler)
            {
                float targetHeight = otherScaler.preferredHeight;
                SetHeight(targetHeight);
                otherScaler.SetHeight(targetHeight);
            }
            else
            {
                SetHeight(preferredHeight);
            }
        }
    }

    public void SetHeight(float height)
    {
        if (scalingCoroutine != null)
            StopCoroutine(scalingCoroutine);
        scalingCoroutine = StartCoroutine(DoScalingOfHeight(height, transitionTime));
    }

    private IEnumerator DoScalingOfHeight(float targetHeight, float duration)
    {
        float initialTime = Time.time;
        float initialHeight = panel.sizeDelta.y;
        
        bool complete = false;
        while (!complete)
        {
            float t = (Time.time - initialTime) / duration;
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, Mathf.Lerp(initialHeight, targetHeight, t));
            if (t >= 1)
                complete = true;
            else
                yield return null;
        }

        scalingCoroutine = null;
    }
}
