using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFinder : Singleton<PlayerFinder>
{
    private GameObject player;
    private CinemachineCamera camRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        camRef = GetComponentInChildren<CinemachineCamera>();
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            SetTrackingTarget();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(player != null && camRef.Target.TrackingTarget ==null)
        //{
        //    player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        //}
    }

    void SetTrackingTarget()
    {

      camRef.Target.TrackingTarget = player.gameObject.transform;

    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    this.enabled = true;
    //    SetTrackingTarget();
    //}
}
