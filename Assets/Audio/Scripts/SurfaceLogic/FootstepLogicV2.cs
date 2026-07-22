using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/**
--- FootstepLogicV2 ---
(Universal Footstep Logic)

FootstepLogicV2 is the main footstep detection and playback system, replacing the older logic across all characters.

This system supports multiple surface resolution paths:
- SurfaceIdentifier
- Material group lookup
- Terrain layer lookup

It uses cached ground detection and resolves the active surface source before playback, avoiding unnecessary checks during footstep events.

This is the current hybrid footstep system for the project. SurfaceIdentifier is still required for edge cases where multiple objects share the same material but should produce different sounds.
- Example: Trees have the same material for leaves and trunk. SurfaceIdentifier allows them to be differentiated without needing unique materials.  

--- Current Status ---
- Core surface detection is implemented and working
- Material, SurfaceIdentifier, and Terrain support are all in place
- Stable for current project use

--- Remaining Work ---
- Add character-specific footstep handling
- Support alternate behaviour by character type:
  - Cats: lower volume, more muffled playback
  - Humans: heavier thump, separate event set
  - Other character types as needed
- Expand surface mappings so more objects resolve correctly
- Expand materual list on Audio Wizard for all used materials.
- Add SurfaceMemory feature - where if the new surface is the same as the last, skip identifying a new source.
*/

[ExecuteInEditMode]
public class FootstepLogicV2 : MonoBehaviour
{
    private enum CharacterTypes // Used for character-specific audio adjustments, such as pitch offsets
    {
        Player,
        Cat,
        Dog,
        Bird,
        Human
    }

    private enum SurfaceSourceTypes
    {
        Default, // No valid surface source was found
        SurfaceIdentifier, // Uses SurfaceIdentifier on the hit object
        MaterialGroups, // Uses MeshRenderer material lookup
        TerrainData // Uses terrain splatmap dominant layer lookup
    }

    [Header("Surface Detection")]
    [SerializeField] private SurfaceTypes surfaceType;
    public SurfaceTypes CurrentSurfaceType => surfaceType; // Exposes the current surface type for external use
    [SerializeField] private LayerMask surfaceDetectionLayers;
    [SerializeField] private float raycastDistance = 1.5f; // Ground detection ray distance, adjustable per character height
    [SerializeField] private Transform surfaceDetectionOrigin;

    [Header("Character Settings")]
    [SerializeField] private CharacterTypes characterType;

    [Header("FMOD")]
    [SerializeField] private EventReference footstepEvent; // FMOD event used for footstep playback

    [Header("Script References")]
    private AudioWizard audioWizard;
    private SurfaceType surfaceTypeScript; // Reference to the script containing material group definitions

    [Header("Cached Surface State")]
    private RaycastHit rayHit; // Cached ground hit reused during footstep playback
    private Object currentSurfaceObject; // Current detected surface object
    private Object lastSurfaceObject; // Previous detected surface object

    private MeshRenderer currentMeshRenderer; // Cached renderer used during surface resolution
    private Material currentSurfaceMaterial; // Cached material used for material group lookup
    private SurfaceIdentifier currentSurfaceIdentifier; // Cached SurfaceIdentifier when available
    private Terrain currentTerrain; // Cached Terrain when available
    public bool Use3DSound = false; // Option to toggle 3D sound settings on footstep events, can be set per character

    private bool canGetNewSurfaceInfo = true; // Prevents stale surface data from being refreshed at the wrong time

