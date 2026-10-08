using UnityEngine;
using UnityEngine.InputSystem; // only needed temporarily
using Unity.Cinemachine;
using UnityEngine.Serialization;
using Unity.VisualScripting;


public class ScreenshotCameraController : MonoBehaviour
{
    [Header("Prefab components (assign via inspector)")]
    [SerializeField] private PlayerStateController psc;
    [SerializeField] private Transform camAnchorFront;
    [SerializeField] private Transform camAnchorBack;

    [SerializeField] private CinemachineCamera photoCamFront;
    [SerializeField] private CinemachineCamera photoCamSelfie;
    private CinemachineCamera ActivePhotoCam => isSelfie ? photoCamSelfie : photoCamFront;


    [Header("Run time components (assign via script")]
    [SerializeField] private Camera cam;
    private Collider cameraCollider;
    private CinemachineBrain cinemachineBrain;

    [SerializeField] private UI_HudController _uiHud;

    private UI_HudController uiHud
    {
        get
        {
            if (_uiHud == null) _uiHud = FindAnyObjectByType<UI_HudController>();
            return _uiHud;
        }

    }

    //private Transform originalCamParent;

    //internal variables
    [Header("internal variables")]
    [SerializeField] bool isSelfie = false;
    private float pitch = 0f;
    private float yaw = 0f;
    [SerializeField] private float orientSpeed = 50f;
    [SerializeField] private float pitchLimit = 45f;
    [SerializeField] private float yawLimit = 60f;

    //input actions
    private InputActionMap playerMap;
    private InputActionMap photoMap;
    private InputAction openCameraAction;
    private InputAction exitPhotoModeAction;
    private InputAction flipCameraAction;
    private InputAction snapPhotoAction;
    //to do: orientCamera, Zoom camera actions
    private InputAction orientAction;
    private InputAction zoomAction;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //setting reference variables
        /* cam = Camera.main;
         if (cam != null)
         {
             cameraCollider = cam.GetComponent<Collider>();
             cinemachineBrain = cam.GetComponent<CinemachineBrain>();
         }*/

        //moveAction = InputSystem.actions.FindAction("Move");
        //component check
        if (psc == null) Debug.LogError("PlayerStateController not wired", this);
        if (camAnchorFront == null) Debug.LogError("FrontCamAnchor not wired", this);
        if (camAnchorBack == null) Debug.LogError("SelfieCamAnchor not wired", this);

        //Look up for action map + actions
        playerMap = InputSystem.actions.FindActionMap("Player");
        photoMap = InputSystem.actions.FindActionMap("PhotoMode");
        Debug.Log($"playerMap found: {playerMap != null}");
        Debug.Log($"photoMap found: {photoMap != null}");


        //openCameraAction = playerMap.FindAction("OpenCamera");
        exitPhotoModeAction = photoMap.FindAction("ExitPhotoMode");
        flipCameraAction = photoMap.FindAction("FlipCamera");
        snapPhotoAction = photoMap.FindAction("SnapPhoto");
        orientAction = photoMap.FindAction("Orient");
        zoomAction = photoMap.FindAction("Zoom");

        Debug.Log($"flipCameraAction found: {flipCameraAction != null}");
        Debug.Log($"exitPhotoModeAction found: {exitPhotoModeAction != null}");
        Debug.Log($"snapPhotoAction found: {snapPhotoAction != null}");
        Debug.Log($"orientAction found: {orientAction != null}");
        Debug.Log($"zoomAction found: {zoomAction != null}");


        //subscribe for actions
        //openCameraAction.performed += OnOpenCamera;
        exitPhotoModeAction.performed += OnExitPhotoMode;
        flipCameraAction.performed += OnFlipCamera;
        snapPhotoAction.performed += OnSnapPhoto;


