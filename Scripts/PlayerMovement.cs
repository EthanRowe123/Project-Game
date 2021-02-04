using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    float horizontalMove = 0f;
    bool jump = false;
    public float runSpeed = 40f;
    // Start is called before the first frame update

    
    
    // Update is called once per frame
    [Client]
    void Update()
    {
        //gets input
        if (this.isLocalPlayer)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("IsJumping", true);
            }
        }
 
    }
    
    [Client]
    void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
            jump = false;
        }

    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
}
