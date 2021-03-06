using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public Submenu activeSubmenu
    {
        get
        {
            if (submenus == null || index < 0 || index > submenus.Count - 1)
                return null;
            return submenus[index];
        }
    }
        

    protected virtual void Awake()
    {
        GetSubmenuChildren();
        foreach (Submenu submenu in submenus)
        {
            submenu.onSubmenuFinishedClosing += OnSubmenuFinishedClosing;
        }
    }

    private void OnDestroy()
    {
        foreach (Submenu submenu in submenus)
        {
            submenu.onSubmenuFinishedClosing -= OnSubmenuFinishedClosing;
        }
    }

    protected virtual void OnEnable()
    {
        SwitchSubmenu(resetOnEnable || index < 0 ? 0 : index, null);
    }

    protected virtual void OnDisable()
    {
        
    }

    private void GetSubmenuChildren()
    {

        submenus = new List<Submenu>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Submenu submenu = transform.GetChild(i)?.GetComponent<Submenu>();
            submenu.switcher = this;
            submenus.Add(submenu);
            if (i != index)
                submenu.Close(true);
        }
    }

    public void SwitchSubmenu(int index, Selectable overrideSelection = null)
    {
        //for (int i = 0; i < submenus.Count; i++)
        //    if (i != this.index && i != index)
        //        submenus[i].Close();

        index = Mathf.Clamp(index, 0, submenus.Count - 1);
        if (this.index == index && submenus[this.index].isOpen) return;

        int oldIndex = this.index;
        this.index = index;
        if (oldIndex >= 0)
            submenus[oldIndex].Close();
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

    private void OnSubmenuFinishedClosing(Submenu caller)
    {
        int openSubmenus = 0;
        foreach (Submenu submenu in submenus)
        {
            if (submenu.isOpen)
                ++openSubmenus;
        }

        if (openSubmenus <= 0)
            gameObject.SetActive(false);
    }

    public void Close()
    {
        foreach (Submenu submenu in submenus)
        {
            submenu.Close();
        }
        index = -1;
    }
}
