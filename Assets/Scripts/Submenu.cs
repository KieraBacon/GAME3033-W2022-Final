using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Submenu : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private int AnimatorOpenHash = Animator.StringToHash("Open");
    private int AnimatorClosedHash = Animator.StringToHash("Closed");
    [SerializeField]
    private RectTransform panel;
    [SerializeField]
    private float preferredHeight = 400.0f;
    private bool _isOpen = false;
    public bool isOpen => _isOpen;
    [SerializeField]
    private Selectable initialSelection;

    private void OnValidate()
    {
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, preferredHeight);
    }

    public void Close(bool immediate = false)
    {
        if (!immediate && !_isOpen) return;

        _isOpen = false;
        if (immediate)
        {
            panel.gameObject.SetActive(false);
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
    }
}
