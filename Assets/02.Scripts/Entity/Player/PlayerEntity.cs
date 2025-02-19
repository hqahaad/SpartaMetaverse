using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : MonoBehaviour, IEntity, UserActions.IPlayerActions, IVisitable
{
    [SerializeField]
    private float moveSpeed = 5f;

    public event Action<InputAction.CallbackContext> OnMoveCallback = delegate { };
    public event Action<InputAction.CallbackContext> OnInteractionCallback = delegate { };

    private UserActions input;
    private CharacterController2D cc;
    private SpriteRenderer sprRenderer;
    private IRigidbodyController controllerCache;

    private IInputActionCollection collection;

    void Awake()
    {
        input = new UserActions();
        input.Player.SetCallbacks(this);
        input.Enable();

        OnMoveCallback = Movement;

        //InputManager.Instance.RegisterInputAction(InputContext.Player, input);

        //var a = InputManager.Instance.GetInputAction(InputContext.Player);
    }

    void Start()
    {
        cc = GetComponent<CharacterController2D>();
        controllerCache = cc;
        sprRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Movement(InputAction.CallbackContext context)
    {
        var moveVec = context.ReadValue<Vector2>() * moveSpeed;

        cc.Move(new Vector2(moveVec.x, 0f));

        sprRenderer.flipX = moveVec.x < 0f ? true : false;

        if (moveVec.y != 0f)
        {
            if (cc.IsGround)
            {
                cc.Jump(17f);
            }
        }
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Cancel(IVisitor visitor)
    {
        visitor.Leave(this);
    }

    #region IPlayerActions 인터페이스

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveCallback?.Invoke(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnInteractionCallback?.Invoke(context);
        }
    }


    #endregion
}
