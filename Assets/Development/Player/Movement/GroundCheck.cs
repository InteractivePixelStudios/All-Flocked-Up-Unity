using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float checkDistance = 0.2f;
    [SerializeField] LayerMask groundMask;

    public bool IsGrounded()
    {
        // create a sphere and check if the player is on the ground, if player is on ground return true
        if (Physics.CheckSphere(transform.position, checkDistance, groundMask))
        {
            return true;
        }
        return false;
    }
}
