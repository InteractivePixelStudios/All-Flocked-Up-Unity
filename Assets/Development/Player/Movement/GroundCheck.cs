using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float checkDistance = 0.2f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask propMask;

    public bool IsGrounded()
    {

        // create a sphere and check if the player is on the ground, if player is on ground return true
        if (Physics.CheckSphere(transform.position, checkDistance, groundMask))
        {
            return true;
        }else if (Physics.CheckSphere(transform.position, checkDistance, propMask))
        {
            return true;
        }
        return false;
    }
}
