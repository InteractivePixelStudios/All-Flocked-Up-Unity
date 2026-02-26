using UnityEngine;
using UnityEngine.InputSystem;

public class GraffitiZone : MonoBehaviour
{
    private BoxCollider graffitiZoneCollider;
    private bool playerInMe = false;
    InputAction grafit;
    private UI_CanvasController canvasController;

    void Start()
    {
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
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the graffiti zone.");
            //pop up UI here
            //press E to do it
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
