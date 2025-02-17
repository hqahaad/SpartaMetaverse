using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : MonoBehaviour, IEntity, UserActions.IPlayerActions
{
    private UserActions input;
    private Rigidbody2D rigid;
    private CharacterController2D cc;

    [SerializeField]
    private float moveSpeed = 5f;

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
        cc = GetComponent<CharacterController2D>();
    }

    private void Movement(InputAction.CallbackContext context)
    {
        var moveVec = context.ReadValue<Vector2>() * moveSpeed;

        cc.Move(moveVec);
    }

    #region IPlayerActions 인터페이스

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveCallback?.Invoke(context);
    }

    #endregion
}
