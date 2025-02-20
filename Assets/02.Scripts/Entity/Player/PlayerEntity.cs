using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : MonoBehaviour, IEntity, UserActions.IPlayerActions, IVisitable, IBind<PositionData>
{
    [SerializeField]
    private float moveSpeed = 5f;

    public event Action<InputAction.CallbackContext> OnMoveCallback = delegate { };
    public event Action<InputAction.CallbackContext> OnInteractionCallback = delegate { };

    [SerializeField]
    private UserActions input;

    [SerializeField]
    private CharacterController2D cc;

    [SerializeField]
    private SpriteRenderer sprRenderer;
    private IRigidbodyController controllerCache;

    [SerializeField]
    private Animator animator;

    private IInputActionCollection collection;
    [field: SerializeField]
    public PositionData Data { get; private set; }

    void Awake()
    {
        input = new UserActions();
        input.Player.SetCallbacks(this);
        input.Enable();

        OnMoveCallback = Movement;
    }

    void Start()
    {
        cc = GetComponent<CharacterController2D>();
        controllerCache = cc;
        sprRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void OnEnable()
    {
        input.Enable();

        SceneLoadManager.Instance.OnSceneUnloaded += Save;
    }

    void OnDisable()
    {
        input.Disable();

        SceneLoadManager.Instance.OnSceneUnloaded -= Save;
    }

    void Save()
    {
        Data.x = transform.position.x;
        Data.y = transform.position.y;

        SaveLoader.Instance.data.positionData = Data;
        SaveLoader.Instance.Save();
    }

    private void Movement(InputAction.CallbackContext context)
    {
        //임시
        var moveVec = context.ReadValue<Vector2>() * moveSpeed;

        cc.Move(new Vector2(moveVec.x, 0f));

        if (moveVec.x != 0f)
        {
            sprRenderer.flipX = moveVec.x < 0f ? true : false;
            animator.SetInteger("StateID", 1);
        }
        else
        {
            animator.SetInteger("StateID", 0);
        }

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

    public void Bind(PositionData t)
    {
        Data = t;

        transform.position = new Vector3(Data.x, Data.y, transform.position.z);
    }
}


[System.Serializable]
public class PositionData : ISavable
{
    public float x;
    public float y;
}
