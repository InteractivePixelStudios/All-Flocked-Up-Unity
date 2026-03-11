using UnityEngine;

public class KickComponent : MonoBehaviour
{
    public int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            var player = collider.gameObject.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
            Debug.Log("PlayerHit");
        }
    }
}
