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
        [HideInInspector]
        public int SubmenuIndex;
    }

    [SerializeField]
    private ButtonSubmenuPair[] buttonSubmenuPairs;

    private void OnEnable()
    {
        foreach (ButtonSubmenuPair pair in buttonSubmenuPairs)
        {
            pair.button.onClick.AddListener(() => SwitchSubmenu(GetSubmenuIndex(pair.submenu)));
        }
    }

    private void OnDisable()
    {
        foreach (ButtonSubmenuPair pair in buttonSubmenuPairs)
        {
            pair.button.onClick.RemoveListener(() => SwitchSubmenu(GetSubmenuIndex(pair.submenu)));
        }
    }
}
