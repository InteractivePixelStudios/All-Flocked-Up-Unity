using UnityEngine;
using UnityEngine.InputSystem; // only needed temporarily
using Unity.Cinemachine;
using UnityEngine.Serialization;


public class ScreenshotCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private PlayerStateController psc;
    [SerializeField] private Transform camAnchorFront;
    [SerializeField] private Transform camAnchorBack;
    [FormerlySerializedAs("UI_Hud")] [SerializeField] private UI_HudController uiHud;
    
    
    private bool isSelfie = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider cameraCollider;
    private CinemachineBrain cinemachineBrain;
    void Start()
    {
        
        cam =  Camera.main;
        cameraCollider = cam.GetComponent<Collider>();
        cinemachineBrain = cam.GetComponent<CinemachineBrain>();
        camAnchorFront = transform.Find("FrontCamAnchor");
        camAnchorBack = transform.Find(("SelfieCamAnchor"));
        uiHud = FindAnyObjectByType<UI_HudController>();

        if (!camAnchorFront)
            Debug.Log("camAnchorFront is null");
        if (!camAnchorBack)
            Debug.Log("camAnchorBack is null");
        

    }


    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame) //temporary for test
        {
            if (psc.CurrentState == PlayerState.PhotoMode)
            {
            

                EndPhotoMode();
                Debug.Log("P pressed");
            }
            else
                StartPhotoMode();
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
