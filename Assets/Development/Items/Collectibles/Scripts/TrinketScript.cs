using UnityEngine;

public class TrinketScript : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private int value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            CollectTrinket();
        }
    }

    private void CollectTrinket()
    {
        //Add when trinket & inv complete
    }
}
