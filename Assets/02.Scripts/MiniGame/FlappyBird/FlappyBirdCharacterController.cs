using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlappyBirdCharacterController : MonoBehaviour, IRigidbodyController
{
    [SerializeField] private float jumpForce;

    private Rigidbody2D myRigidbody;

    public BindingData<bool> isGameOver = new(false);

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isGameOver.Value)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.y < -0.1f || viewPos.y > 1.1f)
        {
            isGameOver.Value = true;
        }
    }

    public void Move(Vector2 vector)
    {
        
    }

    public void Jump()
    {
        myRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isGameOver.Value = true;
    }
}
