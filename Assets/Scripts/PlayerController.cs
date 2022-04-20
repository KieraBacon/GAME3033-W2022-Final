using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCharacter), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction slowAction;

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        interactAction = playerInput.actions.FindAction("Interact");
        slowAction = playerInput.actions.FindAction("Slow");
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        interactAction.performed += OnInteract;
        slowAction.performed += OnSlow;
        slowAction.canceled += OnSlow;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        interactAction.performed -= OnInteract;
        slowAction.performed -= OnSlow;
        slowAction.canceled -= OnSlow;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        playerCharacter.Move(obj.ReadValue<Vector2>());
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        playerCharacter.Interact();
    }

    private void OnSlow(InputAction.CallbackContext obj)
    {
        playerCharacter.Slow(obj.performed);
    }
}
