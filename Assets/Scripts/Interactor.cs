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
    private IInteractable currentInteractionTarget = null;
    private IInteractable persistentInteraction = null;

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

    private void Update()
    {
        if (persistentInteraction == null)
            SetCurrentInteractionTarget(GetNearestInteractable());
    }

    public void Interact()
    {
        if (persistentInteraction != null)
        {
            if (onBeforeInteraction?.Invoke(this)! == true) return;
        }

        IInteractable interactable = GetNearestInteractable();
        if (interactable != null && interactable.Interact())
        {
            onInteract?.Invoke(this, interactable);
        }
    }

    public IInteractable GetNearestInteractable()
    {
        if (persistentInteraction != null)
        {
            return null;
        }
        
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

    private void SetCurrentInteractionTarget(IInteractable interactable)
    {
        if (currentInteractionTarget != null)
        {
            if (interactable == currentInteractionTarget)
                return;

            currentInteractionTarget.SetHighlight(false);
        }

        currentInteractionTarget = interactable;
        if (currentInteractionTarget != null)
        {
            currentInteractionTarget.SetHighlight(true);
        }
    }

    public void SetPersistentInteraction(IInteractable interactable)
    {
        persistentInteraction = interactable;
        
        if (persistentInteraction != null)
        {
            SetCurrentInteractionTarget(null);
        }
    }
}
