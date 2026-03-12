using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class NPC_Vocalizer : MonoBehaviour
{
    [SerializeField]EventReference speechEventRef;
    [SerializeField] EventInstance speechEventInstance;

    public void PlaySpeechSound()
    {
        RuntimeManager.PlayOneShot(speechEventRef, transform.position);
    }

    public void Speech()
    {
        EventInstance speech = RuntimeManager.CreateInstance(speechEventRef);

        speech.start();
        speech.release();
    }
}
