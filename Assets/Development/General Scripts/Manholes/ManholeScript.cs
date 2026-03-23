using UnityEngine;

public class ManholeScript : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private float airVerticalForce = 10f;
    [SerializeField] private bool resetBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void AddVerticalForce()
    {
        if(playerRef != null)
        {
            playerRef.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,airVerticalForce);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            AddVerticalForce();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = null;
        }
    }
}
