using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
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
    [SerializeField, Range(0, 1), Tooltip("The degree to which the target algorithm prioritizes the interactor's facing over distance. If set to 1, the interactor will always select the interactable closest to the interator's forward position.")]
    private float interactionFacingBias = 0.5f;
    private float maxDistance;
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

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
        float highestValue = 0;
        float maxDistance = collider.bounds.size.magnitude / 2;

        foreach (IInteractable interactable in interactablesInRange)
        {
            if (!interactable.canInteract) continue;

            float distance = Vector3.Distance(transform.position, interactable.gameObject.transform.position);
            float angle = Vector3.Angle(transform.forward, interactable.gameObject.transform.position - transform.position);
            float value = ((1 - (angle / 180)) * interactionFacingBias) + (((maxDistance - distance) / maxDistance) * (1 - interactionFacingBias));

            if (nearestInteractable == null || value > highestValue)
            {
                nearestInteractable = interactable;
                highestValue = value;
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
