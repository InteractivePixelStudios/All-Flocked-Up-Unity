using UnityEngine;
using UnityEngine.InputSystem;

public class GraffitiZone : MonoBehaviour
{
    [SerializeField] private BoxCollider graffitiZoneCollider;
    [SerializeField] private bool playerInMe = false;
    [SerializeField] InputAction grafit;
    [SerializeField] private UI_CanvasController canvasController;

    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera grafittiCam;

    void Start()
    {
        grafittiCam.enabled = false;
        if (TryGetComponent<BoxCollider>(out BoxCollider collider))
        {
            graffitiZoneCollider = collider;
            Debug.Log("BoxCollider component found and assigned to graffitiZoneCollider.");
        }
        else
        {
            Debug.LogError("BoxCollider component not found on GraffitiZone.");
        }

        grafit = InputSystem.actions.FindAction("Interact");

        PlayerInput();

        canvasController = FindFirstObjectByType<UI_CanvasController>();
        if (canvasController == null)
        {
            Debug.LogError("UI_CanvasController not found in the scene.");
        }
        else
        {
            Debug.Log("UI_CanvasController found and assigned to canvasController.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayerInput()
    {
        grafit.started += ctx => GrafittiTime();
    }

    void GrafittiTime()
    {
        if (playerInMe)
        {
            canvasController.SetUIMap();
            Debug.Log("Player is interacting with the graffiti zone.");
            // Implement graffiti interaction logic here

            grafittiCam.enabled = true;
            mainCam.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the graffiti zone.");
            //pop up UI here
            playerInMe = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited the graffiti zone.");
            //close UI here
            playerInMe = false;
        }
    }
}
