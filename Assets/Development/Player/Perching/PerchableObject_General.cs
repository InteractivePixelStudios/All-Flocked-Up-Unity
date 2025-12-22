using UnityEngine;

public class PerchableObject_General : MonoBehaviour, I_Perchable
{
    public GameObject playerRef;
    [SerializeField] private bool isPerching;
    [SerializeField] private GameObject perchPoint;
    [SerializeField] private GameObject placementMesh;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopPerch();
                playerRef.GetComponent<Rigidbody>().linearVelocity = new Vector3(1, 1, 0);
            }
            UpdatePerch();
        }
        else return;
    }

    public void StartPerch()
    {
        isPerching = true;
        playerRef.transform.position = perchPoint.transform.position;
    }

    public void StopPerch()
    {
        isPerching = false;
    }

    public void UpdatePerch()
    {
        playerRef.transform.position = perchPoint.transform.position;
    }

    public void MovePosition()
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
        }
    }
}
