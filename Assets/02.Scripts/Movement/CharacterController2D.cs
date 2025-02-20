using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D : MonoBehaviour, IRigidbodyController
{
    //ScriptableObject로 바꾸자

    [Header("Floating Capsule")]
    [SerializeField] private FloatingCapsule floatingCapsule;
    [SerializeField] private float floatingStrength;
    [SerializeField] float damping = 2f;

    [Header("Layer")]
    [SerializeField] private LayerMask targetLayer;

    [Header("Gravity")]
    [SerializeField] private bool useGravity = true;
    [SerializeField] private Vector2 gravityDirection = Vector2.down;
    [SerializeField] private float gravityForce = 0.17f;
    [SerializeField] private float maxGravityForce = 11f;

    [Header("Ground Detected")]
    [SerializeField] private float detectedDistance = 1f;
    [SerializeField] private float groundTolerance = 0.01f;
    [SerializeField] private float skinWidth = 0.05f;

    [Header("Slope")]
    [Range(0f, 89f)]
    [SerializeField] private float slopeLimit = 60f;

    [Header("State")]
    [SerializeField] private bool isGround;
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isSteepSlope;

    private Rigidbody2D myRigidbody;
    private CapsuleCollider2D myCollider;
    private RaycastHit2D[] raycastHits = new RaycastHit2D[maxBudget];
    private float gravity = 0f;
    private Vector2 groundNormal = Vector2.up;
    private float distanceToGround = 0f;

    private Vector2 gravityVelocity = Vector2.zero;
    private Vector2 floatingVelocity = Vector2.zero;
    private Vector2 inputVelocity = Vector2.zero;
    private Vector2 targetVelocity = Vector2.zero;

    private const int maxBudget = 10;

    //properties
    public bool IsGround => isGround;
    public bool IsSlope => isSlope;
    public bool IsSteepSlope => isSteepSlope;
    public Vector2 GroundNormal => groundNormal;

    private Vector2 CapsuleBottomPoint => myCollider.bounds.center + new Vector3(0f, -myCollider.bounds.size.y * 0.5f - floatingCapsule.Height);

    public void Move(Vector2 input)
    {
        InternalMove(input);
    }

    public void Jump(float strength)
    {
        gravity = -strength;
    }

    private void DetectedGround()
    {
        isGround = isSlope = isSteepSlope = false;

        int hitCount = Physics2D.CapsuleCastNonAlloc(myCollider.bounds.center, myCollider.bounds.size - new Vector3(skinWidth, 0f, 0f), myCollider.direction, 0f, Vector2.down, 
            raycastHits, detectedDistance + floatingCapsule.Height, targetLayer);

        if (hitCount > 0)
        {
            RaycastHit2D hit = default;

            for (int i = 0; i < hitCount; i++)
            {
                var dist = CapsuleBottomPoint.y - hit.point.y;

                if (dist > CapsuleBottomPoint.y - raycastHits[i].point.y || hit == default)
                {
                    hit = raycastHits[i];
                }

                Debug.DrawLine(transform.position, raycastHits[i].point);
            }

            Debug.DrawLine(transform.position, hit.point, Color.red);

            var ray = Physics2D.Raycast(hit.point + new Vector2(0f, 0.01f), Vector2.down, 0.02f, targetLayer);

            if (ray)
            {
                groundNormal = ray.normal;
            }
            else
            {
                groundNormal = Vector2.up;
            }

            Debug.DrawLine(hit.point, hit.point + groundNormal * 5f, Color.magenta);

            distanceToGround = CapsuleBottomPoint.y - hit.point.y;

            if (distanceToGround <= 0f + groundTolerance)
            {
                isGround = true;

                if (Vector2.Angle(groundNormal, Vector2.up) > slopeLimit)
                {
                    isSlope = true;
                    isSteepSlope = true;
                }
                else if (groundNormal != Vector2.up)
                {
                    isSlope = true;
                }
            }
        }
    }

    private void Floating()
    {
        if (distanceToGround < 0)
        {
            float correctionSpd = -distanceToGround * floatingStrength;
            floatingVelocity.y = Mathf.Lerp(floatingVelocity.y, correctionSpd, Time.deltaTime * damping);
        }
        else
        {
            floatingVelocity = Vector2.zero;
        }
    }

    private void ApplyGravity()
    {
        if (!useGravity)
            return;

        if (isGround && !isSteepSlope && gravity != 0f)
        {
            gravity = 0f;
        }
        else if (!isGround)
        {
            gravity += gravityForce;
        }

        gravityVelocity = gravityDirection.normalized * gravity;

        if (isSteepSlope)
        {
            gravityVelocity = CalculateNormalVector(gravityVelocity, groundNormal);
        }
    }

    private void InternalMove(Vector2 input)
    {
        inputVelocity = input;
        targetVelocity = inputVelocity;
    }

    private void UpdateVelocity()
    {
        if (isSteepSlope)
        {
            targetVelocity = Vector2.zero;
        }

        if (isGround)
        {
            targetVelocity = CalculateNormalVector(targetVelocity, groundNormal);
        }
    }

    private void ApplyVelocity()
    {
        myRigidbody.velocity = gravityVelocity + floatingVelocity + targetVelocity;
    }

    private Vector2 CalculateNormalVector(Vector2 vel, Vector2 normal)
    {
        var mag = vel.magnitude;
        var vec = vel - Vector2.Dot(vel, normal) * normal.normalized;

        return vec.normalized * mag;
    }

    #region LifeCycle
    void Awake()
    {
        floatingCapsule.Initialize(this.gameObject);
        floatingCapsule.UpdateCapsuleDimensions();
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CapsuleCollider2D>();
    }

    void OnValidate()
    {
        floatingCapsule.Initialize(this.gameObject);
        floatingCapsule.UpdateCapsuleDimensions();
    }

    void FixedUpdate()
    {
        DetectedGround();
        ApplyGravity();
        Floating();
        UpdateVelocity();
        ApplyVelocity();

        inputVelocity = Vector2.zero;
    }
    #endregion
}

