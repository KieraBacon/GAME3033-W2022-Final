using UnityEngine;

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

    private void OnValidate()
    {
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, preferredHeight);
    }

    public void Close(bool immediate = false)
    {
        if (!_isOpen) return;

        _isOpen = false;
        if (immediate)
            panel.gameObject.SetActive(false);
        else
            animator.SetTrigger(AnimatorClosedHash);
    }

    public void Open()
    {
        if (_isOpen) return;

        animator.SetTrigger(AnimatorOpenHash);
        _isOpen = true;
    }

    //private IEnumerator DoChangeHeight(float targetHeight)
    //{
    //    float currentHeight = 
    //}
}