    [Header("Surface Source State")]
    [SerializeField] private SurfaceSourceTypes surfaceSourceType; // Stored source type

    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Get reference to the main audio system
        if (audioWizard != null)
            surfaceTypeScript = audioWizard.gameObject.GetComponent<SurfaceType>(); // Access material group data from AudioWizard
        if (surfaceTypeScript == null)
            Debug.LogWarning("FootstepLogicV2: No SurfaceType script found on AudioWizard. Material group resolution will not work.");
    }

    private void Update()
    {
        //Debug.DrawLine(surfaceDetectionOrigin.position, surfaceDetectionOrigin.position + Vector3.down * raycastDistance, Color.green); // Visualize the ground check ray

        if (!TryGetGround(out RaycastHit tempHit))
            return;

        currentSurfaceObject = tempHit.collider.gameObject; // Get the current surface object from the raycast hit

        if (currentSurfaceObject != lastSurfaceObject) // Only resolve surface source if we've hit a new object to prevent stale data issues
        {
            //Debug.Log("New surface detected: " + currentSurfaceObject.name + ". Resolving surface source...");
            TryResolveSurfaceSource(tempHit); // Determine the surface source type for the current hit
            rayHit = tempHit; // Cache the current hit for use during footstep event resolution
            lastSurfaceObject = currentSurfaceObject; // Update the last surface reference
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
            ~0, // Checks against all layers for now
            QueryTriggerInteraction.Ignore
        );
    }

    // The Public call from an outside source.
    public void FootstepEvent() // Called by animation event to resolve the current surface and play the matching footstep sound
    {
        switch (surfaceSourceType)
        {
            case SurfaceSourceTypes.SurfaceIdentifier:
                GetSurfaceTypeUsingSurfaceIdentifier(rayHit, currentSurfaceIdentifier);
                break;
            case SurfaceSourceTypes.MaterialGroups:
                GetSurfaceTypeUsingMaterialGroups(rayHit, currentMeshRenderer);
                break;
            case SurfaceSourceTypes.TerrainData:
                GetSurfaceTypeUsingTerrainIndex(rayHit, currentTerrain);
                break;
            default:
                surfaceType = SurfaceTypes.Default;
                break;
        }
    }

    // Final Step - Play the audio with the specified index.
    private void PlayFootstepSound(int index) // Creates and plays a one-shot FMOD footstep event using the surface parameter
    {
        EventInstance footstepInstance = RuntimeManager.CreateInstance(footstepEvent);

        if (Use3DSound)
        {
            footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(rayHit.point)); // Set 3D position to the footstep location
        }

        footstepInstance.setParameterByName("Surface", index);

        footstepInstance.start();
        footstepInstance.release(); // Safe for one-shot usage
    }

    // Determines which surface detection method should be used for the current hit - it also stores the current hit reference.
    private void TryResolveSurfaceSource(RaycastHit hit)
    {
        ClearReferences(); // Clear cached references at the start of detection to prevent stale data issues.

        if (hit.collider.TryGetComponent(out SurfaceIdentifier surfaceIdentifier)) // Highest priority: explicit surface assignment
        {
            surfaceSourceType = SurfaceSourceTypes.SurfaceIdentifier;
            currentSurfaceIdentifier = surfaceIdentifier; // Cache for use during footstep event
            return;
        }

        currentMeshRenderer = hit.collider.GetComponent<MeshRenderer>() ?? hit.collider.GetComponentInChildren<MeshRenderer>(); // Flexible renderer lookup across related objects
        if (currentMeshRenderer != null)
        {
            surfaceSourceType = SurfaceSourceTypes.MaterialGroups;
            return;
        }

        if (hit.collider.TryGetComponent(out Terrain terrain))
        {
            surfaceSourceType = SurfaceSourceTypes.TerrainData;
            currentTerrain = terrain; // Cache for use during footstep event
            return;
        }

        surfaceSourceType = SurfaceSourceTypes.Default; // Fallback when no supported source is found
    }

    // Material Group Resolver
    private void GetSurfaceTypeUsingMaterialGroups(RaycastHit hit, MeshRenderer meshRenderer) // Resolves the surface type by checking the hit object's material against defined groups
    {
        //Debug.Log("Resolving surface type using material groups for object: " + hit.collider.gameObject.name);
        currentSurfaceMaterial = meshRenderer.sharedMaterial; // Safe here, this path only runs when a MeshRenderer exists
        if (currentSurfaceMaterial == null)
        {
            //Debug.LogWarning("FootstepLogicV2: No material found on hit object for material group resolution.");
            return;
        }

        foreach (var group in surfaceTypeScript.surfaceMaterialGroups)
        {
            if (group.materials.Contains(currentSurfaceMaterial))
            {
                surfaceType = group.surfaceType;
                PlayFootstepSound((int)surfaceType); // Play the resolved footstep sound
                //Debug.Log("Material match found in group: " + group.surfaceType + " for material: " + currentSurfaceMaterial.name);
                return;
            }
            // If no material match is found, no sound is played
        }
    }

    // Surface Identifier Resolver
    private void GetSurfaceTypeUsingSurfaceIdentifier(RaycastHit hit, SurfaceIdentifier surfaceIdentifier)
    {
        surfaceType = currentSurfaceIdentifier.SurfaceType; // Directly uses the SurfaceIdentifier value
        PlayFootstepSound((int)surfaceType); // Play the resolved footstep sound
    }

    // Terrain Resolver
    private void GetSurfaceTypeUsingTerrainIndex(RaycastHit hit, Terrain terrain) // Resolves the surface type using the dominant terrain layer at the hit point
    {
        //Debug.Log("Resolving surface type using terrain data for object: " + hit.collider.gameObject.name);
        Vector3 worldPosition = hit.point;
        int terrainIndex = GetDominantTerrainLayerIndex(worldPosition);

        switch (terrainIndex) // Maps terrain layer indices to logical surface types
        {
            case 0:
            case 1:
                surfaceType = SurfaceTypes.Grass;
                break;
            case 2:
                surfaceType = SurfaceTypes.Metal;
                break;
            case 3:
                surfaceType = SurfaceTypes.Concrete;
                break;
            default:
                surfaceType = SurfaceTypes.Default;
                break;
        }

        PlayFootstepSound((int)surfaceType);
    }

    public int GetDominantTerrainLayerIndex(Vector3 worldPosition) // Returns the dominant terrain paint layer index at the given world position
    {
        if (currentTerrain == null)
            return -1;

        TerrainData terrainData = currentTerrain.terrainData;
        Vector3 localPos = worldPosition - currentTerrain.transform.position;

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

    private void ClearReferences() // Clears cached references after footstep processing to prevent stale data issues
    {
        currentSurfaceMaterial = null;
        currentSurfaceIdentifier = null;
        currentTerrain = null;
    }

    private void KillAudioEarly(EventInstance instance) // Stops and releases an FMOD instance early if needed
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}