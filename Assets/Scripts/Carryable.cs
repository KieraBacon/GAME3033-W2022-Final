using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour, IInteractable
{
    public event IInteractable.CanInteractChangedEventHandler onCanInteractChanged;

    [SerializeField]
    private Outline outline;
    private Carrier carrier;
    public bool isBeingCarried => carrier;
    public bool canInteract => !isBeingCarried;
    [SerializeField]
    private Rigidbody _rigidbody;
    public Rigidbody rigidbody => _rigidbody;

    public bool Interact()
    {
        return canInteract;
    }

    public void SetHighlight(bool value)
    {
        outline.enabled = value;
    }

    public void PickUp(Carrier carrier)
    {
        if (this.carrier == carrier) return;

        this.carrier = carrier;
        onCanInteractChanged?.Invoke(this, canInteract);
    }

    public void Drop()
    {
        carrier = null;
        onCanInteractChanged?.Invoke(this, canInteract);
    }
}
