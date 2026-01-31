using UnityEngine;

public class BasicAudioCollision : MonoBehaviour
{
    Vector3 lastHitPoint;
    bool hasHit;

    void OnCollisionEnter(Collision collision)
    {
        // Grab the first contact point
        lastHitPoint = collision.contacts[0].point;
        hasHit = true;
    }

    void OnDrawGizmos()
    {
        if (!hasHit) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastHitPoint, 0.1f);
    }
}

