using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class Carrier : MonoBehaviour
{
    [SerializeField]
    private Transform carryAttachmentPoint;
    private Carryable carriedObject;
    private Transform carriedObjectParent;
    private Vector3 carryOffset;
    public bool isCarrying => carriedObject != null;
    private Interactor interactor;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void OnEnable()
    {
        interactor.onBeforeInteraction += OnBeforeInteraction;
        interactor.onInteract += OnInteract;
    }

    private void OnDisable()
    {
        interactor.onBeforeInteraction -= OnBeforeInteraction;
        interactor.onInteract -= OnInteract;
    }

    private bool OnBeforeInteraction(Interactor interactor)
    {
        if (isCarrying)
        {
            Drop();
            return true;
        }

        return false;
    }

    private void OnInteract(Interactor interactor, IInteractable interactable)
    {
        Carryable carryable = interactable as Carryable;
        if (carryable)
        {
            PickUp(carryable);
        }
    }

    private void PickUp(Carryable carryable)
    {
        if (isCarrying)
            Drop();

        carriedObject = carryable;
        carriedObject.PickUp(this);
        carriedObjectParent = carriedObject.transform.parent;
        carriedObject.transform.SetParent(carryAttachmentPoint);
        carryOffset = carriedObject.transform.position - carryAttachmentPoint.position;
        carriedObject.transform.localPosition = Vector3.zero;
    }

    private void Drop()
    {
        carriedObject.transform.SetParent(carriedObjectParent);
        carriedObject.transform.position += new Vector3(0, carryOffset.y, 0);
        carriedObject.Drop();
        carriedObject = null;
    }
}
