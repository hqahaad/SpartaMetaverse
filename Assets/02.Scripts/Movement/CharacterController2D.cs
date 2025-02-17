using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Floating Capsule")]
    [SerializeField] private FloatingCapsule floatingCapsule;
    [SerializeField] private float floatingStrength;

    [Header("Layer")]
    [SerializeField] private LayerMask targetLayer;

    [Header("Gravity")]
    [SerializeField] private bool useGravity = true;
    [SerializeField] private Vector2 gravityDirection = Vector2.down;
    [SerializeField] private float gravityForce = 0.17f;
    [SerializeField] private float maxGravityForce = 11f;

    [Header("Ground Detected")]
    [SerializeField] private float detectedDistance = 1f;
    [SerializeField] private float safetyGroundTolerance = 0.01f;

    [Header("Slope")]
    [Range(0f, 89f)]
    [SerializeField] private float slopeLimit = 60f;

    [Header("State")]
    [SerializeField] private bool isGround;
    [SerializeField] private bool isSlope;
    [SerializeField] private bool isSteepSlope;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private RaycastHit2D[] _raycastHits = new RaycastHit2D[_maxBudget];
    private float _gravity = 0f;
    private Vector2 _groundNormal = Vector2.up;
    private float _distanceToGround = 0f;

    private Vector2 _gravityVelocity = Vector2.zero;
    private Vector2 _floatingVelocity = Vector2.zero;
    private Vector2 _inputVelocity = Vector2.zero;
    private Vector2 _targetVelocity = Vector2.zero;

    private const int _maxBudget = 10;

    public void Move(Vector2 input)
    {
        InternalMove(input);
    }


    private void DetectedGround()
    {
        isGround = isSlope = isSteepSlope = false;

        int hitCount = Physics2D.CapsuleCastNonAlloc(_collider.bounds.center, _collider.bounds.size, _collider.direction, 0f, Vector2.down, 
            _raycastHits, detectedDistance + floatingCapsule.Height, targetLayer);

        if (hitCount > 0)
        {
            RaycastHit2D hit = _raycastHits[0];

            for (int i = 0; i < hitCount; i++)
            {
                if (hit.distance > _raycastHits[i].distance)
                {
                    hit = _raycastHits[i];
                }
            }

            _groundNormal = hit.normal;
            _distanceToGround = hit.distance;

            if (_distanceToGround <= floatingCapsule.Height)
            {
                isGround = true;

                if (Vector2.Angle(_groundNormal, Vector2.up) > slopeLimit)
                {
                    isSlope = true;
                    isSteepSlope = true;
                }
                else if (_groundNormal != Vector2.up)
                {
                    isSlope = true;
                }
            }
        }
    }

    private void Floating()
    {
        if (_distanceToGround + safetyGroundTolerance <= floatingCapsule.Height && isGround)
        {
            _floatingVelocity = Vector2.up * (floatingCapsule.Height - _distanceToGround) * floatingStrength;
        }
        else
        {
            _floatingVelocity = Vector2.zero;
        }
    }

    private void ApplyGravity()
    {
        if (!useGravity)
            return;

        if (isGround && !isSteepSlope)
        {
            _gravity = 0f;
        }
        else
        {
            _gravity += gravityForce;
        }

        _gravity = Mathf.Clamp(_gravity, 0f, maxGravityForce);
        _gravityVelocity = gravityDirection.normalized * _gravity;

        if (isSteepSlope)
        {
            _gravityVelocity = CalculateNormalVector(_gravityVelocity, _groundNormal);
        }
    }

    private void InternalMove(Vector2 input)
    {
        _inputVelocity = input;
        _targetVelocity = _inputVelocity;
    }

    private void UpdateVelocity()
    {
        if (isSteepSlope)
        {
            _targetVelocity = Vector2.zero;
        }

        if (isGround)
        {
            _targetVelocity = CalculateNormalVector(_targetVelocity, _groundNormal);
        }
    }

    private void ApplyVelocity()
    {
        _rigidbody.velocity = _gravityVelocity + _floatingVelocity + _targetVelocity;
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
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
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
        UpdateVelocity();
        Floating();

        ApplyVelocity();

        _inputVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
            
    }
    #endregion
}

