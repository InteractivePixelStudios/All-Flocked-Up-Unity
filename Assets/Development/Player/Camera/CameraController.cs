using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : Singleton<CameraController>
{
    public Transform player;           // Assign in Inspector or via script
    public Transform respawnTarget;    // Assign the object to watch after death
    public Vector3 respawnOffset = new Vector3(0, 20, 0); // Height above target
    public float transitionSpeed = 2f;

    private bool watchPlayer = true;

    [SerializeField]
    float cameraDistanceFromPlayer = 10; // Distance we want the player from camera (later to be used for zooming in/out)

    InputAction lookAction; // Action for mouse input
    float x, y; // Mouse input values

    void Start()
    {
        player = FindFirstObjectByType<PlayerFlightMovement>().transform;
        lookAction = InputSystem.actions.FindAction("Look");
        transform.position = player.position + new Vector3(0, 5, -10);
        transform.LookAt(player);
    }

    void LateUpdate()
    {
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
        PlayerInput();
    }

    void PlayerInput()
    {
        x = lookAction.ReadValue<Vector2>().x;
        y = lookAction.ReadValue<Vector2>().y;
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
