using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Audio_PlayerFootstepSurfaceDetection : MonoBehaviour // REMINDER - Change the name of this script, it is too long.
{
    [SerializeField] EventReference footstepEvent; // Assign in Inspector
    [SerializeField] LayerMask surfaceDetectionLayers; // Layers to detect surfaces on
    [SerializeField] float raycastDistance = 1.5f; // Distance for raycast
    [SerializeField] Transform surfaceDetectionOrigin;
    private AudioWizard audioWizard;

    public void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
    }

    public void PlayerFootstep()
    {
        FootstepSurface surfaceType = FootstepSurface.Default;

        RaycastHit hit;
        if (Physics.Raycast(surfaceDetectionOrigin.position, Vector3.down, out hit, raycastDistance, surfaceDetectionLayers))
        {
            SurfaceType surfaceComponent = hit.collider.GetComponent<SurfaceType>();
            if (surfaceComponent != null)
            {
                surfaceType = surfaceComponent.surface;
                //Debug.Log("Detected surface type: " + surfaceType.ToString());
            }
        }

        EventInstance inst = RuntimeManager.CreateInstance(footstepEvent);
        inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        inst.setParameterByName("Surface", (float)surfaceType);

        inst.start();
        inst.release();
    }
}
