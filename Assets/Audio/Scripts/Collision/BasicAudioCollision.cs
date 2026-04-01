/* Script managed by IPM */
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/* 
---Notes---
- It is not an issue, YET, but I may need to add an FMOD wait state to prevent sounds from spamming - I know, very technical.
-
-
-
-
-
*/

public class BasicAudioCollision : MonoBehaviour
{
    public enum ObjectType // later use.
    {
        Default, // Thunk    
        Glass,
        Trash,
        Ceramic,
        Metal,
        Wood,
        Food
    }

    [SerializeField] private EventReference collisionSound; // Using one sound for all right now, it is temp.
    private EventInstance collisionSoundInstance; // For more advanced sound control, not used in current testing.
    [SerializeField] private ObjectType objectType;
    Vector3 lastHitPoint;
    bool hasHit;
    private Rigidbody rb;
    private float lastMagnitude; // The magnitude of the last velocity, this is used to determine the intensity of the collision sound, not used in current testing.
    private float impact;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name + " adding one for collision detection.");
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Grab the first contact point
        lastHitPoint = collision.contacts[0].point;
        hasHit = true;

        impact = collision.relativeVelocity.magnitude; // Get the magnitude of the collision impact, this is used to determine the intensity of the collision sound, not used in current testing.
        PlayCollisionSound(); // Play the collision sound at the point of impact, with intensity based on the impact magnitude, this is used for more advanced sound control, not used in current testing

        // Below is testing ONLY, remove for production.
        Vector3 forceDirection = collision.contacts[0].normal;
        rb.AddForce(forceDirection * 10, ForceMode.Impulse); // DONT USE IMPACT - Endless growth of force.
    }

    private void FixedUpdate()
    {
        if (rb != null && !rb.isKinematic && rb.linearVelocity.magnitude > 0.1f) // Safety checks, some not needed, but just in case.
        {
            lastMagnitude = rb.linearVelocity.magnitude; // Update last velocity magnitude each physics frame.
        }
    }

    void OnDrawGizmos()
    {
        if (!hasHit) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastHitPoint, 0.1f);
    }

    private void PlayOneShot() // This was for testing, EventInstance will now be used for actual production.
    {
        RuntimeManager.PlayOneShot(collisionSound, lastHitPoint);
    }

    private void PlayCollisionSound() // This is for more advanced sound control, not used in current testing.
    {
        collisionSoundInstance = RuntimeManager.CreateInstance(collisionSound);
        collisionSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(lastHitPoint));
        collisionSoundInstance.setParameterByName("Impact", impact); // Set the impact parameter to control the intensity of the sound - could double it honestly, further testing needed.
        collisionSoundInstance.start();
    }
}

