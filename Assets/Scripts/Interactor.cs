using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public delegate bool BeforeInteractEventHandler(Interactor interactor);
    public event BeforeInteractEventHandler onBeforeInteraction;
    public delegate void InteractEventHandler(Interactor interactor, IInteractable interactable);
    public event InteractEventHandler onInteract;

    [Header("Interaction")]
    private HashSet<IInteractable> interactablesInRange = new HashSet<IInteractable>();

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInRange.Add(interactable);
            Debug.Log(interactable.gameObject.name + " in range of " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInRange.Remove(interactable);
            Debug.Log(interactable.gameObject.name + " no longer in range of " + gameObject.name);
        }
    }

    public void Interact()
    {
        if (onBeforeInteraction?.Invoke(this)! == true) return;

        IInteractable interactable = GetNearestInteractable();
        if (interactable != null && interactable.Interact())
        {
            onInteract?.Invoke(this, interactable);
        }
    }

    public IInteractable GetNearestInteractable()
    {
        IInteractable nearestInteractable = null;
        float nearestDistance = float.PositiveInfinity;

        foreach (IInteractable interactable in interactablesInRange)
        {
            if (!interactable.canInteract) continue;

            float distance = Vector3.Distance(transform.position, interactable.gameObject.transform.position);
            if (nearestInteractable == null || distance < nearestDistance)
            {
                nearestInteractable = interactable;
                nearestDistance = distance;
            }
        }

        return nearestInteractable;
    }
}
