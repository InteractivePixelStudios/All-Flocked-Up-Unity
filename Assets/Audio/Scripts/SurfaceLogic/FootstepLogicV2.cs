using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/**
 * FootstepLogicV2 is a new script that will eventually replace the existing footstep logic in ALL characters. 
 * It is designed to universal, will feature Character Type for pitch offsetting. 
 * This script is still in development and may not be fully functional yet - it will also be bloated with unnecessary code for right now.
 * Yes, it will be messy to manually maintain, but that is my job - IPM. If someone else joins audio I will update this accordingly.
 *
 *
 * After a lot of consideration, this system will not work with every single object - it will now become Hybrid.
 * I will keep this new system, but also implement my old SurfaceType system for objects that use the same material but are two different surfaces (ie, tree and it's leaves). 
 * This will require level editing (which I wanted to avoid), so, it will be saved for last.
*/

[System.Serializable]
public class SurfaceMaterialGroup
{
    public FootstepLogicV2.SurfaceTypes surfaceType;
    public List<Material> materials;
}

[ExecuteInEditMode]
public class FootstepLogicV2 : MonoBehaviour
{
    public enum SurfaceTypes
    {
        Default,
        Grass,
        Wood,
        Stone,
        Metal,
        Water
    }

    private enum CharacterTypes // Used for pitch offsetting and other character-specific audio adjustments. Optional.
    {
        Player,
        Cat,
        Dog,
        Bird,
        Human
    }

    [Header("Surface Material Settings")] // At this scale, a Dictionary is not needed - but should we scale this project to dozens of creatures, I will upgrade this to a Dictionary.
    [SerializeField] private List<SurfaceMaterialGroup> surfaceMaterialGroups;

    [Header("Surface Detection Settings")]
    [SerializeField] private SurfaceTypes surfaceType;
    [SerializeField] private LayerMask surfaceDetectionLayers;

    [Header("Character Settings")]
    [SerializeField] private CharacterTypes characterType;
    [SerializeField] float raycastDistance = 1.5f; // Distance for raycast - Default is 1.5, but can be adjusted for different character heights or jump arcs.
    [SerializeField] Transform surfaceDetectionOrigin;

    [Header("FMOD Events & Instances")]
    [SerializeField] EventReference footstepEvent; // Assign in Inspector

    private MeshRenderer currentSurfaceMeshRenderer; // Store the current surface's MeshRenderer for material detection.
    private Material currentSurfaceMaterial; // Store the current surface material for use.


    private void Update()
    {
        Debug.DrawLine(surfaceDetectionOrigin.position, surfaceDetectionOrigin.position + Vector3.down * raycastDistance, Color.green);

        if (!TryGetGround(out RaycastHit tempHit)) // Ripping this in Update is crazy work but I will fix later.
        {
            return;
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

    public void FootstepEvent()
    {
        currentSurfaceMaterial = null; // Reset current material before detection.
        currentSurfaceMeshRenderer = null; // Reset current MeshRenderer before detection.

        if (!TryGetGround(out RaycastHit tempHit))
            return;

        currentSurfaceMeshRenderer = tempHit.collider.GetComponent<MeshRenderer>();

        if (currentSurfaceMeshRenderer != null)
        {
            currentSurfaceMaterial = currentSurfaceMeshRenderer.sharedMaterial;
        }
        else
        {
            currentSurfaceMeshRenderer = tempHit.collider.GetComponentInChildren<MeshRenderer>();
            if (currentSurfaceMeshRenderer != null)
                currentSurfaceMaterial = currentSurfaceMeshRenderer.sharedMaterial;
            else
                Debug.LogWarning($"No MeshRenderer found on {tempHit.collider.name} or its children. Surface type will default.");
        }

        GetSurfaceType();

        Debug.Log($"Detected surface: {tempHit.collider.name} with material: {currentSurfaceMaterial?.name ?? "None"}");


        EventInstance footstepInstance = RuntimeManager.CreateInstance(footstepEvent);
        footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        footstepInstance.setParameterByName("Surface", (float)surfaceType);

        footstepInstance.start();
        footstepInstance.release();
    }

    private void GetSurfaceType()
    {
        foreach (var group in surfaceMaterialGroups)
        {
            if (group.materials.Contains(currentSurfaceMaterial))
            {
                surfaceType = group.surfaceType;
                return;
            }
        }

        surfaceType = SurfaceTypes.Default; // Default
    }

    private void KillAudioEarly(EventInstance instance) // May not be needed with this new system but keeping just in case - IPM.
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}
