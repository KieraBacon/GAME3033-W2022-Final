using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField, Range(0, 1)]
    private float slowPercent = 0.5f;
    [SerializeField, Min(0)]
    private float speed = 1;

    [Header("Interaction")]
    private Vector2 moveInput = Vector2.zero;

    [Header("Component References")]
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(moveInput.x * speed, rigidbody.velocity.y, moveInput.y * speed);
    }

    public void Move(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    public void Slow(bool value)
    {
        if (value)
        {
            TimeManager.AdjustTime(gameObject, 1 - slowPercent);
        }
        else
        {
            TimeManager.RestoreTime(gameObject);
        }
    }
}
