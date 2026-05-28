using UnityEngine;
using UnityEngine.InputSystem; // only needed temporarily
using Unity.Cinemachine;
using UnityEngine.Serialization;


public class ScreenshotCameraController : MonoBehaviour
{
    //prefab references - assign in inspector
    [SerializeField] private PlayerStateController psc;
    [SerializeField] private Transform camAnchorFront;
    [SerializeField] private Transform camAnchorBack;
    
    //cross-scene references, need to be resolved at runtime
    [SerializeField] private Camera cam;
    [SerializeField] private UI_HudController uiHud;
    private Collider cameraCollider;
    private CinemachineBrain cinemachineBrain;
    private Transform originalCamParent;   // remember where the camera came from

    //internal variables
    [SerializeField] bool isSelfie = false;
    private float pitch = 0f;
    private float yaw = 0f;
    [SerializeField] private float orientSpeed = 50f;
    [SerializeField] private float pitchLimit = 45f;
    [SerializeField] private float yawLimit = 60f;
    [SerializeField] private InputAction moveAction;

    
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //setting reference variables
        cam =  Camera.main;
        if (cam != null)
        {
            cameraCollider = cam.GetComponent<Collider>();
            cinemachineBrain = cam.GetComponent<CinemachineBrain>();
        }
       
        uiHud = FindAnyObjectByType<UI_HudController>();
        moveAction = InputSystem.actions.FindAction("Move");
        if (psc == null) Debug.LogError("PlayerStateController not wired", this);
        if (camAnchorFront == null) Debug.LogError("FrontCamAnchor not wired", this);
        if (camAnchorBack == null) Debug.LogError("SelfieCamAnchor not wired", this);


    }


    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame) //temporary for test
        {
            //if in photomode, leave photomode
            if (psc.CurrentState == PlayerState.PhotoMode)
            {


                EndPhotoMode();
                Debug.Log("P pressed");
            }
            //if in ground mode (not flying) enter photomode
            else if (psc.CurrentState == PlayerState.GroundMove)
            {
            

                StartPhotoMode();
            }
        }
        if (psc.CurrentState == PlayerState.PhotoMode)
        {
            Debug.Log("in photo mode, waiting for space");
            //change this later as well for inputAction system
            
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("space pressed, flipping");
                FlipCamera();
            }
        }

        
       
    }

    private void StartPhotoMode()
    {
       psc.EnterPhotoMode(); 
       Debug.Log("Photo Mode Entered");

       if (cameraCollider)
       { Debug.Log("Main Camera Collider Component disabled by Photo Mode"); cameraCollider.enabled = false;}
       if (cinemachineBrain)
       { Debug.Log("Main Camera CinemachineBrain Component disabled by Photo Mode"); cinemachineBrain.enabled = false;}

       //move cam to anchor
       cam.transform.parent = camAnchorFront;
       cam.transform.localPosition = Vector3.zero;
       cam.transform.localRotation = Quaternion.identity;
       
       uiHud.ToggleMainHUD(false);
       uiHud.ToggleCameraOverlay(true);
    }

    private void EndPhotoMode()
    {
        psc.ExitPhotoMode();
        Debug.Log("Photo Mode Exited");
        
        { Debug.Log("Main Camera Collider Component enabled by Photo Mode"); cameraCollider.enabled = true;}
        { Debug.Log("Main Camera CinemachineBrain Component enabled by Photo Mode"); cinemachineBrain.enabled = true;}

        //reset to normal camera control
        cam.transform.parent = null;

        //reset to default camera orientation for future camera angles
        isSelfie = false;
        
        uiHud.ToggleMainHUD(true);
        uiHud.ToggleCameraOverlay(false);
    }
    
    private void FlipCamera()
    {
        isSelfie = !isSelfie;
        Transform target = isSelfie ? camAnchorBack : camAnchorFront;
        cam.transform.parent = target;
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;
    }
    
    
    
    
    private void TakePhoto()
    {}
    
    private void OrientCamera()
    {}
    
    private void ZoomCamera()
    {}
    
}
