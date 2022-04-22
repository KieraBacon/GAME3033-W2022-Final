using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submenu : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private int AnimatorFadeOutHash = Animator.StringToHash("Fade Out");

    public void Close()
    {
        animator.SetTrigger(AnimatorFadeOutHash);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
