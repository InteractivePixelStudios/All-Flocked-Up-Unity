using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(PlayerFlightMovement))]

public class PlayerGroundMovement : MonoBehaviour
{

    private Rigidbody playerBody;
    private PlayerFlightMovement playerFlightMovement;
    private static GroundCheck groundCheck;


    // movement variables
    [Header("Movement Speed: ")]
    [SerializeField]
    float moveSpeed = 4500f;
    [SerializeField]
    float maxSpeed = 10f;
    [SerializeField]
    float crouchSpeed = 4f;
    [SerializeField]
    float jumpHeight = 400f;

    float currentMaxSpeed;
    float currentSpeed;

    [Header("Counter Movement: ")]
    [SerializeField]
    float counterMovement = 0.175f;
    [SerializeField]
    float threshold = 0.01f;

    //playerInput
    float x, z;
    bool jumping, crouching;
    bool isJumping = false;
    public bool isFlying = false;

    InputAction moveAction;
    InputAction jumpAction;

    private void Awake() => playerBody = GetComponent<Rigidbody>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundCheck = GetComponentInChildren<GroundCheck>();
        playerFlightMovement = GetComponent<PlayerFlightMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlying)
            PlayerInput();
    }

    void FixedUpdate()
    {
        if (crouching)
            currentMaxSpeed = crouchSpeed;
        else
            currentMaxSpeed = maxSpeed;

        Movement();
        if (jumping)
            Jump();

        if (isJumping)
            if (groundCheck.IsGrounded())
                isJumping = false;
    }

    void PlayerInput()
    {
        // check x and z axis movement
        x = moveAction.ReadValue<Vector2>().x;
        z = moveAction.ReadValue<Vector2>().y;
        // check if player hits spacebar
        jumpAction.started += ctx => Jump();
        // check if player crouches with C or left Control
        crouching = Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl);
    }

    void Movement()
    {
        //Set max speed
        float maxSpeed = currentMaxSpeed;

        // add extra gravity to the player
        playerBody.AddForce(Vector3.down * Time.deltaTime * Physics.gravity.y);

        // Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        currentSpeed = moveSpeed;

        // Counteract sliding and sloppy movement
        CounterMovement(x, z, mag);

        // check whether adding speed will bring player over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (z > 0 && yMag > maxSpeed) z = 0;
        if (z < 0 && yMag < -maxSpeed) z = 0;

        //Apply forces to playerBody
        playerBody.AddForce(transform.forward * z * currentSpeed * Time.deltaTime);
        playerBody.AddForce(transform.right * x * currentSpeed * Time.deltaTime);
    }

    void Jump()
    {
        // check if player is on the ground to jump
        if (groundCheck.IsGrounded() && !isJumping)
        {
            isJumping = true;
            // add verticle force to make the player jump
            playerBody.AddForce(transform.up * jumpHeight);
        }
        else
        {
            InitiateFlight();
        }
    }

    // method gets the players directional speed to be able to limit speed based on max speed
    public Vector2 FindVelRelativeToLook()
    {
        // players current forward angle
        float lookAngle = transform.eulerAngles.y;
        // players angle of movement with 0 being forward
        float moveAngle = Mathf.Atan2(playerBody.linearVelocity.x, playerBody.linearVelocity.z) * Mathf.Rad2Deg;

        // finds the relative velocity angle compared to the moveAngle
        float velY = Mathf.DeltaAngle(lookAngle, moveAngle);
        // the x velocity angle is just 90 degrees away
        float velX = 90 - velY;


        // multiply the magnitude by the angle to get magnitude in each direction
        float magnitude = playerBody.linearVelocity.magnitude;
        float yMag = magnitude * Mathf.Cos(velY * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(velX * Mathf.Deg2Rad);

        // return directional magnitude
        return new Vector2(xMag, yMag);
    }

    // method applies counter movement to better stop the players movement and prevent sliding
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        //Counter movement based on direction of movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            playerBody.AddForce(moveSpeed * transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            playerBody.AddForce(moveSpeed * transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        // Limit the speed of diagonal running to the maxSpeed
        if (Mathf.Sqrt((Mathf.Pow(playerBody.linearVelocity.x, 2) + Mathf.Pow(playerBody.linearVelocity.z, 2))) > currentMaxSpeed)
        {
            // save the falling speed
            float tempFallspeed = playerBody.linearVelocity.y;
            // limit the speed of the player
            Vector3 normSpeed = playerBody.linearVelocity.normalized * currentMaxSpeed;
            // set player speed but keep the falling speed the same
            playerBody.linearVelocity = new Vector3(normSpeed.x, tempFallspeed, normSpeed.z);
        }
    }

    public void InitiateFlight()
    {
        isFlying = true;
        playerFlightMovement.InitiateFlight();
    }

    public void InitiateWalkState()
    {
        isFlying = false;
    }

}
