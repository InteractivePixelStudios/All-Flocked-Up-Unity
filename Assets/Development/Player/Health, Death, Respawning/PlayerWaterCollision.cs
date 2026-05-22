using UnityEngine;

public class PlayerWaterCollision : MonoBehaviour
{
    [SerializeField] GameObject respawnPoint;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = respawnPoint.transform.position;
        }
    }
}
