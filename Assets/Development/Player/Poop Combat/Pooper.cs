using NUnit.Framework.Constraints;
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pooper : MonoBehaviour
{
    [SerializeField] private PoopSystem poopSystem;
    [SerializeField] private PoopFunction poopFunction;
    public PoopType poopType;
    [SerializeField] private CinemachineOrbitalFollow cam;
    [SerializeField] private LayerMask poopableLayer;
    [SerializeField] private float maxRange = 20f; //Adjust as needed for gameplay
    UI_HudController hudController;

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
        hudController = FindAnyObjectByType<UI_HudController>();
        
        var camArray = FindObjectsByType<CinemachineOrbitalFollow>();
        foreach(var found in camArray)
        {
            if (found.CompareTag("Player"))
            {
                cam = found;
            }
        }
        player = this.gameObject;
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
        poopType = poopFunction.currentPoopType;
        startRot = mesh.transform.localRotation;
    }

    private void Update()
    {
        if (GetIsFlying())
        {
            if (GetIsAiming())
            {
                cam.TargetOffset = new Vector3(0, 3.5f, 0);
            }
            else cam.TargetOffset = new Vector3(0, 0, 0); 
        }
        if (GetIsFlying()==false && isAiming && !isTurning)
        {
            RotateMeshToCamera();
        }

        if (!isTurning) return;
        if (spinTime<1)
        {
            spinTime += Time.deltaTime / spinDuration;

            float easeTime = Mathf.SmoothStep(0f, 1f, spinTime);
            mesh.transform.localRotation = Quaternion.Slerp(startRot, endRot, easeTime);
            if (spinTime >= 1f)
            {
                isTurning = false;
            }
        }


    }

    void RotateMeshToCamera()
    {
        var offset = Quaternion.Euler(0f, 180f, 0f);
        Vector3 forward = -Camera.main.transform.forward;
        forward.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(forward);

        mesh.transform.rotation = Quaternion.Slerp(
            mesh.transform.rotation,
            targetRot ,
            12f * Time.deltaTime
        );
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
        if (groundComp.GetIsFlying() == false)
        {
            groundComp.enabled = false;
            hudController.ShowReticle();
            Debug.Log("Aiming started");
            endRot = startRot * Quaternion.Euler(0f, 180f, 0f);
            spinTime = 0f;
            isTurning = true;
        }
        else return;

        //Show aiming UI here if needed
    }


    private void OnAimCanceled(InputAction.CallbackContext ctx)
    {
        groundComp.enabled = true;
        hudController.HideReticle();
        isAiming = false;
        Debug.Log("Aiming canceled");
        endRot = startRot;
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
            if (poopSystem.TryPoop())
            {
                Vector3 target = GetTarget();

                //Get player velocity from pigeon rigidbody
                Vector3 playerVelocity = pigeon.linearVelocity;
                poopFunction.currentPoopType = poopType;
                poopFunction.FirePoop(target , playerVelocity);
            }
        }else
        if (!isFlying && isAiming)
        {
            if (poopSystem.TryPoop())
            {
                poopFunction.FireGroundPoop();
            }
        }
    }

    private Vector3 GetTarget()
    {

        RaycastHit hit;
        if (Physics.SphereCast(transform.position,200f,Vector3.down,out hit,10f,poopableLayer))
        {
            Debug.DrawLine(transform.position,hit.point);
            Debug.Log("Target Hit: " + hit.collider.name);
            return hit.point;
        }else return Vector3.down;

        
    }

    private void OnLevelWasLoaded(int level)
    {
        hudController = FindAnyObjectByType<UI_HudController>();
        var camArray = FindObjectsByType<CinemachineOrbitalFollow>();
        foreach (var found in camArray)
        {
            if (found.CompareTag("Player"))
            {
                cam = found;
            }
        }
    }

}
