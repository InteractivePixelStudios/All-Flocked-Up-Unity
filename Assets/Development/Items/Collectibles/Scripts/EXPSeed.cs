using UnityEngine;

public class EXPSeed : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private int value;
    [SerializeField] private EXPSystem playerXP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            playerXP = other.GetComponent<EXPSystem>();
            EXPConsume();
            Destroy(gameObject);

        }
    }

    private void EXPConsume()
    {
        playerXP.IncrementXP(value);
    }
}
