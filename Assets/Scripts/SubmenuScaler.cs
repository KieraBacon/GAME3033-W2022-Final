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
        Submenu openingSubmenu = submenu.switcher.activeSubmenu;
        if (!openingSubmenu)
        {
            SetHeight(preferredHeight);
        }
        else
        {
            float targetHeight = openingSubmenu.GetComponent<SubmenuScaler>().preferredHeight;
            SetHeight(targetHeight);
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
        float t = (Time.time - initialTime) / duration;

        while (t < 1)
        {
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, Mathf.Lerp(initialHeight, targetHeight, t));
            yield return null;
        }
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, Mathf.Lerp(initialHeight, targetHeight, 1));

        scalingCoroutine = null;
    }
}
