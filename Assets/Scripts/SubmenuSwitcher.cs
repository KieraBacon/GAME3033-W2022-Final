using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SubmenuSwitcher : MonoBehaviour
{
    public delegate void SubmenuSwitchedEventHandler();
    public event SubmenuSwitchedEventHandler onSubmenuSwitched;

    private List<Submenu> submenus;
    [SerializeField]
    private int index = 0;
    [SerializeField]
    private bool resetOnEnable = true;
    public int submenuIndex => index;
    public bool atLastIndex => index >= submenus?.Count - 1;
    public bool atFirstIndex => index <= 0;

    protected virtual void Awake()
    {
        GetSubmenuChildren();
    }

    protected virtual void OnEnable()
    {
        SwitchSubmenu(resetOnEnable ? 0 : index, null);
    }

    protected virtual void OnDisable() { }

    private void GetSubmenuChildren()
    {
        submenus = new List<Submenu>();
        foreach (Transform child in transform)
        {
            Submenu submenu = child.GetComponent<Submenu>();
            submenus.Add(submenu);
            submenu.Close(true);
        }
    }

    public void SwitchSubmenu(int index, Selectable overrideSelection = null)
    {
        for (int i = 0; i < submenus.Count; i++)
            if (i != this.index)
                submenus[i].Close();

        index = Mathf.Clamp(index, 0, submenus.Count - 1);
        if (this.index == index && submenus[this.index].isOpen) return;

        submenus[this.index].Close();
        this.index = index;
        submenus[this.index].Open(overrideSelection);

        onSubmenuSwitched?.Invoke();
    }

    public void SwitchSubmenu(Submenu submenu, Selectable overrideSelection = null)
    {
        SwitchSubmenu(GetSubmenuIndex(submenu), overrideSelection);
    }

    public int GetSubmenuIndex(Submenu submenu)
    {
        return submenu == null ? 0 : submenus.FindIndex((Submenu i) => i == submenu);
    }

    public void Next()
    {
        SwitchSubmenu(index + 1);
    }

    public void Previous()
    {
        SwitchSubmenu(index - 1);
    }
}
