using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMoveble
{
    public PlayerModel playerModel;
    public PlayerView playerView;
    private Vector2 contactNormal = Vector3.zero;
    private Vector2 groundNormal = Vector3.zero;
    private void Update()
    {
        playerView.AnimationUpdate(playerModel);
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
        playerModel.canMoveToTheSide = true;

        var grounds = collision.contacts.Where(c =>
                                                    (int)Mathf.Pow(2, c.collider.gameObject.layer) == playerModel.groundLayer.value &&
                                                    Mathf.Abs(Mathf.Atan2(c.normal.x, c.normal.y) * Mathf.Rad2Deg) > 45f
                                              );

        var isRestedAgainstAWall = grounds.Count() > 0;
        var wallIsLeft = grounds.Any(c => c.collider.gameObject.transform.position.x < gameObject.transform.position.x);
        var wallIsRight = grounds.Any(c => c.collider.gameObject.transform.position.x > gameObject.transform.position.x);
        if (isRestedAgainstAWall)
        {
            if(wallIsLeft && Input.GetAxis("Horizontal") < 0)
            {
                playerModel.canMoveToTheSide = false;
            }
            else if(wallIsRight && Input.GetAxis("Horizontal") > 0)
            {
                playerModel.canMoveToTheSide = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerModel.isGrounded = false;
        playerModel.canMoveToTheSide = true;
    }
    #region Вспомогательные методы
    private void ColliderUpdate(float vericalAxis)
    {
        if(vericalAxis < 0)
        {
            Vector2 otherObjectSize = playerModel.sprite.bounds.size;
            playerModel.capsuleCollider2D.size = otherObjectSize;
        }
        else
        {
            playerModel.capsuleCollider2D.size = playerModel.defaultColliderSize;
        }
    }
    private void IsSitUpdate(float vericalAxis)
    {
        playerModel.isSit = vericalAxis < 0 && !playerModel.isMoving;
    }
    private void IsGroundedUpdate(Collision2D collision)
    {
        var grounds = collision.contacts.Where(c => 
                                                    (int)Mathf.Pow(2, c.collider.gameObject.layer) == playerModel.groundLayer.value &&
                                                    Mathf.Abs(Mathf.Atan2(c.normal.x, c.normal.y) * Mathf.Rad2Deg) <= 45f
                                              );
        if (grounds.Count() != 0)
        {
            groundNormal = grounds.FirstOrDefault().normal;
            playerModel.isGrounded = true;
        }
    }
    public void Jump()
    {
        if (playerModel.isGrounded)
        {
            playerModel.rb.velocity = new Vector2(playerModel.rb.velocity.x, playerModel.jumpForce);
            playerView.JumpTrigger();
        }
    }
    private void Move(float moveInput) 
    {
        if(Mathf.Abs(moveInput) > 0 && playerModel.canMoveToTheSide)
        {
            if (playerModel.isGrounded)
            {
                playerModel.rb.velocity = groundNormal.Perpendicular1() * playerModel.movementSpeed * moveInput;
            }
            else
            {
                playerModel.rb.velocity = new Vector2(moveInput * playerModel.movementSpeed, playerModel.rb.velocity.y);
            }
            playerModel.isMoving = true;
        }
        else
        {
            playerModel.isMoving = false;
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
        playerModel.transform.rotation = Quaternion.Euler(playerModel.transform.rotation.x, 180f, playerModel.transform.rotation.z);
    }
    private void FlipRight()
    {
        playerModel.transform.rotation = Quaternion.Euler(playerModel.transform.rotation.x, 0f, playerModel.transform.rotation.z);
    }

    public void MoveHorizontal(float axisValue)
    {
        Move(axisValue);
        Flip(axisValue);
    }

    public void MoveVertical(float axisValue)
    {
        ColliderUpdate(axisValue);
        IsSitUpdate(axisValue);
    }
    #endregion
}
