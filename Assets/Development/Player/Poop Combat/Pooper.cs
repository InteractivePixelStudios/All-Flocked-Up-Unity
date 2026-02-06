using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pooper : MonoBehaviour
{
    [SerializeField] private PoopSystem poopSystem;
    [SerializeField] private PoopFunction poopFunction;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask poopableLayer;
    [SerializeField] private float maxRange = 20f; //Adjust as needed for gameplay

    //[SeralizeField] private PoopArcRenderer arcRenderer; option for visualizing the arc, not yet implemented

    [SerializeField] private Rigidbody pigeon;
    [SerializeField] private GameObject mesh;

    private bool isAiming = false; // Track if the player is currently aiming
    private PlayerInput playerInput;
    private InputAction aimAction;
    private InputAction poopAction;
    private GameObject player;

    bool isTurning;
    Quaternion startRot;
    Quaternion endRot;
    [SerializeField]float spinTime = 0f;
    [SerializeField]float spinDuration = 1f;
    PlayerGroundMovement groundComp;
    [SerializeField]bool isFlying;

    // #region Setup & Init

    //Switching to new input system - JK Oct/23

    public bool GetIsAiming()
    {
        return isAiming;
    }

    bool GetIsFlying()
    {
        isFlying = groundComp.GetIsFlying();
        return isFlying;
    }
    private void Start()
    {
        groundComp = GetComponent<PlayerGroundMovement>();
        playerInput = GetComponentInParent<PlayerInput>();
        Debug.Log($"PlayerInput: {playerInput != null}");

        //Set up input actions
        aimAction = playerInput.actions.FindAction("Aim");
        poopAction = playerInput.actions.FindAction("Fire");

        if (aimAction == null) Debug.LogError("Could not find 'Aim' action!");
        if (poopAction == null) Debug.LogError("Could not find 'Fire' action!");

        //Subscribe to input action events only if actions were found
        if (aimAction != null && poopAction != null)
        { 
            aimAction.started += OnAimStarted;
            aimAction.canceled += OnAimCanceled;
            poopAction.performed += OnPoopPerformed;
        }
    }

    private void Update()
    {
        if (!isTurning) return;
        
        spinTime += Time.deltaTime / spinDuration;

        float easeTime = Mathf.SmoothStep(0f, 1f, spinTime);
        mesh.transform.rotation = Quaternion.Slerp(startRot, endRot, easeTime);
        if (spinTime >= 1f)
        {
            mesh.transform.localRotation = endRot;
            isTurning = false;
        }

    }
    private void OnDestroy()
    {
        //Unsubscribe from input action events
        aimAction.started -= OnAimStarted;
        aimAction.canceled -= OnAimCanceled;
        poopAction.performed -= OnPoopPerformed;
    }

    //#endregion
    #region Input Callbacks
    private void OnAimStarted(InputAction.CallbackContext ctx)
    {
        isAiming = true;
        Debug.Log("Aiming started");
        startRot = mesh.transform.localRotation;
        endRot = startRot * Quaternion.Euler(0f, 180f, 0f);

        spinTime = 0f;
        isTurning = true;
        //Show aiming UI here if needed

    }

    private void OnAimCanceled(InputAction.CallbackContext ctx)
    {
        isAiming = false;
        Debug.Log("Aiming canceled");
        startRot = mesh.transform.localRotation;
        endRot = startRot * Quaternion.Euler(0f, -180f, 0f);
        spinTime = 0f;
        isTurning = true;
        //Hide aiming UI here if needed
    }

    private void OnPoopPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Poop action performed");

                TryPooping(GetIsFlying());
            
        
    }

    #endregion


    private void TryPooping(bool isFlying)
    {
        if (isFlying)
        {
            Debug.Log("FlyingPoopCalled");
            if (poopSystem.TryPoop())
            {
                Vector3 target = GetTarget();

                //Get player velocity from pigeon rigidbody
                Vector3 playerVelocity = pigeon.linearVelocity;
                poopFunction.FirePoop(target, playerVelocity);
            }
        }else
        if (poopSystem.TryPoop())
        {
            if (isAiming)
            {
                poopFunction.FireGroundPoop();
            }
        }
    }

    private Vector3 GetTarget()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, poopableLayer))
        {
            return hit.point;
        }

        return cam.transform.position + cam.transform.forward * maxRange;
    }


}
