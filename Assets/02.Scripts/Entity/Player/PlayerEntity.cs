using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : MonoBehaviour, IEntity, UserActions.IPlayerActions
{
    private UserActions _input;
    private CharacterController2D _cc;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float moveSpeed = 5f;

    public event Action<InputAction.CallbackContext> OnMoveCallback = delegate { };

    void Awake()
    {
        _input = new UserActions();
        _input.Player.SetCallbacks(this);
        _input.Enable();

        OnMoveCallback = Movement;
    }

    void Start()
    {
        _cc = GetComponent<CharacterController2D>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Movement(InputAction.CallbackContext context)
    {
        var moveVec = context.ReadValue<Vector2>() * moveSpeed;

        _cc.Move(new Vector2(moveVec.x, 0f));

        _spriteRenderer.flipX = moveVec.x < 0f ? true : false;

        if (moveVec.y != 0f)
        {
            if (_cc.IsGround)
            {
                _cc.Jump(17f);
            }
        }
    }

    #region IPlayerActions 인터페이스

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveCallback?.Invoke(context);
    }

    #endregion
}
