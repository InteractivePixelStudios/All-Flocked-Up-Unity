using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.InteropServices;
using System;
using Unity.VisualScripting;
using UnityEngine.Assemblies;

/**
---Notes---

 * FootstepLogicV2 is a new script that will eventually replace the existing footstep logic in ALL characters. 
 * It is designed to universal, will feature Character Type for pitch offsetting. 
 * This script is still in development and may not be fully functional yet - it will also be bloated with unnecessary code for right now.
 * Yes, it will be messy to manually maintain, but that is my job - IPM. If someone else joins audio I will update this accordingly.
 *
 *
 * After a lot of consideration, this system will not work with every single object - it will now become Hybrid.
 * I will keep this new system, but also implement my old SurfaceType system for objects that use the same material but are two different surfaces (ie, tree and it's leaves). 
 * This will require level editing (which I wanted to avoid), so, it will be saved for last - THIS IS NOT LONGER TRUE.

 - Add logic to stop footstep sounds if the material changes mid-step - IPM
 -
 -
 -

*/

[ExecuteInEditMode]
public class FootstepLogicV2 : MonoBehaviour
{
    private enum CharacterTypes // Used for pitch offsetting and other character-specific audio adjustments. Optional.
    {
        Player,
        Cat,
        Dog,
        Bird,
        Human
    }

    private enum SurfaceSourceTypes
    {
        Default, // Default for when no surface is detected.
        SurfaceIdentifier,
        MaterialGroups,
        TerrainData
    }

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
    private RaycastHit rayHit; // Reusable ray for detection.

    [Header("Script References")]
    private AudioWizard audioWizard;
    private SurfaceType surfaceTypeScript;

