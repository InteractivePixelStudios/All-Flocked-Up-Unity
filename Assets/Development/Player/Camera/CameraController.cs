using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : Singleton<CameraController>
{
    public Transform player;           // Assign in Inspector or via script
    public Transform respawnTarget;    // Assign the object to watch after death
    public Vector3 respawnOffset = new Vector3(0, 20, 0); // Height above target
    public float transitionSpeed = 2f;
    public PlayerStateController playerState;
    private bool watchPlayer = true;
    Pooper playerPoop;
    [SerializeField]
    float cameraDistanceFromPlayer = 10; // Distance we want the player from camera (later to be used for zooming in/out)

    InputAction lookAction; // Action for mouse input
    float x, y; // Mouse input values
    private float lookSens = 1f;

    void Start()
    {
        player = FindAnyObjectByType<PlayerFlightMovement>().transform;
        playerState = player.gameObject.GetComponent<PlayerStateController>();
        playerPoop = player.GetComponent<Pooper>();
        lookAction = InputSystem.actions.FindAction("Look");
        respawnTarget = FindAnyObjectByType<NestBase>().transform;
        transform.position = player.position + new Vector3(0, 5, -10);
        transform.LookAt(player);
    }

    void LateUpdate()
    {
        
        if (playerState.CurrentState == PlayerState.PhotoMode)
        {
            return; // Don't do camera movement in photo mode
        }
        //if (playerPoop.GetIsAiming())
        //{
        //    transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * transitionSpeed);
        //}

        
        if (watchPlayer && player != null)
        {
            // Follow player
           CameraMovement();
        }
        else if (respawnTarget != null)
        {
            // Move to top-down view
            Vector3 targetPos = respawnTarget.position + respawnOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * transitionSpeed);
        }
    }

    void Update()
    {
        if (playerState.CurrentState != PlayerState.PhotoMode)
        {
            PlayerInput();
        }
    }

    void PlayerInput()
    {
        x = lookAction.ReadValue<Vector2>().x * lookSens;
        y = lookAction.ReadValue<Vector2>().y * lookSens;
    }

    public void SetLookSens(float value)
    {
        lookSens = value;
    }

    void CameraMovement()
    {
        Vector3 tempPos = transform.forward * -cameraDistanceFromPlayer;

        tempPos -= transform.right * x * (Time.deltaTime /4);
        tempPos -= transform.up * y * (Time.deltaTime/4);

        transform.position = player.transform.position + tempPos;
        transform.LookAt(player);
    }

    public void SwitchToRespawnCam()
    {
        watchPlayer = false;
    }

    public void SwitchToPlayer()
    {
        watchPlayer = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        this.enabled = true;
    }
}
