using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float groundCheckDistance = 0.15f;
    [SerializeField] float airCheckDistance = 0.25f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask propMask;

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
        if (Physics.CheckSphere(transform.position, radius, groundMask))
        {
            return true;
        }
        else if (Physics.CheckSphere(transform.position, radius, propMask))
        {
            return true;
        }
        return false;
    }
}
