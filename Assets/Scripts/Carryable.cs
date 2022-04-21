using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour, IInteractable
{
    private Carrier carrier;
    public bool isBeingCarried => carrier;
    public bool canInteract => !isBeingCarried;

    public bool Interact()
    {
        return canInteract;
    }

    public void PickUp(Carrier carrier)
    {
        this.carrier = carrier;
    }

    public void Drop()
    {
        carrier = null;
    }
}
