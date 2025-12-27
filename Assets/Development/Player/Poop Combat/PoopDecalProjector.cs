using Steamworks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PoopDecalProjector : MonoBehaviour
{
    [SerializeField] private DecalProjector projector;
    [SerializeField] private PlayerGroundMovement moveComp;
    [SerializeField] private Vector3 decalLoc;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private bool isFlying => moveComp.GetIsFlying();
    [SerializeField] private float altitude;

    private void Start()
    {
        moveComp = GetComponent<PlayerGroundMovement>();
        projector = GetComponentInChildren<DecalProjector>();
        groundCheck = GetComponentInChildren<GroundCheck>().gameObject;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.transform.position, Vector3.down,out hit,LayerMask.NameToLayer("Ground")))
        {
            altitude = hit.distance;
            Debug.Log(altitude);
            decalLoc = hit.transform.position;
            projector.transform.position = decalLoc;
            
        }
        if (!isFlying)
        {
            projector.enabled = false;
        }
        else if (isFlying)
        {
            projector.enabled = true;

        }
        else return;
    }
}
