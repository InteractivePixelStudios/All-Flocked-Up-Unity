using Steamworks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PoopDecalProjector : MonoBehaviour
{
    [SerializeField] private DecalProjector projector;
    [SerializeField] private PlayerGroundMovement moveComp;
    [SerializeField] private LayerMask groundMask;
    Rigidbody player;
    private bool isFlying => moveComp.GetIsFlying();

    private void Start()
    {
        moveComp = GetComponent<PlayerGroundMovement>();
        projector = GetComponentInChildren<DecalProjector>();
        player = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isFlying)
        {
            projector.enabled = false;
        }
        else if (isFlying)
        {
            projector.enabled = true;

            // Trigonometry to get the X angle of tilt
            Vector3 calcDistance = new Vector3(player.linearVelocity.x, 0f, player.linearVelocity.z) * Mathf.Sqrt((GetHeight() * 2) / Mathf.Abs(Physics.gravity.y));
            float distance = calcDistance.magnitude;
            float angle = Mathf.Atan2(distance, GetHeight());

            projector.transform.eulerAngles = new Vector3(90 - (Mathf.Rad2Deg * angle), projector.transform.eulerAngles.y, projector.transform.eulerAngles.z);
        }
        else return;
    }

    float GetHeight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10000f, groundMask))
        {
            return hit.distance;
        }

        return transform.position.y;
    }
}
