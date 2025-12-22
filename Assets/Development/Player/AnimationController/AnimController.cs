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
    [SerializeField] private PlayerInteraction interactionComp;

    [Header("Parameters")]
    [SerializeField] private string peckTrigger = "PeckTrigger";
    private bool peck ;
    [SerializeField] private string landTrigger = "LandTrigger";
    private bool land ; 
    [SerializeField] private string flapTrigger = "FlapTrigger";
    private bool flapUp => flightMoveComp.GetFlapUp();
    [SerializeField] private string diveTrigger = "DiveTrigger";
    private bool dive => flightMoveComp.GetIsDiving();
    [SerializeField] private string poopTrigger = "PoopTrigger";
    private bool poop;
    [SerializeField] private float forwardSpeed=> groundMoveComp.GetSpeedForward();
    [SerializeField] private float sideSpeed => groundMoveComp.GetSpeedSide();
    [SerializeField] private float altitude;
    [SerializeField] private bool isGliding =>flightMoveComp.GetIsGliding();
    [SerializeField] private bool isFlying => groundMoveComp.GetIsFlying();
    [SerializeField] private bool isAiming=> poopComp.GetIsAiming();
    [SerializeField] private bool isSneaking => stealthComp.GetIsStealthToggled();
    [SerializeField] private bool isTalking => dialogueBaseComp.GetIsTyping();
    [SerializeField] private bool isGrounded => groundCheckComp.IsGrounded();
    [SerializeField] private bool isJumping => groundMoveComp.GetIsJumping();
    [SerializeField] private bool isSlowFlap;
    [SerializeField] private bool isLeftWingCheck;
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
        dialogueBaseComp = FindFirstObjectByType<DialogueBase>();
        stealthComp =  GetComponent<PlayerStealthSystem>();
        interactionComp = GetComponent<PlayerInteraction>();
        wingventoryComp = GetComponent<PlayerWingventory>();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleIsGrounded();
        playerAnimator.SetFloat("Speed", GetSpeed());
        playerAnimator.SetFloat("SideSpeed",GetSideSpeed());
        playerAnimator.SetFloat("Altitude",GetAltitude());
        ToggleAim();
        ToggleStealth();
        ToggleIsTalking();
        ToggleIsFlying();
        ToggleIsGliding();
        ToggleIsJumping();
        //TogglePoopTrigger();
        //TogglePeckTrigger();
        ToggleLeftWing();
        ToggleRightWing();
        //ToggleDiveTrigger();
        //ToggleFlapTrigger();
        ToggleSlowFlap();
        Debug.Log(forwardSpeed + sideSpeed);
    }

    private float GetSpeed()
    {
        return forwardSpeed;
    }


    private float GetSideSpeed()
    {
        return sideSpeed;
    }

    private float GetAltitude()
    {
        return altitude;
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

    private void GetPeckTrigger()
    {

    }

    private void TogglePeckTrigger()
    {
        playerAnimator.SetTrigger(peckTrigger);
    }

    private void GetFlapTrigger()
    {

    }

    private void ToggleFlapTrigger()
    {
        playerAnimator.SetTrigger(flapTrigger);
    }

    private void GetDiveTrigger()
    {

    }

    private void ToggleDiveTrigger()
    {
        playerAnimator.SetTrigger(diveTrigger);
    }

    private void GetPoopTrigger()
    {

    }
    private void TogglePoopTrigger()
    {
        playerAnimator.SetTrigger(poopTrigger);
    }

    private bool GetIsRightWing()
    {
        return isRightWingCheck;
    }

    private void ToggleRightWing()
    {

    }

    private bool GetIsLeftWing()
    {
        return isLeftWingCheck;
    }

    private void ToggleLeftWing()
    {

    }
}
