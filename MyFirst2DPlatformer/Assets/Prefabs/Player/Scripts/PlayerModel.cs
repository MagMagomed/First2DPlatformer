using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckRadius = 0.2f;
    [SerializeField] public SpriteRenderer sprite;

    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider2D;
    public Vector2 defaultColliderSize;

    public bool isGrounded = false;
    public bool isSit = false;
    public bool canMoveToTheSide = true;
    public bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        defaultColliderSize = capsuleCollider2D.size;
    }

}
