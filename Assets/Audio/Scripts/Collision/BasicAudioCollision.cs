/* Script managed by IPM */
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

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
    [SerializeField] private float cooldown = 0.1f;

    // Gating the sounds with a timer for now - Ideally, I would like to track if a sound is playing, but FMOD is not easy.
    private bool canPlaySound = true;

    // Minimum velocity required to trigger a collision sound, this is used to prevent sounds from playing for very minor collisions, not used in current testing.
    private float minVel = 1f;
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

        // Get the magnitude of the collision impact, this is used to determine the intensity of the collision sound, not used in current testing.
        impact = collision.relativeVelocity.magnitude / 2;
        //Debug.LogWarning("Collision detected with impact magnitude: " + impact);

        // Play the collision sound at the point of impact, with intensity based on the impact magnitude, this is used for more advanced sound control, not used in current testing
        PlayCollisionSound();

        // Below is testing ONLY, remove for production.
        //Vector3 forceDirection = collision.contacts[0].normal;
        //rb.AddForce(forceDirection * 10, ForceMode.Impulse); // DONT USE IMPACT - Endless growth of force.
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
        if (!canPlaySound || impact < minVel) return; // Check if we can play a sound and if the impact is above the minimum threshold

        canPlaySound = false; // Set canPlaySound to false to prevent spamming
        collisionSoundInstance = RuntimeManager.CreateInstance(collisionSound);
        collisionSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(lastHitPoint));
        collisionSoundInstance.setParameterByName("Impact", impact); // Set the impact parameter to control the intensity of the sound - could double it honestly, further testing needed.
        collisionSoundInstance.start();
        StartCoroutine(CollisionSoundCooldown()); // Start the cooldown coroutine to reset canPlaySound after a short delay
    }

    private IEnumerator CollisionSoundCooldown() // This is for gating the sounds with a timer, not used in current testing.
    {
        yield return new WaitForSeconds(cooldown);
        canPlaySound = true; // Reset canPlaySound to true after the cooldown period
    }
}

