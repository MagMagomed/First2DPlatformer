using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private Rigidbody2D rb;
    public Animator animator;
    private bool isFacingRight = true;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);

        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
        }

        
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, isGrounded ? Color.green : Color.red);
        animator.SetBool("Idle", isGrounded);
        animator.SetFloat("xVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
