using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SubmenuSwitcher : MonoBehaviour
{
    private List<Submenu> submenus;
    [SerializeField]
    private int index = 0;

    private void Awake()
    {
        GetSubmenuChildren();
        SwitchSubmenu(index);
    }

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

    public void SwitchSubmenu(int index)
    {
        index = Mathf.Clamp(index, 0, submenus.Count - 1);
        if (this.index == index && submenus[this.index].isOpen) return;

        submenus[this.index].Close();
        this.index = index;
        submenus[this.index].Open();
    }

    public void SwitchSubmenu(Submenu submenu)
    {
        SwitchSubmenu(GetSubmenuIndex(submenu));
    }

    public int GetSubmenuIndex(Submenu submenu)
    {
        return submenus.FindIndex((Submenu i) => i == submenu);
    }
}
