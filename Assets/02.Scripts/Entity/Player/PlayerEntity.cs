using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class PlayerEntity : MonoBehaviour, IEntity, UserActions.IPlayerActions
{
    private UserActions input;
    private Rigidbody2D rigid;
    private CircleCollider2D collide;

    [SerializeField]
    private float moveSpeed = 1f;

    public event Action<InputAction.CallbackContext> OnMoveCallback = delegate { };

    void Awake()
    {
        input = new UserActions();
        input.Player.SetCallbacks(this);
        input.Enable();

        OnMoveCallback = Movement;
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        collide = GetComponent<CircleCollider2D>();
    }

    private void Movement(InputAction.CallbackContext context)
    {
        rigid.velocity = context.ReadValue<Vector2>() * moveSpeed;
    }

    #region IPlayerActions 인터페이스

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveCallback?.Invoke(context);
    }

    #endregion
}
