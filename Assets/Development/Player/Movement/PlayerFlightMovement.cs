using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.Interpolators;

public class PlayerFlightMovement : MonoBehaviour
{
    GroundCheck groundCheck;
    Rigidbody playerBody;
    StaminaSystem playerStamina;
    [SerializeField] Transform meshTransform;

    bool isFlying = false;
    bool gliding = true;
    bool flapUp = false;
    bool isDiving = false;
    bool isSlowFlap = false;
    bool isStalling = false;
    bool isSpeedUp = false;

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
    [SerializeField] float diveSpeed = 100f;

    [Header("Airstall Variables: ")]
    [SerializeField] float stallTime = .5f;
    float currentStallTime = 0f;
    [SerializeField] float stallHeight = 1f;
    Vector3 stallStartLocation;
    bool reverseStallLerp = false;

    float flapUpVelocity;

    float horizontalMovement, forwardMovement;

    InputAction moveAction;
    InputAction flapAction;
    InputAction diveAction;
    InputAction stallAction;

    public bool GetIsGliding()
    {
        return gliding;
    }

    public bool GetFlapUp()
    {
        return flapUp;
    }

    public bool GetIsDiving()
    {
        return isDiving;
    }

    public bool GetIsSlowFlap()
    {
        return isSlowFlap;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerStamina = GetComponent<StaminaSystem>();
        groundCheck = GetComponentInChildren<GroundCheck>();

        flapUpVelocity = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) * flapUpHeight);

        moveAction = InputSystem.actions.FindAction("Move");
        flapAction = InputSystem.actions.FindAction("Jump");
        diveAction = InputSystem.actions.FindAction("Dive");
        stallAction = InputSystem.actions.FindAction("AirStall");
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying && !isStalling)
        {
            if (groundCheck.IsGrounded(false))
            {
                ReturnToWalkState();
            }

            PlayerInput();
        }

        if (isStalling)
        {
            if (currentStallTime < stallTime && !reverseStallLerp)
            {
                currentStallTime += Time.deltaTime;
                transform.position = new Vector3(stallStartLocation.x, Mathf.Lerp(stallStartLocation.y, stallStartLocation.y + stallHeight, currentStallTime / stallTime), stallStartLocation.z);
            }
            else if (currentStallTime > 0 && reverseStallLerp)
            {
                currentStallTime -= Time.deltaTime;
                transform.position = new Vector3(stallStartLocation.x, Mathf.Lerp(stallStartLocation.y, stallStartLocation.y + stallHeight, currentStallTime / stallTime), stallStartLocation.z);
            }
            else
                ReturnToWalkState();
        }
    }

    void FixedUpdate()
    {
        if (isFlying && !isDiving && !isStalling)
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

        if (forwardMovement > 0 && !flapUp)
        {
            isSpeedUp = true;

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
            isSlowFlap = true;
            Vector3 tempVel = playerBody.linearVelocity;
            
            playerBody.linearVelocity -= transform.forward * stallDownSpeed * Time.deltaTime;

            Vector3 currentAngle = meshTransform.eulerAngles + new Vector3(forwardMovement, 0, 0) * tiltSpeed * Time.deltaTime;

            // Weird math to get relative angle
            currentAngle.x = Mathf.Clamp(((currentAngle.x + 540) % 360) - 180, -25f, 25f);
            meshTransform.rotation = Quaternion.Euler(currentAngle);
        }
        else
        {
            isSlowFlap = false;
            isSpeedUp = false;
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
        if (gliding && !isSpeedUp)
        {
            Vector3 forwardGlideAmount = transform.forward * baseGlideSpeed * Time.deltaTime;
            forwardGlideAmount.y = playerBody.linearVelocity.y;
            playerBody.linearVelocity = forwardGlideAmount;
        }
    }

    async void FlapUp(InputAction.CallbackContext context)
    {
        if (isFlying && !isDiving && !isStalling)
        {
            if (playerStamina.UseStamina(flapStaminaAmount))
            {
                playerBody.linearVelocity = new Vector3(playerBody.linearVelocity.x, flapUpVelocity, playerBody.linearVelocity.z);
                flapUp = true;
                await Task.Delay(500);
                flapUp = false;
            }
            
        }
    }

    async void Dive(InputAction.CallbackContext context)
    {
        if (isDiving || !isFlying) return;
        
        isDiving = true;
        playerBody.AddForce(transform.forward * diveSpeed);
        playerBody.AddForce(Vector3.down * diveSpeed);
        await Task.Delay(1500);
        isDiving = false;
    }

    void AirStall(InputAction.CallbackContext context)
    {
        if (isStalling || !isFlying) return;

        playerBody.linearVelocity = Vector3.zero;
        
        stallStartLocation = transform.position;
        isStalling = true;
        playerBody.useGravity = false;
    }

    public void InitiateFlight()
    {
        isFlying = true;

        // check if player hits spacebar
        flapAction.started += FlapUp;
        diveAction.performed += Dive;
        stallAction.performed += AirStall;

        GetComponent<VFXController>().ToggleStreakOn();
        FlapUp(new InputAction.CallbackContext());
    }

    void ReturnToWalkState()
    {
        isFlying = false;
        isDiving = false;
        isStalling = false;
        reverseStallLerp = false;
        currentStallTime = 0;

        // check if player hits spacebar
        flapAction.started -= FlapUp;
        diveAction.performed -= Dive;
        stallAction.performed -= AirStall;

        GetComponent<VFXController>().ToggleStreakOff();
        meshTransform.localRotation = Quaternion.Euler(Vector3.zero);
        GetComponent<PlayerGroundMovement>().InitiateWalkState();
    }
}
