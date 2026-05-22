using UnityEngine;

public class WindZoneForce : MonoBehaviour
{
    [SerializeField] float pushForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.GetComponent<Rigidbody>();
            if (pushForce <= 0) pushForce = 100f;
            rb.AddForce(new Vector3(0,0, pushForce), ForceMode.Impulse);
        }
    }
}