    [Header("Surface Source Settings")]
    [SerializeField] private SurfaceSourceTypes surfaceSourceType; // One time set when we are on a surface.
    [SerializeField] private SurfaceSourceTypes currentSurfaceSourceType; // The current source we're on - checked every update.

    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
        if (audioWizard != null)
            surfaceTypeScript = audioWizard.gameObject.GetComponent<SurfaceType>(); // Get reference to SurfaceType script for material groups.
    }

    private void Update()
    {
        Debug.DrawLine(surfaceDetectionOrigin.position, surfaceDetectionOrigin.position + Vector3.down * raycastDistance, Color.green);

        if (!TryGetGround(out RaycastHit tempHit))
            return;

        rayHit = tempHit; // Store the hit for use in footstep event.
        TryResolveSurfaceSource(tempHit);

        if (surfaceSourceType != currentSurfaceSourceType) // this is to stop spamming the resolver every frame - it only needs to be called when we hit a new surface.
            currentSurfaceSourceType = surfaceSourceType;
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

    public void FootstepEvent() // This will now be turned into the master method which only handles which detection method to use.
    {
        switch (currentSurfaceSourceType)
        {
            case SurfaceSourceTypes.SurfaceIdentifier:
                GetSurfaceTypeUsingSurfaceIdentifier(rayHit); // all these checks could be moved to the detection layer.
                break;
            case SurfaceSourceTypes.MaterialGroups:
                GetSurfaceTypeUsingMaterialGroups(rayHit);
                break;
            case SurfaceSourceTypes.TerrainData:
                GetSurfaceTypeUsingTerrainIndex(rayHit);
                break;
            default:
                surfaceType = SurfaceTypes.Default;
                break;
        }

        //PlayFootstepSound((int)surfaceType); // Play the footstep sound with the correct surface type parameter.
    }

    private void GetSurfaceTypeUsingMaterialGroups(RaycastHit hit)
    {
        currentSurfaceMeshRenderer = hit.collider.GetComponent<MeshRenderer>();
        currentSurfaceMaterial = currentSurfaceMeshRenderer.sharedMaterial; // Safety is not needed, can't get to this point without a MeshRenderer.

        foreach (var group in surfaceTypeScript.surfaceMaterialGroups)
        {
            if (group.materials.Contains(currentSurfaceMaterial))
            {
                surfaceType = group.surfaceType;
                PlayFootstepSound((int)surfaceType); // Play the footstep sound with the correct surface type parameter. 
                return;
            }
            // Before I played the default sound if nothing was found, but now we're not going to play anything.
        }
    }

    private void GetSurfaceTypeUsingTerrainIndex(RaycastHit hit) // this entire method is dog shit 
    {
        Vector3 worldPosition = hit.point;
        int terrainIndex = GetDominantTerrainLayerIndex(worldPosition);

        switch (terrainIndex) // Self note - might be better to make this a dictionary 
        {
            case 0:
            case 1:
                surfaceType = SurfaceTypes.Grass;
                break;
            case 2:
                surfaceType = SurfaceTypes.Concrete;
                break;
            case 3:
                surfaceType = SurfaceTypes.Wood;
                break;
            default:
                surfaceType = SurfaceTypes.Default;
                break;
        }

        PlayFootstepSound((int)surfaceType);
    }

    private void GetSurfaceTypeUsingSurfaceIdentifier(RaycastHit hit)
    {
        currentSurfaceMeshRenderer = hit.collider.GetComponent<MeshRenderer>();
        surfaceType = hit.collider.GetComponent<SurfaceIdentifier>().SurfaceType;
        PlayFootstepSound((int)surfaceType); // Play the footstep sound with the correct surface type parameter.
    }

    private void PlayFootstepSound(int index) // The last step.
    {
        EventInstance footstepInstance = RuntimeManager.CreateInstance(footstepEvent);
        footstepInstance.setParameterByName("Surface", index);
        footstepInstance.start();
        footstepInstance.release(); // Release immediately - could make this a one shot really.
    }

    // WORKING
    private void TryResolveSurfaceSource(RaycastHit hit) // Meh, it works, but I may make a dedicated Resolver at some point.
    {
        if (hit.collider.TryGetComponent(out SurfaceIdentifier surfaceIdentifier)) // Check for SurfaceIdentifier first before anything.
        {
            surfaceSourceType = SurfaceSourceTypes.SurfaceIdentifier;
            return;
        }

        MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>() ?? hit.collider.GetComponentInParent<MeshRenderer>() ?? hit.collider.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            surfaceSourceType = SurfaceSourceTypes.MaterialGroups;
            return;
        }

        if (hit.collider.TryGetComponent(out Terrain terrain))
        {
            surfaceSourceType = SurfaceSourceTypes.TerrainData;
            return;
        }

        surfaceSourceType = SurfaceSourceTypes.Default; // Default if no source is found.
    }

    public int GetDominantTerrainLayerIndex(Vector3 worldPosition)
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
            return -1;

        TerrainData terrainData = terrain.terrainData;
        Vector3 localPos = worldPosition - terrain.transform.position;

        int x = Mathf.FloorToInt((localPos.x / terrainData.size.x) * terrainData.alphamapWidth);
        int z = Mathf.FloorToInt((localPos.z / terrainData.size.z) * terrainData.alphamapHeight);

        x = Mathf.Clamp(x, 0, terrainData.alphamapWidth - 1);
        z = Mathf.Clamp(z, 0, terrainData.alphamapHeight - 1);

        float[,,] alphaMap = terrainData.GetAlphamaps(x, z, 1, 1);

        int dominantIndex = 0;
        float strongestWeight = alphaMap[0, 0, 0];

        for (int i = 1; i < terrainData.alphamapLayers; i++)
        {
            if (alphaMap[0, 0, i] > strongestWeight)
            {
                strongestWeight = alphaMap[0, 0, i];
                dominantIndex = i;
            }
        }

        return dominantIndex;
    }

    private void KillAudioEarly(EventInstance instance) // May not be needed with this new system but keeping just in case - IPM.
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}
