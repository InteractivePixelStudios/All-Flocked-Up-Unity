using UnityEngine;

public class MapCameraFollow : MonoBehaviour
{
    [SerializeField] private Camera mapCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset = new Vector3(0,50,0);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
        mapCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        if (mapCamera != null)
        {
            mapCamera.transform.position = player.transform.position+offset;

        }
    }


}
