using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public delegate void CanInteractChangedEventHandler(IInteractable caller, bool canInteract);
    public event CanInteractChangedEventHandler onCanInteractChanged;
    public GameObject gameObject { get; }
    public bool canInteract { get; }
    public bool Interact();
    public void SetHighlight(bool value);
}
