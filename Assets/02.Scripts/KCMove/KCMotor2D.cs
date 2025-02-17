using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev.KC;
using System.Linq;

namespace Dev.KC
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class KCMotor2D : MonoBehaviour
    {
        [Header("Test")]
        public Vector2 groundNormal;
        [SerializeField]
        private VelocityMode velocityMode = VelocityMode.Simple;

        [Header("Layer")]
        [SerializeField]
        private LayerMask targetLayers;

        [Header("Gravity")]
        [SerializeField]
        private bool useGravity;
        [SerializeField]
        private float gravityForce;
        [SerializeField]
        private Vector2 gravityDirection = Vector2.down;

        [Header("Detected")]
        [SerializeField]
        private float skinWidth = 0.01f;
        [SerializeField]
        private float detectedGroundDistance;

        private Rigidbody2D _rigidbody;
        [SerializeField]
        private CapsuleCollider2D _collider;
        private const int _maxBudget = 50;
        private RaycastHit2D[] _raycastHit = new RaycastHit2D[_maxBudget];

        [Header("Floating Capsule")]
        [SerializeField]
        private float dampingForce;

        [Header("Slope")]
        [SerializeField][Range(0f, 89f)] private float slopeLimit = 45f;

        [Header("Debug")]
        [SerializeField]
        private bool isGround;
        [SerializeField]
        private bool isSlope;
        [SerializeField]
        private bool isSteepSlope;
        [SerializeField]
        private float distanceToGround;

        [SerializeField]
        private FloatingCapsule floatingCapsule;

        private Vector2 inputDirection = Vector2.zero;
        private Vector2 entityVelocity = Vector2.zero;

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
            Floating();
            UpdateVelocity();
        }

        public void Move(Vector2 direction)
        {
            InternalMove(direction);
        }

        private void InternalMove(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            inputDirection = direction;
            entityVelocity = CalculateSlope(inputDirection, groundNormal);
        }

        public void UpdateVelocity()
        {
            if (inputDirection == Vector2.zero && entityVelocity == Vector2.zero)
                return;

            _rigidbody.velocity = entityVelocity;
        }

        public void Floating()
        {
            if (isGround)
            {
                if (distanceToGround < floatingCapsule.Height)
                {
                    var damping = floatingCapsule.Height - distanceToGround;
                    _rigidbody.MovePosition(_rigidbody.position + Vector2.up * damping * dampingForce * Time.deltaTime);
                }
            }
        }

        public void DetectedGround()
        {
            isGround = false;
            isSlope = false;
            isSteepSlope = false;

            var hitCount = Physics2D.CapsuleCastNonAlloc(_collider.bounds.center, _collider.bounds.size, _collider.direction, 0f, Vector2.down, 
                _raycastHit, detectedGroundDistance + floatingCapsule.Height, targetLayers);

            if (hitCount > 0)
            {
                RaycastHit2D hit = _raycastHit[0];
                for (int i = 0; i < hitCount; i++)
                {
                    if (hit.distance > _raycastHit[i].distance)
                    {
                        hit = _raycastHit[i];
                    }
                }

                groundNormal = hit.normal;
                distanceToGround = hit.distance;

                if (distanceToGround <= floatingCapsule.Height)
                {
                    isGround = true;

                    if (Vector2.Angle(groundNormal, Vector3.up) > slopeLimit)
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

        public Vector2 CalculateSlope(Vector2 velocity, Vector2 normal)
        {
            var mag = velocity.magnitude;
            var project = Vector3.ProjectOnPlane(velocity, normal).normalized * mag;

            return new Vector2(project.x, project.y);
        }

        public Vector2 UpdateSmoothVelocity(Vector2 velocity, Vector2 targetVelocity, float delta, float accel, float force = 1f)
        {
            if (velocity == targetVelocity)
                return velocity;

            return Vector2.MoveTowards(velocity, targetVelocity, accel * delta * force);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_collider.bounds.center, _collider.bounds.center + Vector3.down * detectedGroundDistance);
            Gizmos.color = Color.green;

            var hitCount = Physics2D.CapsuleCastNonAlloc(_collider.bounds.center, _collider.bounds.size, _collider.direction, 0f, Vector2.down,
                _raycastHit, detectedGroundDistance + floatingCapsule.Height, targetLayers);

            if (hitCount > 0)
            {
                RaycastHit2D hit = _raycastHit[0];

                for (int i = 0; i < hitCount; i++)
                {
                    if (hit.distance > _raycastHit[i].distance)
                    {
                        hit = _raycastHit[i];
                    }
                }

                groundNormal = hit.normal;

                //Gizmos.DrawSphere(hit.point, 0.1f);
                Gizmos.DrawLine(hit.point, hit.point + groundNormal * 5f);
                Gizmos.DrawWireSphere(_collider.bounds.center + Vector3.down * _collider.bounds.size.y * 0.5f + Vector3.down * floatingCapsule.Height, 0.05f);
                Gizmos.color = Color.magenta;
            }
        }

        
    }

    public enum VelocityMode
    {
        Raw,
        Simple
    }
}
