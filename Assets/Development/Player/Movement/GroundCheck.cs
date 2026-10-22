using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float groundCheckDistance = 0.15f;
    [SerializeField] float airCheckDistance = 0.25f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask propMask;
    [SerializeField] private bool grounded;

    public bool IsGrounded(bool groundMovement)
    {
        if (groundMovement)
            return SphereCheck(groundCheckDistance);
        else
            return SphereCheck(airCheckDistance);
    }

    private bool SphereCheck(float radius)
    {
        // create a sphere and check if the player is on the ground, if player is on ground return true
        if (Physics.CheckSphere(transform.position, radius, groundMask, QueryTriggerInteraction.Ignore))
        {
            grounded = true;
            return grounded;
        }
        else if (Physics.CheckSphere(transform.position, radius, propMask, QueryTriggerInteraction.Ignore))
        {
            grounded = true;
            return grounded;
        }
        grounded = false;
        return grounded;
    }
}
