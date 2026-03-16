using UnityEngine;
using UnityEngine.InputSystem; // only needed temporarily
//using Cinemachine; 


public class ScreenshotCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private PlayerStateController psc;
    [SerializeField] private Transform camAnchor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider cameraCollider;

    void Start()
    {
        cameraCollider = cam.GetComponent<Collider>();
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
       { Debug.Log("camera collider disabled"); cameraCollider.enabled = false;}
       //move cam to anchor
       cam.transform.parent = camAnchor;
       cam.transform.localPosition = Vector3.zero;
       cam.transform.localRotation = Quaternion.identity;
    }

    private void EndPhotoMode()
    {
        psc.ExitPhotoMode();
        Debug.Log("Photo Mode Exited");
        
        { Debug.Log("camera collider enabled"); cameraCollider.enabled = true;}

        //reset to normal camera control
        cam.transform.parent = null;
    }
    
    
}
