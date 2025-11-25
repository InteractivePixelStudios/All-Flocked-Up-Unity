using UnityEngine;
using FMODUnity;

public class AmbienceLogic_Music : MonoBehaviour
{
    [SerializeField] EventReference musicTimelineEvent; // Assign in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RuntimeManager.PlayOneShot(musicTimelineEvent); // This will play the music timeline event - it loops itself.
    }

    public void AdjustVolume(float volume)
    {
        // Placeholder for future volume adjustment logic - right now I have no idea how volume works with FMOD.
    }
}
