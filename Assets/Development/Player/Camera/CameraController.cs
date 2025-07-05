using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;           // Assign in Inspector or via script
    public Transform respawnTarget;    // Assign the object to watch after death
    public Vector3 respawnOffset = new Vector3(0, 20, 0); // Height above target
    public float transitionSpeed = 2f;

    private bool watchPlayer = true;

    void LateUpdate()
    {
        if (watchPlayer && player != null)
        {
            // Follow player
            transform.position = player.position + new Vector3(0, 5, -10);
            transform.LookAt(player);
        }
        else if (respawnTarget != null)
        {
            // Move to top-down view
            Vector3 targetPos = respawnTarget.position + respawnOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * transitionSpeed);
        }
    }

    public void SwitchToRespawnCam()
    {
        watchPlayer = false;
    }

    public void SwitchToPlayer()
    {
        watchPlayer = true;
    }
}
