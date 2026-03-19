using Steamworks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PoopDecalProjector : MonoBehaviour
{
    [SerializeField] private DecalProjector projector;
    [SerializeField] private PlayerGroundMovement moveComp;
    private bool isFlying => moveComp.GetIsFlying();

    private void Start()
    {
        moveComp = GetComponent<PlayerGroundMovement>();
        projector = GetComponentInChildren<DecalProjector>();
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

        }
        else return;
    }
}
