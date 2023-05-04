using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;
    private Vector2 defaultColliderSize;
    public Animator animator;
    private bool isGrounded = false;
    private bool isSit = false;

    private Vector2 contactNormal = Vector3.zero;
    private Vector2 groundNormal = Vector3.zero;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        defaultColliderSize = capsuleCollider2D.size;
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Move(moveInput);
        Flip(moveInput);

        ColliderUpdate();
        IsSitUpdate();
        AnimationUpdate();
        Jump();
    }

    private void ColliderUpdate()
    {
        if(Input.GetAxis("Vertical") < 0)
        {
            Vector2 otherObjectSize = sprite.bounds.size;
            capsuleCollider2D.size = otherObjectSize;
        }
        else
        {
            capsuleCollider2D.size = defaultColliderSize;
        }
    }

    private void IsSitUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        isSit = verticalInput < 0 && isGrounded && Input.GetAxisRaw("Horizontal") == 0f;
    }
    private void IsGroundedUpdate(Collision2D collision)
    {
        //var grounds = collision.contacts.Where(c => (int)Mathf.Pow(2, c.collider.gameObject.layer)  == groundLayer.value);
        //foreach (var ground in grounds)
        //{
        //    contactNormal = ground.normal;
        //    float surfaceAngle = Mathf.Atan2(contactNormal.x, contactNormal.y) * Mathf.Rad2Deg;
        //    if (Mathf.Abs(surfaceAngle) <= 45f)
        //    {
        //        groundNormal = ground.normal;
        //        isGrounded = true;
        //        break;
        //    }
        //}

        var ground = collision.contacts.Where(c => 
                                                    (int)Mathf.Pow(2, c.collider.gameObject.layer) == groundLayer.value &&
                                                    Mathf.Abs(Mathf.Atan2(c.normal.x, c.normal.y) * Mathf.Rad2Deg) <= 45f
                                              );
        if (ground.Count() != 0)
        {
            groundNormal = ground.FirstOrDefault().normal;
            isGrounded = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        IsGroundedUpdate(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        IsGroundedUpdate(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
    private void AnimationUpdate()
    {
        animator.SetBool("Idle", isGrounded);
        animator.SetFloat("xVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isSit", isSit);
    }
    private void Jump()
    {
        if (Input.GetAxis("Vertical") > 0.1f && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("JumpTrigger");
        }
    }
    private void Move(float moveInput) 
    {
        if(Mathf.Abs(moveInput) > 0)
        {
            rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);
        }
    }
    private void Flip(float moveInput)
    {
        if (moveInput > 0)
        {
            FlipRight();
        }
        if (moveInput < 0)
        {
            FlipLeft();
        }
    }
    private void FlipLeft()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
    }
    private void FlipRight()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
    }
}
