using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMoveble, IAnimatable
{
    public Player player;
    private Vector2 contactNormal = Vector3.zero;
    private Vector2 groundNormal = Vector3.zero;
    private void Update()
    {
        AnimationUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsGroundedUpdate(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        IsGroundedUpdate(collision);
        IsCanMoveUpdate(collision);
    }
    private void IsCanMoveUpdate(Collision2D collision)
    {
        player.canMoveToTheSide = true;

        var grounds = collision.contacts.Where(c =>
                                                    (int)Mathf.Pow(2, c.collider.gameObject.layer) == player.groundLayer.value &&
                                                    Mathf.Abs(Mathf.Atan2(c.normal.x, c.normal.y) * Mathf.Rad2Deg) > 45f
                                              );

        var isRestedAgainstAWall = grounds.Count() > 0;
        var wallIsLeft = grounds.Any(c => c.collider.gameObject.transform.position.x < gameObject.transform.position.x);
        var wallIsRight = grounds.Any(c => c.collider.gameObject.transform.position.x > gameObject.transform.position.x);
        if (isRestedAgainstAWall)
        {
            if(wallIsLeft && Input.GetAxis("Horizontal") < 0)
            {
                player.canMoveToTheSide = false;
            }
            else if(wallIsRight && Input.GetAxis("Horizontal") > 0)
            {
                player.canMoveToTheSide = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        player.isGrounded = false;
        player.canMoveToTheSide = true;
    }
    #region Вспомогательные методы
    private void ColliderUpdate()
    {
        if(Input.GetAxis("Vertical") < 0)
        {
            Vector2 otherObjectSize = player.sprite.bounds.size;
            player.capsuleCollider2D.size = otherObjectSize;
        }
        else
        {
            player.capsuleCollider2D.size = player.defaultColliderSize;
        }
    }
    private bool IsJump()
    {
        return Input.GetButton("Jump");
    }
    private void IsSitUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        player.isSit = verticalInput < 0 && player.isGrounded && Input.GetAxisRaw("Horizontal") == 0f;
    }
    private void IsGroundedUpdate(Collision2D collision)
    {
        var grounds = collision.contacts.Where(c => 
                                                    (int)Mathf.Pow(2, c.collider.gameObject.layer) == player.groundLayer.value &&
                                                    Mathf.Abs(Mathf.Atan2(c.normal.x, c.normal.y) * Mathf.Rad2Deg) <= 45f
                                              );
        if (grounds.Count() != 0)
        {
            groundNormal = grounds.FirstOrDefault().normal;
            player.isGrounded = true;
        }
    }
    public void AnimationUpdate()
    {
        player.animator.SetBool("Idle", player.isGrounded);
        player.animator.SetFloat("xVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        player.animator.SetFloat("yVelocity", player.rb.velocity.y);
        player.animator.SetBool("isSit", player.isSit);
    }
    public void Jump()
    {
        if (player.isGrounded)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
            player.animator.SetTrigger("JumpTrigger");
        }
    }
    private void Move(float moveInput) 
    {
        if(Mathf.Abs(moveInput) > 0 && player.canMoveToTheSide)
        {
            if (player.isGrounded)
            {
                player.rb.velocity = groundNormal.Perpendicular1() * player.movementSpeed * moveInput;
            }
            else
            {
                player.rb.velocity = new Vector2(moveInput * player.movementSpeed, player.rb.velocity.y);
            }
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

    public void MoveHorizontal(float axisValue)
    {
        Move(axisValue);
        Flip(axisValue);
    }

    public void MoveVertical(float axisValue)
    {
        ColliderUpdate();
        IsSitUpdate();
    }
    #endregion
}
