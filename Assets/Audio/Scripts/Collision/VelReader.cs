using UnityEngine;

public class VelReader : MonoBehaviour
{
    private float Magnitude;
    private float relativeVelocityMagnitude;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            //Debug.LogError("No Rigidbody found on " + gameObject.name + " adding one for velocity reading.");
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        //Debug.Log("Relative velocity magnitude: " + relativeVelocityMagnitude);
    }

    private void FixedUpdate()
    {
        if (rb != null && !rb.isKinematic)
        {
            Magnitude = rb.linearVelocity.magnitude;
            //Debug.Log("Current velocity magnitude: " + Magnitude);
        }
    }
}
