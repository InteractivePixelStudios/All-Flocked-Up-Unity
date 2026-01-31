using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

//[ExecuteInEditMode]
public class Audio_Player : MonoBehaviour // REMINDER - Clean this damn script up later - IPM.
{
    [Header("FMOD Events")]
    [SerializeField] EventReference footstepEvent; // Assign in Inspector
    [SerializeField] EventReference wingFlapEvent; // Assign in Inspector
    [SerializeField] EventReference poopEvent; // Assign in Inspector
    [SerializeField] EventReference splatEvent; // Assign in Inspector

    [Header("FMOD Instances")] //Used to control audio playback and early cancellation
    private EventInstance footstepInstance;
    private EventInstance wingFlapInstance;
    private EventInstance poopInstance;
    private EventInstance splatInstance;

    [SerializeField] LayerMask surfaceDetectionLayers; // Layers to detect surfaces on
    [SerializeField] float raycastDistance = 1.5f; // Distance for raycast
    [SerializeField] Transform surfaceDetectionOrigin;
    private bool canPlay;
    private AudioWizard audioWizard;

    public void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
    }

    private void Update()
    {
        Debug.DrawLine(surfaceDetectionOrigin.position, surfaceDetectionOrigin.position + Vector3.down * raycastDistance, Color.green);

        if (!TryGetGround(out RaycastHit tempHit))
        {
            Debug.Log("No ground detected.");
            KillAudioEarly(footstepInstance);
            return;
        }
        else
        {
            Debug.Log("Ground detected: " + tempHit.collider.name);
        }
    }

    private void KillAudioEarly(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    private bool TryGetGround(out RaycastHit hit)
    {
        Vector3 origin = surfaceDetectionOrigin.position;
        return Physics.Raycast(
            origin,
            Vector3.down,
            out hit,
            raycastDistance,
            ~0, // EVERYTHING
            QueryTriggerInteraction.Ignore
        );
    }

    public void PlayerFootstep()
    {
        Debug.Log("PlayerFootstep PRE-CALLED");

        if (!TryGetGround(out RaycastHit tempHit))
            return;

        Debug.Log("PlayerFootstep CALLED");

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

        EventInstance footstepInstance = RuntimeManager.CreateInstance(footstepEvent);
        footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        footstepInstance.setParameterByName("Surface", (float)surfaceType);

        footstepInstance.start();
        footstepInstance.release();
    }

    public void WingFlap()
    {
        EventInstance wingFlapInstance = RuntimeManager.CreateInstance(wingFlapEvent);
        //wingFlapInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        wingFlapInstance.start();
        wingFlapInstance.release();
    }

    public void Poop()
    {
        EventInstance poopInstance = RuntimeManager.CreateInstance(poopEvent);
        //poopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        poopInstance.start();
        poopInstance.release();
    }

    public void Splat()
    {
        EventInstance splatInstance = RuntimeManager.CreateInstance(splatEvent);
        //splatInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        splatInstance.start();
        splatInstance.release();
    }
}
