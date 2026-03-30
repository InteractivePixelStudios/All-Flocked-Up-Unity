using UnityEngine;
using FMODUnity;

public class AmbienceLogic_Music : MonoBehaviour
{
    [SerializeField] EventReference musicTimelineEvent; // Assign in Inspector
    private FMOD.Studio.EventInstance timeLineInstance;
    private bool musicCreated = false; // This is to prevent multiple instances of the music timeline from being created.
    void Start()
    {
        CreateMusicTimeline();
    }

    private void CreateMusicTimeline()
    {
        if (!musicCreated)
        {
            timeLineInstance = RuntimeManager.CreateInstance(musicTimelineEvent);
            timeLineInstance.setTimelinePosition(390000);
            timeLineInstance.start();
            musicCreated = true;
        }
    }
}
