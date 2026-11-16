using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFinder : Singleton<PlayerFinder>
{
    private GameObject player;
    private CinemachineCamera camRef;

    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        camRef = GetComponentInChildren<CinemachineCamera>();
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            SetTrackingTarget();
        }       
    }

    void SetTrackingTarget()
    {
      camRef.Target.TrackingTarget = player.gameObject.transform;
    }

}
