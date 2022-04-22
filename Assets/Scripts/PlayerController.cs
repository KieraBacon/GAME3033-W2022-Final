using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCharacter), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static HashSet<PlayerController> allPlayerControllers = new HashSet<PlayerController>();

    [SerializeField] private PlayerCharacter _playerCharacter;
    public PlayerCharacter playerCharacter => _playerCharacter;
    [SerializeField] private Interactor interactor;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction slowAction;
    private InputAction pauseAction;
    public bool allowInput;

    private void Awake()
    {
        _playerCharacter = GetComponent<PlayerCharacter>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        interactAction = playerInput.actions.FindAction("Interact");
        slowAction = playerInput.actions.FindAction("Slow");
        pauseAction = playerInput.actions.FindAction("Pause");
    }

    private void OnEnable()
    {
        allPlayerControllers.Add(this);
        moveAction.performed += OnMove;
        interactAction.performed += OnInteract;
        slowAction.performed += OnSlow;
        slowAction.canceled += OnSlow;
        pauseAction.performed += OnPause;
    }

    private void OnDisable()
    {
        allPlayerControllers.Remove(this);
        moveAction.performed -= OnMove;
        interactAction.performed -= OnInteract;
        slowAction.performed -= OnSlow;
        slowAction.canceled -= OnSlow;
        pauseAction.performed -= OnPause;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        if (allowInput)
            _playerCharacter.Move(obj.ReadValue<Vector2>());
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (allowInput)
            interactor.Interact();
    }

    private void OnSlow(InputAction.CallbackContext obj)
    {
        if (allowInput)
            _playerCharacter.Slow(obj.performed);
    }

    private void OnPause(InputAction.CallbackContext obj)
    {
        GameManager.PauseGame();
    }

    public void StartGame()
    {
        allowInput = true;
        _playerCharacter.StartGame();
    }

    public void EndGame()
    {
        allowInput = false;
    }
}
