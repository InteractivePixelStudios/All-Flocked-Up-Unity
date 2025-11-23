using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlightMovement : MonoBehaviour
{
    GroundCheck groundCheck;
    Rigidbody playerBody;
    StaminaSystem playerStamina;
    [SerializeField] Transform meshTransform;

    bool isFlying = false;
    bool gliding = true;

    [Header("Flight Speeds: ")]
    [SerializeField] float baseGlideSpeed = 400f;
    [SerializeField] float maxDownwardVelocity = -3f;

    [Header("Flap Variables: ")]
    [SerializeField] float flapUpHeight = 5f;
    [SerializeField] float flapStaminaAmount = 2f;

    [Header("Movement Variables: ")]
    [SerializeField] float rotateSpeed = 80f;
    [SerializeField] float glideDownSpeed = 1000f;
    [SerializeField] float glideDownDropSpeed = 1f;
    [SerializeField] float stallDownSpeed = .00001f;
    [SerializeField] float tiltSpeed = 100f;

    float flapUpVelocity;

    float horizontalMovement, forwardMovement;

    InputAction moveAction;
    InputAction flapAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerStamina = GetComponent<StaminaSystem>();
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

            Vector3 currentAngle = meshTransform.eulerAngles + new Vector3(0, 0, -horizontalMovement) * tiltSpeed * Time.deltaTime;
    
            // Weird math to get relative angle
            currentAngle.z = Mathf.Clamp(((currentAngle.z + 540) % 360) - 180, -25f, 25f);
            meshTransform.rotation = Quaternion.Euler(currentAngle);
        }
        else if (meshTransform.localRotation.z != 0)
        {
            Vector3 currentAngle = meshTransform.localEulerAngles;
            if (currentAngle.z < 30)
                currentAngle.z = Mathf.Lerp(meshTransform.localEulerAngles.z, 0, 2f * Time.deltaTime);
            else
                currentAngle.z = Mathf.Lerp(meshTransform.localEulerAngles.z, 360, 2f * Time.deltaTime);

            meshTransform.localRotation = Quaternion.Euler(currentAngle);

            if (meshTransform.localEulerAngles.z < 1)
                meshTransform.localRotation = Quaternion.Euler(new Vector3(meshTransform.eulerAngles.x, 0, 0));

        }

        if (forwardMovement > 0)
        {
            if (gliding)
                gliding = false;

            Vector3 glideDownAmount = transform.forward * glideDownSpeed * Time.deltaTime;
            glideDownAmount.y = playerBody.linearVelocity.y - (glideDownDropSpeed * Time.deltaTime);
            playerBody.linearVelocity = glideDownAmount;

            Vector3 currentAngle = meshTransform.eulerAngles + new Vector3(forwardMovement, 0, 0) * tiltSpeed * Time.deltaTime;

            // Weird math to get relative angle
            currentAngle.x = Mathf.Clamp(((currentAngle.x + 540) % 360) - 180, -25f, 25f);
            meshTransform.rotation = Quaternion.Euler(currentAngle);
        }
        else if (forwardMovement < 0)
        {
            if (gliding)
                gliding = false;
            Vector3 tempVel = playerBody.linearVelocity;
            Debug.Log(tempVel);
            playerBody.linearVelocity = new Vector3(Mathf.Clamp(tempVel.x - (stallDownSpeed * Time.deltaTime), 0, 10000), tempVel.y, Mathf.Clamp(tempVel.z - (stallDownSpeed * Time.deltaTime), 0, 10000));

            Vector3 currentAngle = meshTransform.eulerAngles + new Vector3(forwardMovement, 0, 0) * tiltSpeed * Time.deltaTime;

            // Weird math to get relative angle
            currentAngle.x = Mathf.Clamp(((currentAngle.x + 540) % 360) - 180, -25f, 25f);
            meshTransform.rotation = Quaternion.Euler(currentAngle);
        }
        else
        {
            if (!gliding)
                gliding = true;

            if (meshTransform.localRotation.x != 0)
            {
                Vector3 currentAngle = meshTransform.localEulerAngles;
                if (currentAngle.x < 30)
                    currentAngle.x = Mathf.Lerp(meshTransform.localEulerAngles.x, 0, 2f * Time.deltaTime);
                else
                    currentAngle.x = Mathf.Lerp(meshTransform.localEulerAngles.x, 360, 2f * Time.deltaTime);

                meshTransform.localRotation = Quaternion.Euler(currentAngle);

                if (meshTransform.localEulerAngles.x < 1)
                    meshTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, meshTransform.eulerAngles.z));
            }
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
            if (playerStamina.UseStamina(flapStaminaAmount))
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
        meshTransform.localRotation = Quaternion.Euler(Vector3.zero);
        GetComponent<PlayerGroundMovement>().InitiateWalkState();
    }
}
