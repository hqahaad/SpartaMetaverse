using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlappyBirdCharacterController : MonoBehaviour, IRigidbodyController
{
    [SerializeField] private float jumpForce;

    private Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void Move(Vector2 vector)
    {
        
    }

    public void Jump()
    {
        myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
