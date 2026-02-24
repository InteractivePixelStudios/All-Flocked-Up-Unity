using UnityEngine;

public class ScreenshotCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private PlayerStateController psc;
    [SerializeField] private Transform camAnchor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //temporary for now 
        {
            if (psc.CurrentState == PlayerState.PhotoMode)
                EndPhotoMode();
            else
                StartPhotoMode();
        }
       
    }

    void StartPhotoMode()
    {
       psc.EnterPhotoMode(); 
    }

    void EndPhotoMode()
    {
        psc.ExitPhotoMode();
    }
    
    
}
