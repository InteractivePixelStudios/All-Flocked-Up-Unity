using UnityEngine;

public class KickComponent : MonoBehaviour
{
    public int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
            
        }
    }
}
