using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Threading;

public enum RidingObjects
{
    Skateboard,Kart
}

public interface IRidingObjects
{
     void StartRiding();

     void StopRiding();
}

public class Rider_Base : MonoBehaviour, IRidingObjects
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] protected List<GameObject> basePrefabs = new();
    [SerializeField] protected GameObject baseObject;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected GameObject playerSpot;
    [SerializeField] protected bool isRiding = false;
    private RidingObjects objectEnum;

    private PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;
    InputAction trickAction;

    Vector2 moveInput;
    Vector3 jumpForce;
    [SerializeField]float maxForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        SelectRandom();
        InitRidingObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRiding)
        {
            HoldPlayerPos();
            Vector3 movement = new Vector3(moveInput.y, 0, moveInput.x) * 5f * Time.deltaTime;
            baseObject.transform.position += movement;
        }
            switch (objectEnum)
        {
            case RidingObjects.Skateboard:
                break;
            case RidingObjects.Kart:
                break;
        }
    }

    void FixedUpdate()
    {
        if (isRiding)
        {

        }
    }

    private void InitRidingControls()
    {
        //playerInput.SwitchCurrentActionMap("Rider");

        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        jumpAction = playerInput.actions.FindAction("Jump");
        trickAction = playerInput.actions.FindAction("Trick");

        moveAction.performed += MoveRide;
        lookAction.performed += LookAround;
        jumpAction.performed += AddJumpForce;
        jumpAction.canceled+= JumpRide;
        trickAction.performed += TrickRide;


    }

    private void RemoveRidingControls()
    {
        moveAction.performed -= MoveRide;
        lookAction.performed -= LookAround;
        jumpAction.performed -= AddJumpForce;
        jumpAction.canceled -= JumpRide;
        trickAction.performed -= TrickRide;

        //playerInput.SwitchCurrentActionMap("Player");

    }

    private void InitRidingObject()
    {
        if (playerSpot == null)
        {
            var children = GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.CompareTag("Player"))
                {
                    playerSpot = child.gameObject ;
                    break;
                }
            }
        }
    }

    void SelectRandom()
    {
        var rand = Random.Range(0, 1);
        if(rand == 0)
        {
            objectEnum = RidingObjects.Skateboard;
            baseObject = basePrefabs[0];
        }
        else { objectEnum = RidingObjects.Kart; baseObject = basePrefabs[1]; }
    }

    public void StartRiding()
    {
        //if (playerRef == null) return;
        isRiding = true;
        InitRidingControls();
    }

    public void HoldPlayerPos()
    {
        if (playerRef == null || !isRiding) return;
        playerRef.transform.position = playerSpot.transform.position;
        playerRef.transform.rotation = Quaternion.Euler(0, playerSpot.transform.rotation.y, 0);
    }

    public void StopRiding()
    {
        if(playerRef!=null && isRiding)
        {
            isRiding=false;
            RemoveRidingControls();
        }
    }

    void MoveRide(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

    }

    void LookAround(InputAction.CallbackContext ctx)
    {

    }

    void AddJumpForce(InputAction.CallbackContext ctx)
    {
        var force = Mathf.Clamp((float)ctx.duration,1,maxForce);
        jumpForce = new Vector3(0, 0, force);
    }

    void JumpRide(InputAction.CallbackContext ctx)
    {
        rb.AddForce(jumpForce,ForceMode.Impulse);
    }

    void TrickRide(InputAction.CallbackContext ctx)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRef != null) return;
            playerRef = other.gameObject;

        }
    }

    void OnTriggerExit(Collider other)
    {

    }
}
