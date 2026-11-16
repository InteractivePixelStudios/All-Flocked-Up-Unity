using UnityEngine;

public class WindZoneForce : MonoBehaviour
{
    [SerializeField] float pushForce;

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
