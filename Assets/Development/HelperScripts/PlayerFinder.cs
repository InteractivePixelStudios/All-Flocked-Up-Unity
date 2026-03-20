using Unity.Cinemachine;
using UnityEngine;

public class PlayerFinder : Singleton<PlayerFinder>
{
    private GameObject player;
    private CinemachineCamera camRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        camRef = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTrackingTarget()
    {

      camRef.Target.TrackingTarget = player.gameObject.transform;

    }

    private void OnLevelWasLoaded(int level)
    {
        this.enabled = true;
        SetTrackingTarget();
    }
}
