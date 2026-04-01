#if UNITY_EDITOR

using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMOD_AV_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject visualizerPrefab;
    [SerializeField] private EventReference eventReference;

    private float minDistance;
    private float maxDistance;

    private void Awake()
    {
        if (visualizerPrefab == null || eventReference.IsNull)
        {
            Debug.LogWarning("FMOD_AV_Spawner: No visualizer prefab or FMOD event reference assigned. Please assign them in the inspector.");
            this.enabled = false; // Disable the script if no prefab is assigned to prevent errors.
            return;
        }
    }

    private void Start()
    {
        SpawnVisualizer();
        CacheEventDistances();
        ApplyToVisualizer();
    }

    private void SpawnVisualizer()
    {
        if (visualizerPrefab == null) return;

        GameObject visualizer = Instantiate(visualizerPrefab, transform.position, Quaternion.identity, transform);
        visualizer.name = "FMOD_AttenuationVisualizer";
    }

    private void CacheEventDistances()
    {
        EventDescription eventDesc = RuntimeManager.GetEventDescription(eventReference);

        if (eventDesc.isValid())
            eventDesc.getMinMaxDistance(out minDistance, out maxDistance);
        else
            Debug.LogWarning($"FMOD event not found: {eventReference.Path}");
    }

    private void ApplyToVisualizer()
    {
        FMODAttenuationVisualizer visualizer = GetComponentInChildren<FMODAttenuationVisualizer>();

        if (visualizer == null) return;

        visualizer.AdjustSpheres(minDistance, maxDistance);
    }
}

#endif