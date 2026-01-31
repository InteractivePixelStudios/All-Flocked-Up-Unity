using UnityEngine;
using FMODUnity;

public class AmbienceLogic_Wind : MonoBehaviour
{
    [SerializeField] EventReference windTimelineEvent; // Assign in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RuntimeManager.PlayOneShot(windTimelineEvent); // This will play the wind timeline event - it loops itself.
    }
}
