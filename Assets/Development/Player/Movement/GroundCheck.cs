using UnityEngine;
using UnityEngine.AI;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float groundCheckDistance = 0.15f;
    [SerializeField] float airCheckDistance = 0.25f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask propMask;
    [SerializeField] LayerMask vehicleMask;
    [SerializeField] private bool grounded;
    [SerializeField] private bool onVehicle;
    [SerializeField] private NavMeshAgent currentVehicle;

    public bool GetIsOnVehicle()
    {
        return onVehicle;
    }

    public NavMeshAgent GetCurrentVehicle()
    {
        return currentVehicle;
    }

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