        photoMap.Disable();
    }

    private void OnDestroy()
    {
        //unsub actions when scene transitioning :)
        if (openCameraAction != null) openCameraAction.performed -= OnOpenCamera;
        if (exitPhotoModeAction != null) exitPhotoModeAction.performed -= OnExitPhotoMode;
        if (flipCameraAction != null) flipCameraAction.performed -= OnFlipCamera;
        if (snapPhotoAction != null) snapPhotoAction.performed -= OnSnapPhoto;
        //todo: orientCamera, Zoom camera actions

    }

    //callbacks
    private void OnOpenCamera(InputAction.CallbackContext context)
    {
        if (psc.CurrentState == PlayerState.GroundMove)
            StartPhotoMode();
    }
    private void OnExitPhotoMode(InputAction.CallbackContext context)
    {
        if (psc.CurrentState == PlayerState.PhotoMode)
            EndPhotoMode();
    }
    private void OnFlipCamera(InputAction.CallbackContext context)
    {
        Debug.Log("OnFlipCamera fired");
        FlipCamera();
    }

    private void OnSnapPhoto(InputAction.CallbackContext context) => TakePhoto();

    //todo: OnOrientCamera, OnZoomCamera actions




    //replace update with actions later
    // Update is called once per frame
    private void Update()
    {

        if (psc.CurrentState != PlayerState.PhotoMode) return;
        OrientCamera();
        ZoomCamera();

        /*
        if (Keyboard.current.pKey.wasPressedThisFrame) //temporary for test, replace it later with input action
        {
            //if in photomode, leave photomode
            if (psc.CurrentState == PlayerState.PhotoMode)
            {


                EndPhotoMode();
                Debug.Log("P pressed");
            }

     

            /*commented out for inputaction testing
            //if in ground mode (not flying) enter photomode
           else if (psc.CurrentState == PlayerState.GroundMove)
            {
                StartPhotoMode();
            } 
        }
        */

        /* commented out for inputaction testing

         if (psc.CurrentState == PlayerState.PhotoMode)
           {
               Debug.Log("in photo mode, waiting for space");
               //change this later as well for inputAction system

               if (Keyboard.current.spaceKey.wasPressedThisFrame)
               {
                   Debug.Log("space pressed, flipping");
                   FlipCamera();
               }
           } */
    }


    // -state transition stuff-
    public void CallEnterPhotoMode() => StartPhotoMode();
    public void CallExitPhotoMode() => EndPhotoMode();

    private void StartPhotoMode()
    {

        //fail fast checks
        if (psc == null)
        {
            Debug.LogError("StartPhotoMode: psc not wired", this);
            return;
        }
        /*
        if (cam == null)
        {
            Debug.LogError("StartPhotoMode: main cam missing", this);
            return;
        } */
        if (camAnchorFront == null)
        {
            Debug.LogError("StartPhotoMode: cam anchor missing/not wired", this);
            return;
        }
        if (playerMap == null || photoMap == null)
        {
            Debug.LogError("StartPhotoMode: action maps missing", this);
            return;
        }
        if (photoCamFront == null || photoCamSelfie == null)
        {
            Debug.LogError("StartPhotoMode: photo cams not wired in prefab", this);
            return;
        }


        //all clear, go go go
        psc.EnterPhotoMode(); Debug.Log("Photo Mode Entered");


        /*
            if (cameraCollider)
            { Debug.Log("Main Camera Collider Component disabled by Photo Mode"); cameraCollider.enabled = false;}
            if (cinemachineBrain)
           { Debug.Log("Main Camera CinemachineBrain Component disabled by Photo Mode"); cinemachineBrain.enabled = false;}
         */

        //--NEEDS REFACTORING TO USE PLAYER STATE CONTROLLER AS SINGLE SOURCE OF TRUTH -- swap the active input action map --NEEDS REFACTORING --
        playerMap.Disable();
        photoMap.Enable();
        Debug.Log($"After Enable — photoMap.enabled = {photoMap.enabled}");
        Debug.Log($"flipCameraAction.enabled = {flipCameraAction.enabled}");
        Debug.Log($"flipCameraAction.bindings.Count = {flipCameraAction.bindings.Count}");

        photoCamFront.gameObject.SetActive(true);


        //move cam to anchor
        /*
        cam.transform.parent = camAnchorFront;
       cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity; */



        uiHud.ToggleMainHUD(false);
        uiHud.ToggleCameraOverlay(true);
    }

    private void EndPhotoMode()
    {
        psc.ExitPhotoMode();
        Debug.Log("Photo Mode Exited");

        photoCamFront.gameObject.SetActive(false); //turns em both off and brain returns to gameplay cam
        photoCamSelfie.gameObject.SetActive(false);


        //these should never fail tbh, or else we have a problem... 
        //not my problem anymore, see ya old school crap

        /* if (cameraCollider)
         { Debug.Log("Main Camera Collider Component enabled by Photo Mode"); cameraCollider.enabled = true; }

         if (cinemachineBrain)
         { Debug.Log("Main Camera CinemachineBrain Component enabled by Photo Mode"); cinemachineBrain.enabled = true; }
         */

        //swap the active input action map back
        playerMap.Enable();
        photoMap.Disable();

        //reset to normal camera control
        // cam.transform.parent = null;

        //reset to default camera orientation for future camera angles
        isSelfie = false;

        uiHud.ToggleMainHUD(true);
        uiHud.ToggleCameraOverlay(false);


    }

    private void FlipCamera()
    {
        Debug.Log("FlipCamera fired");

        isSelfie = !isSelfie;

        /* 
        Transform target = isSelfie ? camAnchorBack : camAnchorFront;
         cam.transform.parent = target;
         cam.transform.localPosition = Vector3.zero;
         cam.transform.localRotation = Quaternion.identity;
        */

        photoCamFront.gameObject.SetActive(!isSelfie); //if isSelfie is false, this activates, and vice versa
        photoCamSelfie.gameObject.SetActive(isSelfie); //if isSelfie is true, this activates, and vice versa
    }




    private void TakePhoto()
    {
        //TODO phototaking stuff
        string folder = System.IO.Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!System.IO.Directory.Exists(folder)) //this check is technically redundant, as the CreateDirectory function already safegaurds for this. This is just for our visibility
            System.IO.Directory.CreateDirectory(folder);

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fullPath = System.IO.Path.Combine(folder, $"{timestamp}.png");

        ScreenCapture.CaptureScreenshot(fullPath);


        Debug.Log($"Screenshot Saved:{fullPath}");
    }

    private void OrientCamera()
    {

        Vector2 input = orientAction.ReadValue<Vector2>();
        float dt = Time.unscaledDeltaTime; // (only needed really if something alters time in photo mode, maybe if we want that)

        yaw = Mathf.Clamp(yaw + input.x * orientSpeed * dt, -yawLimit, yawLimit);
        pitch = Mathf.Clamp(pitch + input.y * orientSpeed * dt, -pitchLimit, pitchLimit);

        ActivePhotoCam.transform.localRotation = Quaternion.Euler(-pitch, yaw, 0f);

        //TODO: up/down, left/right, limited axis
    }


    [SerializeField] private float zoomSpeed = 30f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 70f;
    
        private void ZoomCamera()
    {
        float z = zoomAction.ReadValue<float>();
        if (z == 0f) return;
        Debug.Log($"zoom raw: {z}");

        float fov = ActivePhotoCam.Lens.FieldOfView - z * zoomSpeed * Time.unscaledDeltaTime;
        ActivePhotoCam.Lens.FieldOfView = Mathf.Clamp(fov, minFOV, maxFOV);
        //TODO: in/out
    }

}
