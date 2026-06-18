using UnityEngine;

public class PerchableObject_General : MonoBehaviour, I_Perchable
{
    public GameObject playerRef;
    [SerializeField] private bool isPerching;
    [SerializeField] private GameObject perchPoint;
    [SerializeField] private GameObject placementMesh;
    bool jumpCheck => playerRef.GetComponent<PlayerGroundMovement>().GetIsFlying();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placementMesh.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPerching)
        {
            if (jumpCheck)
            {
                StopPerch();
                playerRef.GetComponent<Rigidbody>().linearVelocity = new Vector3(1, 1, 0);
            }
            UpdatePerch();
        }
        else return;
    }

    public void SetPlayerRef(GameObject player)
    {
        playerRef = player;
    }

    public void StartPerch()
    {
        isPerching = true;
        playerRef.transform.position = perchPoint.transform.position;
        playerRef.GetComponentInChildren<IconToggle>().HideIcon();
    }

    public void StopPerch()
    {
        isPerching = false;
    }

    public void UpdatePerch()
    {
        playerRef.transform.position = perchPoint.transform.position;
    }

    public void MovePosition(float x)
    {
        //not needed for General Perching... maybe use later?
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(playerRef == null)
            {
                playerRef = other.gameObject;
                StartPerch();
            }
            playerRef.GetComponentInChildren<IconToggle>().ShowIcon();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerRef != null)
            {
                playerRef = null;
            }
            playerRef.GetComponentInChildren<IconToggle>().HideIcon();
        }
    }
}
