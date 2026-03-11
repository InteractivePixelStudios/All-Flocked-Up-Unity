using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[ExecuteAlways]
public class FMODAttenuationGizmo : MonoBehaviour
{
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

        if (!RuntimeManager.IsInitialized)
        {
            hasDistanceData = false;
            return;
        }

        var eventDescription = RuntimeManager.GetEventDescription(eventReference);

        if (eventDescription.isValid())
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
