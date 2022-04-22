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
        if (carriedObject != null)
        {
            Drop();
            return;
        }

        Carryable carryable = interactable as Carryable;
        if (carryable)
        {
            PickUp(carryable);
        }
    }

    private void PickUp(Carryable carryable)
    {
        if (carryable == null)
            return;

        if (carriedObject != null)
            Drop();

        carriedObject = carryable;
        carriedObject.PickUp(this);
        carriedObject.rigidbody.isKinematic = !carriedObject.canInteract;
        carriedObject.rigidbody.detectCollisions = false;
        carriedObjectParent = carriedObject.transform.parent;
        carriedObject.transform.SetParent(carryAttachmentPoint);
        carryOffset = carriedObject.transform.position - carryAttachmentPoint.position;
        carriedObject.transform.localPosition = Vector3.zero;

        interactor.SetPersistentInteraction(carriedObject);
    }

    private void Drop()
    {
        if (carriedObject == null)
            return;

        carriedObject.Drop();
        carriedObject.rigidbody.isKinematic = !carriedObject.canInteract;
        carriedObject.rigidbody.detectCollisions = true;
        carriedObject.transform.SetParent(carriedObjectParent);
        carriedObject.rigidbody.MovePosition(carriedObject.transform.position + new Vector3(0, carryOffset.y, 0));
        //carriedObject.transform.position += new Vector3(0, carryOffset.y, 0);
        carriedObject = null;

        interactor.SetPersistentInteraction(null);
    }
}
