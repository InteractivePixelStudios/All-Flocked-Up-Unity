using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlightMovement : MonoBehaviour
{
    GroundCheck groundCheck;
    Rigidbody playerBody;

    bool isFlying = false;
    bool gliding = true;

    [Header("Flight Speeds: ")]
    [SerializeField] float baseGlideSpeed = 400f;
    [SerializeField] float maxDownwardVelocity = -3f;

    [Header("Flap Variables: ")]
    [SerializeField] float flapUpHeight = 5f;

    [Header("Movement Variables: ")]
    [SerializeField] float rotateSpeed = 80f;
    [SerializeField] float glideDownSpeed = 1000f;
    [SerializeField] float glideDownDropSpeed = 1f;
    [SerializeField] float stallDownSpeed = .00001f;

    float flapUpVelocity;

    float horizontalMovement, forwardMovement;

    InputAction moveAction;
    InputAction flapAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();

        flapUpVelocity = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) * flapUpHeight);

        moveAction = InputSystem.actions.FindAction("Move");
        flapAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying)
        {
            if (groundCheck.IsGrounded())
                ReturnToWalkState();

            PlayerInput();
        }
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            // add "Gravity" to player
            playerBody.AddForce((Vector3.down * Mathf.Abs(Physics.gravity.y / 4)) * Time.deltaTime, ForceMode.VelocityChange);
            // clamp the max downward velocity
            playerBody.linearVelocity = new Vector3(playerBody.linearVelocity.x, Mathf.Clamp(playerBody.linearVelocity.y, maxDownwardVelocity, 10000) , playerBody.linearVelocity.z);
            FlightMovement();
            ForwardGlide();
        }
    }

    void PlayerInput()
    {
        // check x and z axis movement
        horizontalMovement = moveAction.ReadValue<Vector2>().x;
        forwardMovement = moveAction.ReadValue<Vector2>().y;

        // check if player hits spacebar
        flapAction.started += ctx => FlapUp();
    }

    void FlightMovement()
    {
        if (horizontalMovement < 0 || horizontalMovement > 0)
        {
            transform.Rotate(new Vector3(0, rotateSpeed * horizontalMovement * Time.deltaTime, 0));
        }

        if (forwardMovement > 0)
        {
            if (gliding)
                gliding = false;

            Vector3 glideDownAmount = transform.forward * glideDownSpeed * Time.deltaTime;
            glideDownAmount.y = playerBody.linearVelocity.y - (glideDownDropSpeed * Time.deltaTime);
            playerBody.linearVelocity = glideDownAmount;
        }
        else if (forwardMovement < 0)
        {
            if (gliding)
                gliding = false;
            Vector3 tempVel = playerBody.linearVelocity;
            Debug.Log(tempVel);
            playerBody.linearVelocity = new Vector3(Mathf.Clamp(tempVel.x - (stallDownSpeed * Time.deltaTime), 0, 10000), tempVel.y, Mathf.Clamp(tempVel.z - (stallDownSpeed * Time.deltaTime), 0, 10000));
        }
        else
        {
            if (!gliding)
                gliding = true;
        }
    }

    void ForwardGlide()
    {
        if (gliding)
        {
            Vector3 forwardGlideAmount = transform.forward * baseGlideSpeed * Time.deltaTime;
            forwardGlideAmount.y = playerBody.linearVelocity.y;
            playerBody.linearVelocity = forwardGlideAmount;
        }
    }

    void FlapUp()
    {
        if (isFlying)
        {
            playerBody.linearVelocity = new Vector3(playerBody.linearVelocity.x, flapUpVelocity, playerBody.linearVelocity.z);
        }
    }

    public void InitiateFlight()
    {
        isFlying = true;
        FlapUp();
    }

    void ReturnToWalkState()
    {
        isFlying = false;
        GetComponent<PlayerGroundMovement>().InitiateWalkState();
    }
}
