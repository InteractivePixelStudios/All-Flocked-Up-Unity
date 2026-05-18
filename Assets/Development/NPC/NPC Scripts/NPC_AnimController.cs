
using System.Collections.Generic;
using UnityEngine;

public class NPC_AnimController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CPURacer groundMoveComp;
    [SerializeField] private GroundCheck groundCheckComp;


    [Header("Parameters")]
    private bool flapUp => groundMoveComp.GetFlapUp();
    private bool isDiving => groundMoveComp.GetIsDiving();
    private float forwardSpeed => groundMoveComp.GetSpeedForward();
    private bool isGliding => groundMoveComp.GetIsGliding();
    private bool isFlying => groundMoveComp.GetIsFlying();
    private bool isGrounded => groundMoveComp.GroundCheck();
    private bool isJumping => groundMoveComp.GetIsJumping();
    private bool isSlowFlap => groundMoveComp.GetIsSlowFlap();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        groundMoveComp = GetComponent<CPURacer>();

    }

    // Update is called once per frame
    void Update()
    {
        ToggleIsGrounded();
        playerAnimator.SetFloat("Speed", GetSpeed());
        ToggleIsFlying();
        ToggleIsGliding();
        ToggleIsJumping();
        ToggleDiveTrigger();
        ToggleFlapTrigger();
        ToggleSlowFlap();
    }

    private float GetSpeed()
    {
        return forwardSpeed;
    }



    private bool GetIsFlying()
    {
        return isFlying;
    }

    private void ToggleIsFlying()
    {
        playerAnimator.SetBool("isFlying", GetIsFlying());
    }

    private bool GetIsJumping()
    {
        return isJumping;
    }

    private void ToggleIsJumping()
    {
        playerAnimator.SetBool("isJumping", GetIsJumping());
    }



    private bool GetIsGrounded()
    {
        return isGrounded;

    }

    private void ToggleIsGrounded()
    {
        playerAnimator.SetBool("isGrounded", GetIsGrounded());
    }

    private bool GetIsGliding()
    {
        return isGliding;
    }

    private void ToggleIsGliding()
    {
        playerAnimator.SetBool("isGliding", GetIsGliding());
    }

    private bool GetIsSlowFlap()
    {
        return isSlowFlap;

    }

    private void ToggleSlowFlap()
    {
        playerAnimator.SetBool("isSlowFlap", GetIsSlowFlap());
    }



    private bool GetFlapTrigger()
    {
        return flapUp;
    }

    private void ToggleFlapTrigger()
    {
        playerAnimator.SetBool("isFlap", GetFlapTrigger());
    }

    private bool GetDiveTrigger()
    {
        return isDiving;
    }

    private void ToggleDiveTrigger()
    {
        playerAnimator.SetBool("isDiving", GetDiveTrigger());
    }


}


