using UnityEngine;

public class FMODAttenuationVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject minSphere;
    [SerializeField] private GameObject midPointSphere;
    [SerializeField] private GameObject maxSphere;

    public void AdjustSpheres(float minDistance, float maxDistance)
    {
        if (minSphere != null)
        {
            minSphere.transform.localScale = Vector3.one * minDistance;
        }

        if (midPointSphere != null)
        {
            float midDistance = (minDistance + maxDistance) / 2f;
            midPointSphere.transform.localScale = Vector3.one * midDistance;
        }

        if (maxSphere != null)
        {
            maxSphere.transform.localScale = Vector3.one * maxDistance;
        }
    }
}

