using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    [SerializeField] int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
        }
    }
}
