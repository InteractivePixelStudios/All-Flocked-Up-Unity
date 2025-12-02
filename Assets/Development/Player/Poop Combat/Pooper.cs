using System;
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

    private bool isAiming = false; // Track if the player is currently aiming
    private PlayerInput playerInput;
    private InputAction aimAction;
    private InputAction poopAction;
    private GameObject player;

    float spinTime = 0f;
    float spinDuration = 1f;

    // #region Setup & Init

    //Switching to new input system - JK Oct/23
    private void Start()
    {
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
        if (spinTime < spinDuration)
        {
            spinTime += Time.deltaTime;
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
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.Euler(0f, transform.eulerAngles.y + 180f, 0f);

        transform.rotation = Quaternion.Slerp(start,end,spinTime);
        //Show aiming UI here if needed
    }

    private void OnAimCanceled(InputAction.CallbackContext ctx)
    {
        isAiming = false;
        Debug.Log("Aiming canceled");
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.Euler(0f, transform.eulerAngles.y - 180f, 0f);
        transform.rotation = Quaternion.Slerp(start, end, spinTime);
        //Hide aiming UI here if needed
    }

    private void OnPoopPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Poop action performed");

        if (isAiming)
        {
            TryPooping();
        }
    }

    #endregion


    private void TryPooping()
    {
        Debug.Log("PoopCalled");
        if (poopSystem.TryPoop())
        {
            Vector3 target = GetTarget();

            //Get player velocity from pigeon rigidbody
            Vector3 playerVelocity = pigeon.linearVelocity;
            poopFunction.FirePoop(target, playerVelocity);
            Debug.Log("Pooping");
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
