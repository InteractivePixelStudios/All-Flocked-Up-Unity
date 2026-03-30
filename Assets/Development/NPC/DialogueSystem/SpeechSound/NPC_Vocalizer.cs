using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class NPC_Vocalizer : MonoBehaviour
{
    [SerializeField] private EventReference speechEventRef;
    [SerializeField] private EventInstance speechEventInstance;

    public void PlaySpeechSound()
    {
        RuntimeManager.PlayOneShot(speechEventRef, transform.position);
    }

    public void Speech()
    {
        speechEventInstance = RuntimeManager.CreateInstance(speechEventRef);

        speechEventInstance.start();
        speechEventInstance.release();
    }
}
