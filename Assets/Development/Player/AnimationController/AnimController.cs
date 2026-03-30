using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject playerRef;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerGroundMovement groundMoveComp;
    [SerializeField] private PlayerFlightMovement flightMoveComp;
    [SerializeField] private GroundCheck groundCheckComp;
    [SerializeField] private Pooper poopComp;
    [SerializeField] private DialogueBase dialogueBaseComp;
    [SerializeField] private PlayerStealthSystem stealthComp;
    [SerializeField] private PlayerWingventory wingventoryComp;
    [SerializeField] private PlayerPeckComponent peckComp;
    [SerializeField] private PlayerInteraction interactionComp;

    [Header("Parameters")]
    private bool isPecking => peckComp.GetIsPecking();
    private bool flapUp => flightMoveComp.GetFlapUp();
    private bool isDiving => flightMoveComp.GetIsDiving();

    private bool isPooping;
    private float forwardSpeed=> groundMoveComp.GetSpeedForward();
    private float sideSpeed => groundMoveComp.GetSpeedSide();
    private bool isGliding =>flightMoveComp.GetIsGliding();
    private bool isFlying => groundMoveComp.GetIsFlying();
    private bool isAiming=> poopComp.GetIsAiming();
    private bool isSneaking => stealthComp.GetIsStealthToggled();
    private bool isTalking => dialogueBaseComp.GetIsTyping();
    private bool isGrounded => groundCheckComp.IsGrounded();
    private bool isJumping => groundMoveComp.GetIsJumping();
    private bool isSlowFlap => flightMoveComp.GetIsSlowFlap();
    private bool isLeftWingCheck => interactionComp.GetIsWingventoryOpen();
    [SerializeField] private bool isRightWingCheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = this.gameObject;
        playerAnimator = GetComponent<Animator>();
        groundMoveComp = GetComponent<PlayerGroundMovement>();
        flightMoveComp = GetComponent<PlayerFlightMovement>();
        groundCheckComp = GetComponentInChildren<GroundCheck>();
        poopComp = GetComponent<Pooper>();
        stealthComp =  GetComponent<PlayerStealthSystem>();
        interactionComp = GetComponent<PlayerInteraction>();
        wingventoryComp = GetComponent<PlayerWingventory>();
        peckComp = GetComponent<PlayerPeckComponent>();
        dialogueBaseComp = GetComponentInChildren<DialogueBase>();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleIsGrounded();
        playerAnimator.SetFloat("Speed", GetSpeed());
        playerAnimator.SetFloat("SideSpeed",GetSideSpeed());
        ToggleAim();
        ToggleStealth();
        ToggleIsTalking();
        ToggleIsFlying();
        ToggleIsGliding();
        ToggleIsJumping();
        TogglePoopTrigger();
        TogglePeckTrigger();
        ToggleLeftWing();
        ToggleRightWing();
        ToggleDiveTrigger();
        ToggleFlapTrigger();
        ToggleSlowFlap();
    }

    private float GetSpeed()
    {
        return forwardSpeed;
    }


    private float GetSideSpeed()
    {
        return sideSpeed;
    }


    private bool GetIsAiming()
    {
        return isAiming;
    }

    private void ToggleAim()
    {

        playerAnimator.SetBool("isAiming", GetIsAiming());
        
    }

    private bool GetIsFlying()
    {
        return isFlying;
    }

    private void ToggleIsFlying()
    {
        playerAnimator.SetBool("isFlying", GetIsFlying());
    }
    private bool GetIsSneaking()
    {
        return isSneaking;
    }

    private bool GetIsJumping()
    {
        return isJumping;
    }

    private void ToggleIsJumping()
    {
        playerAnimator.SetBool("isJumping", GetIsJumping());
    }

    private void ToggleStealth()
    {
        playerAnimator.SetBool("isSneaking", GetIsSneaking());
    }

    private bool GetIsTalking()
    {
        return isTalking;
    }

    private void ToggleIsTalking()
    {
        playerAnimator.SetBool("isTalking",GetIsTalking());
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
        playerAnimator.SetBool("isGliding",GetIsGliding()); 
    }

    private bool GetIsSlowFlap()
    {
        return isSlowFlap;

    }

    private void ToggleSlowFlap()
    {
        playerAnimator.SetBool("isSlowFlap",GetIsSlowFlap());
    }

    private bool GetPeckTrigger()
    {
        return isPecking;
    }


    private void TogglePeckTrigger()
    {

            playerAnimator.SetBool("isPecking",GetPeckTrigger());
       

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
        playerAnimator.SetBool("isDiving",GetDiveTrigger());
    }

    private bool GetPoopTrigger()
    {
        return isPooping;
    }
    private void TogglePoopTrigger()
    {
        playerAnimator.SetBool("isPooping", GetPoopTrigger());
    }

    private bool GetIsRightWing()
    {
        return isRightWingCheck;
    }

    private void ToggleRightWing()
    {
        playerAnimator.SetBool("isRightWingCheck",GetIsRightWing());
    }

    private bool GetIsLeftWing()
    {
        return isLeftWingCheck;
    }

    private void ToggleLeftWing()
    {
        playerAnimator.SetBool("isLeftWingCheck",GetIsLeftWing());
    }
}
