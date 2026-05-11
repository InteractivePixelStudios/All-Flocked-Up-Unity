using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Audio_OmniLoop : MonoBehaviour // Universal script for any and all world objects that play a looping Timeline.
{
    [SerializeField] private EventReference loopEvent; // Assign in Inspector
    [SerializeField] private GameObject attenuationPrefab;
    private EventInstance loopInstance;

    [Header("Debug Settings - Do Not Leave Triggered")]
    public bool showAttenuationGizmo; // For debugging - shows the attenuation range when the object is selected in the editor.

    private float minDistance;
    private float maxDistance;

    private void Start()
    {
        loopInstance = RuntimeManager.CreateInstance(loopEvent);
        loopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        loopInstance.start();

        if (showAttenuationGizmo)
        {
            ShowAttenuation();
        }
    }

    private void ShowAttenuation()
    {
        RuntimeManager.GetEventDescription(loopEvent).getMinMaxDistance(out float minDistance, out float maxDistance);
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;

        var instantiatedAttenuation = Instantiate(attenuationPrefab, transform.position, Quaternion.identity, transform);
        var visualizer = instantiatedAttenuation.GetComponent<FMODAttenuationVisualizer>();
        visualizer.AdjustSpheres(minDistance, maxDistance);
    }
}
