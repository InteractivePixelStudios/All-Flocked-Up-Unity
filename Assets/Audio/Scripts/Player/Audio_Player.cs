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
        }
        else
        {
            Debug.Log("Ground detected: " + tempHit.collider.name);
        }
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

        EventInstance inst = RuntimeManager.CreateInstance(footstepEvent);
        inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        inst.setParameterByName("Surface", (float)surfaceType);

        inst.start();
        inst.release();
    }

    public void WingFlap()
    {
        EventInstance inst = RuntimeManager.CreateInstance(wingFlapEvent);
        //inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        inst.start();
        inst.release();
    }

    public void Poop()
    {
        EventInstance inst = RuntimeManager.CreateInstance(poopEvent);
        //inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        inst.start();
        inst.release();
    }

    public void Splat()
    {
        EventInstance inst = RuntimeManager.CreateInstance(wingFlapEvent);
        //inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        inst.start();
        inst.release();
    }
}
