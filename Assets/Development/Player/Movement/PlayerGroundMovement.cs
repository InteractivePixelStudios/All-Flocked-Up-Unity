using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(PlayerFlightMovement), typeof(StaminaSystem))]

public class PlayerGroundMovement : MonoBehaviour
{

    Rigidbody playerBody;
    StaminaSystem playerStamina;
    PlayerFlightMovement playerFlightMovement;
    PlayerStealthSystem playerStealthComponent;
    static GroundCheck groundCheck;
    Transform cameraRef;

    [Header("Additional Requirements")]
    [SerializeField] GameObject crouchVinete;

    // movement variables
    [Header("Movement Speed: ")]
    [SerializeField] float moveSpeed = 500f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float jumpHeight = 150f;

    [SerializeField]float currentMaxSpeed;
    float currentSpeed;

    [Header("Counter Movement: ")]
    [SerializeField] float counterMovement = 0.175f;
    [SerializeField] float threshold = 0.01f;

    [Header("Step Variables: ")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmoothing = 2f;
    [SerializeField] float stepCastDistance = .2f;


    [Header("Other Variables")]
    [SerializeField] float rotationLerpSpeed = 0.1f;
    [SerializeField] float sprintStaminaAmount = 0.2f;
    [SerializeField] float sprintTriggerStaminaTime = .5f;


    //playerInput
    float x, z;
    bool crouching, sprinting;
    bool isJumping = false;
    bool isFlying = false;
    float sprintTimer = 0f;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction crouchAction;

    private void Awake()
    { 
        playerBody = GetComponent<Rigidbody>();
        playerStamina = GetComponent<StaminaSystem>();
        playerStealthComponent = GetComponent<PlayerStealthSystem>();

        stepRayUpper.transform.localPosition = new Vector3(stepRayUpper.transform.localPosition.x, stepHeight, stepRayUpper.transform.localPosition.z);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraRef = Camera.main.transform;
        groundCheck = GetComponentInChildren<GroundCheck>();
        playerFlightMovement = GetComponent<PlayerFlightMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMaxSpeed = maxSpeed;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        PlayerInput();
    }

    // Update is called once per frame
    void Update()
    {
       // if (!isFlying)
           // PlayerInput();
    }

    void FixedUpdate()
    {
        if (isFlying)
            return;

        Movement();

        if (sprinting)
            Sprint();

        if (isJumping)
            if (groundCheck.IsGrounded())
                isJumping = false;
    }

    void PlayerInput()
    {
        //x = moveAction.ReadValue<Vector2>().x;
        //z = moveAction.ReadValue<Vector2>().y;

        // check if player hits spacebar
        jumpAction.performed += ctx => Jump();
        // check if player crouches with C or left Control
        crouchAction.started += ctx => StartCrouch();
        crouchAction.canceled += ctx => EndCrouch();
        sprintAction.started += ctx => StartSprint();
        sprintAction.canceled += ctx => EndSprint();
    }

    void Movement()
    {
        if (isFlying)
            return;

        x = moveAction.ReadValue<Vector2>().x;
        z = moveAction.ReadValue<Vector2>().y;

        if (x == 0 && z == 0)
        {
            //playerBody.linearVelocity = new Vector3(0, playerBody.linearVelocity.y, 0);
            CounterMovement(x, z, FindVelRelativeToLook());
            return;
        }

        if (x != 0 || z != 0)
            StepClimb();

        //Set max speed
        //float maxSpeed = currentMaxSpeed;

        // add extra gravity to the player
        playerBody.AddForce(Vector3.down * Time.deltaTime * Physics.gravity.y);

        // Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        currentSpeed = moveSpeed;

        // Counteract sliding and sloppy movement
        CounterMovement(x, z, mag);

        // check whether adding speed will bring player over max speed
        if (x > 0 && xMag > currentMaxSpeed) x = 0;
        if (x < 0 && xMag < -currentMaxSpeed) x = 0;
        if (z > 0 && yMag > currentMaxSpeed) z = 0;
        if (z < 0 && yMag < -currentMaxSpeed) z = 0;


        if (z > 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, cameraRef.eulerAngles.y, rotationLerpSpeed), transform.eulerAngles.z);
        }
        //Apply forces to playerBody
        playerBody.AddForce(transform.forward * z * currentSpeed * Time.deltaTime);
        playerBody.AddForce(transform.right * x * currentSpeed * Time.deltaTime);      
    }

    void Jump()
    {
        if (isFlying)
            return;

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

    void StartCrouch()
    {
        if (isFlying || sprinting)
            return;

        if (!crouching)
        {
            crouchVinete.SetActive(true);
            crouching = true;
            currentMaxSpeed = crouchSpeed;
            playerStealthComponent.ToggleStealthOn();
            Debug.Log("Crouched");
        }
    }

    void EndCrouch()
    {
        if (crouching)
        {
            crouchVinete.SetActive(false);
            crouching = false;
            currentMaxSpeed = maxSpeed;
            playerStealthComponent.ToggleStealthOff();
            Debug.Log("UnCrouched");
        }
    }

    void StartSprint()
    {
        if (isFlying || crouching)
            return;

        if (playerStamina.UseStamina(sprintStaminaAmount))
        {
            sprinting = true;
            currentMaxSpeed = sprintSpeed;
        }
    }

    void Sprint()
    {
        if (!sprinting)
            return;

        if (sprintTimer < sprintTriggerStaminaTime)
            sprintTimer += Time.deltaTime;
        else if (sprintTimer >= sprintTriggerStaminaTime)
        {
            sprintTimer -= sprintTriggerStaminaTime;
            if (!playerStamina.UseStamina(sprintStaminaAmount))
                EndSprint();
        }

    }

    void EndSprint()
    {
        if (sprinting)
        {
            sprinting = false;
            currentMaxSpeed = maxSpeed;
            playerStamina.RegenStamina();
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

    void StepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, stepCastDistance))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, stepCastDistance))
            {
                playerBody.position -= new Vector3(0f, -stepSmoothing * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower30;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1f, 0f, 1f), out hitLower30, stepCastDistance))
        {
            RaycastHit hitUpper30;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1f, 0f, 1f), out hitUpper30, stepCastDistance))
            {
                playerBody.position -= new Vector3(0f, -stepSmoothing * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus30;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1f, 0f, 1f), out hitLowerMinus30, stepCastDistance))
        {
            RaycastHit hitUpperMinus30;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1f, 0f, 1f), out hitUpperMinus30, stepCastDistance)) 
            {
                playerBody.position -= new Vector3(0f, -stepSmoothing * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower60;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(2f, 0f, 1f), out hitLower60, stepCastDistance))
        {
            RaycastHit hitUpper60;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(2f, 0f, 1f), out hitUpper60, stepCastDistance))
            {
                playerBody.position -= new Vector3(0f, -stepSmoothing * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus60;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-2f, 0f, 1f), out hitLowerMinus60, stepCastDistance))
        {
            RaycastHit hitUpperMinus60;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-2f, 0f, 1f), out hitUpperMinus60, stepCastDistance))
            {
                playerBody.position -= new Vector3(0f, -stepSmoothing * Time.deltaTime, 0f);
            }
        }
    }

    public void InitiateFlight()
    {
        isFlying = true;
        playerBody.useGravity = false;
        playerStamina.CancelRegen();
        playerFlightMovement.InitiateFlight();
    }

    public void InitiateWalkState()
    {
        isFlying = false;
        playerBody.useGravity = true;
        if (groundCheck.IsGrounded())
            playerStamina.RegenStamina();
    }

}
