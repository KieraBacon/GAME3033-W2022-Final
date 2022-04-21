using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public GameObject gameObject { get; }
    public bool canInteract { get; }
    public bool Interact();
}
