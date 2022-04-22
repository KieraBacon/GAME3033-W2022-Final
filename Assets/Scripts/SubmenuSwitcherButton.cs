using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SubmenuSwitcherButton : MonoBehaviour
{
    [SerializeField]
    private SubmenuSwitcher submenuSwitcher;
    private Button button;
    [SerializeField]
    private int increment = 1;
    [SerializeField]
    private Selectable navigateOnDisable;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        submenuSwitcher.onSubmenuSwitched += OnSubmenuSwitched;
        OnSubmenuSwitched();
    }

    private void OnDisable()
    {
        submenuSwitcher.onSubmenuSwitched -= OnSubmenuSwitched;
    }

    private void OnSubmenuSwitched()
    {
        if (increment > 0)
            button.interactable = !submenuSwitcher.atLastIndex;
        else if (increment < 0)
            button.interactable = !submenuSwitcher.atFirstIndex;

        if (!button.interactable)
        {
            EventSystem.current.SetSelectedGameObject(navigateOnDisable.gameObject);
        }
    }

    public void Increment()
    {
        submenuSwitcher.SwitchSubmenu(submenuSwitcher.submenuIndex + increment);
    }
}
