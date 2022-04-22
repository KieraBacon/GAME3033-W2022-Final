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
            animator.SetTrigger(AnimatorClosedHash);
        }
    }

    public void Open(Selectable overrideSelection = null)
    {
        if (_isOpen) return;

        panel.gameObject.SetActive(true);
        animator.SetTrigger(AnimatorOpenHash);
        if (initialSelection)
            EventSystem.current.SetSelectedGameObject(overrideSelection ? overrideSelection.gameObject : initialSelection.gameObject);

        _isOpen = true;
        onSubmenuStartedOpening?.Invoke(this);
    }

    private IEnumerator DoTransitioning(bool open)
    {
        animator.SetTrigger(open ? AnimatorOpenHash : AnimatorClosedHash);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

        if (open)
        {
            panel.gameObject.SetActive(true);
            onSubmenuFinishedOpening?.Invoke(this);
        }
        else
        {
            panel.gameObject.SetActive(false);
            onSubmenuFinishedClosing?.Invoke(this);
        }
    }
}
