using UnityEngine;
using FMODUnity;
using FMOD.Studio;

//[ExecuteInEditMode]
public class Audio_Player : MonoBehaviour // REMINDER - Clean this damn script up later - IPM.
{
    [Header("FMOD Events")]
    [SerializeField] EventReference wingFlapEvent; // Assign in Inspector
    [SerializeField] EventReference poopEvent; // Assign in Inspector
    [SerializeField] EventReference splatEvent; // Assign in Inspector

    [Header("FMOD Instances")] //Used to control audio playback and early cancellation
    private EventInstance footstepInstance;
    private EventInstance wingFlapInstance;
    private EventInstance poopInstance;
    private EventInstance splatInstance;

    [SerializeField] LayerMask surfaceDetectionLayers; // Layers to detect surfaces on
    private bool canPlay;
    private AudioWizard audioWizard;

    public void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
    }

    private void KillAudioEarly(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    public void WingFlap()
    {
        EventInstance wingFlapInstance = RuntimeManager.CreateInstance(wingFlapEvent);
        //wingFlapInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        wingFlapInstance.start();
        wingFlapInstance.release();
    }

    public void Poop()
    {
        EventInstance poopInstance = RuntimeManager.CreateInstance(poopEvent);
        //poopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        poopInstance.start();
        poopInstance.release();
    }

    public void Splat()
    {
        EventInstance splatInstance = RuntimeManager.CreateInstance(splatEvent);
        //splatInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        splatInstance.start();
        splatInstance.release();
    }
}
