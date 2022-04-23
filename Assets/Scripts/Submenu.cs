using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Submenu : MonoBehaviour
{
    public delegate void SubmenuFinishedClosingEventHandler(Submenu submenu);
    public event SubmenuFinishedClosingEventHandler onSubmenuStartedOpening;
    public event SubmenuFinishedClosingEventHandler onSubmenuFinishedOpening;
    public event SubmenuFinishedClosingEventHandler onSubmenuStartedClosing;
    public event SubmenuFinishedClosingEventHandler onSubmenuFinishedClosing;

    [SerializeField]
    private Animator animator;
    [HideInInspector]
    public SubmenuSwitcher switcher;
    private int AnimatorOpenHash = Animator.StringToHash("Open");
    private int AnimatorClosedHash = Animator.StringToHash("Closed");
    //private int AnimatorSubmenuFadeOutAnimationHash = Animator.StringToHash("Submenu Fade Out Animation");
    
    [SerializeField]
    private RectTransform panel;
    private bool _isOpen = false;
    public bool isOpen => _isOpen;
    [SerializeField]
    private Selectable initialSelection;
    [SerializeField]
    private bool slowInitialOpening = false;
    private Coroutine transitionCoroutine = null;

    //private void Awake()
    //{
    //    animator.Play(AnimatorSubmenuFadeOutAnimationHash, 0, 1);
    //}

    public void Close(bool immediate = false)
    {
        if (!immediate && !_isOpen) return;

        _isOpen = false;
        onSubmenuStartedClosing?.Invoke(this);

        if (immediate)
        {
            animator.ResetTrigger(AnimatorOpenHash);
            animator.ResetTrigger(AnimatorClosedHash);
            //animator.Play(AnimatorSubmenuFadeOutAnimationHash, 0, 1);
            panel.gameObject.SetActive(false);
            onSubmenuFinishedClosing?.Invoke(this);
        }
        else
        {
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = null;
            }
            transitionCoroutine = StartCoroutine(DoTransitioningOut());
            //animator.SetTrigger(AnimatorClosedHash);
        }
    }

    public void Open(Selectable overrideSelection = null)
    {
        if (_isOpen) return;

        panel.gameObject.SetActive(true);

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }
        transitionCoroutine = StartCoroutine(DoTransitioningIn());

        if (initialSelection)
            EventSystem.current.SetSelectedGameObject(overrideSelection ? overrideSelection.gameObject : initialSelection.gameObject);

        _isOpen = true;
        onSubmenuStartedOpening?.Invoke(this);
    }

    private IEnumerator DoTransitioningIn()
    {
        animator.SetTrigger(AnimatorOpenHash);

        if (slowInitialOpening)
            animator.speed = 0.25f;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

            panel.gameObject.SetActive(true);
            onSubmenuFinishedOpening?.Invoke(this);

        if (slowInitialOpening)
        {
            animator.speed = 1.0f;
            slowInitialOpening = false;
        }
        transitionCoroutine = null;
    }

    private IEnumerator DoTransitioningOut()
    {
        animator.SetTrigger(AnimatorClosedHash);

        yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

        panel.gameObject.SetActive(false);
        onSubmenuFinishedClosing?.Invoke(this);

        transitionCoroutine = null;
    }
}
