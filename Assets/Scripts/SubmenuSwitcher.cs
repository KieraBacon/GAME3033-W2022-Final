using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmenuSwitcher : MonoBehaviour
{
    private List<Submenu> submenus = new List<Submenu>();

    private void Awake()
    {
        foreach (Transform t in transform)
        {
            Debug.Log(t.name);
        }
    }
}
