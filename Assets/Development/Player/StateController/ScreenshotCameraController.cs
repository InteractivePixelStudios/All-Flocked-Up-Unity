using UnityEngine;
using UnityEngine.InputSystem; // only needed temporarily
using Unity.Cinemachine;


public class ScreenshotCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private PlayerStateController psc;
    [SerializeField] private Transform camAnchor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider cameraCollider;
    private CinemachineBrain cinemachineBrain;
    void Start()
    {
        cameraCollider = cam.GetComponent<Collider>();
        cinemachineBrain = cam.GetComponent<CinemachineBrain>();

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
       
    }

    private void StartPhotoMode()
    {
       psc.EnterPhotoMode(); 
       Debug.Log("Photo Mode Entered");

       if (cameraCollider)
       { Debug.Log("Main Camera Collider Component disabled by StartPhotoMode"); cameraCollider.enabled = false;}
       if (cinemachineBrain)
       { Debug.Log("Main Camera CinemachineBrain Component disabled by StartPhotoMode"); cinemachineBrain.enabled = false;}

       //move cam to anchor
       cam.transform.parent = camAnchor;
       cam.transform.localPosition = Vector3.zero;
       cam.transform.localRotation = Quaternion.identity;
    }

    private void EndPhotoMode()
    {
        psc.ExitPhotoMode();
        Debug.Log("Photo Mode Exited");
        
        { Debug.Log("Main Camera Collider Component enabled by StartPhotoMode"); cameraCollider.enabled = true;}
        { Debug.Log("Main Camera CinemachineBrain Component enabled by StartPhotoMode"); cinemachineBrain.enabled = true;}

        //reset to normal camera control
        cam.transform.parent = null;
    }
    
    
}
