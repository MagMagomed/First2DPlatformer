using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator animator;
    public void AnimationUpdate(PlayerModel playerModel)
    {
        animator.SetBool("Idle", playerModel.isGrounded);
        animator.SetFloat("xVelocity", Mathf.Abs(playerModel.rb.velocity.x));
        animator.SetFloat("yVelocity", playerModel.rb.velocity.y);
        animator.SetBool("isSit", playerModel.isSit);
    }
    public void JumpTrigger()
    {
        animator.SetTrigger("JumpTrigger");
    }
}
