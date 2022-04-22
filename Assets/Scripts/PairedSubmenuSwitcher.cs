using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PairedSubmenuSwitcher : SubmenuSwitcher
{
    [System.Serializable]
    struct ButtonSubmenuPair
    {
        public Button button;
        public Submenu submenu;
        public Selectable overrideSelection;
        [HideInInspector]
        public int SubmenuIndex;
    }

    [SerializeField]
    private ButtonSubmenuPair[] buttonSubmenuPairs;

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (ButtonSubmenuPair pair in buttonSubmenuPairs)
        {
            pair.button.onClick.AddListener(() => SwitchSubmenu(GetSubmenuIndex(pair.submenu), pair.overrideSelection));
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (ButtonSubmenuPair pair in buttonSubmenuPairs)
        {
            pair.button.onClick.RemoveListener(() => SwitchSubmenu(GetSubmenuIndex(pair.submenu), pair.overrideSelection));
        }
    }
}
