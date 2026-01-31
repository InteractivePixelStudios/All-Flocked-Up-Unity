using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[ExecuteAlways]
public class FMODAttenuationGizmo : MonoBehaviour
{
    [EventRef]
    public EventReference eventReference;

    public Color minColor = Color.green;
    public Color maxColor = Color.red;

    private float minDistance;
    private float maxDistance;
    private bool hasDistanceData;

    private void Update()
    {
        if (eventReference.IsNull)
        {
            hasDistanceData = false;
            return;
        }

        // Only try to talk to FMOD when it's initialized (play mode or editor with FMOD loaded)
        if (!RuntimeManager.IsInitialized)
        {
            hasDistanceData = false;
            return;
        }

        EventDescription eventDescription;
        var result = RuntimeManager.StudioSystem.getEvent(eventReference.Path, out eventDescription);

        if (result == FMOD.RESULT.OK && eventDescription.isValid())
        {
            eventDescription.getMinMaxDistance(out minDistance, out maxDistance);
            hasDistanceData = true;
        }
        else
        {
            hasDistanceData = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!hasDistanceData)
            return;

        // Min distance
        Gizmos.color = minColor;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        // Max distance
        Gizmos.color = maxColor;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
