using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/*
---Notes---
- Wing flaps use two different events - for now this is fine, but I will be changing this.
-
-
-
-
*/

public class Audio_Player : MonoBehaviour // REMINDER - Clean this damn script up later - IPM.
{
    [Header("FMOD Events")]
    [SerializeField] EventReference wingFlapUpEvent; // Assign in Inspector
    [SerializeField] EventReference wingFlapDownEvent; // Assign in Inspector
    [SerializeField] EventReference poopEvent; // Assign in Inspector
    [SerializeField] EventReference splatEvent; // Assign in Inspector

    [Header("FMOD Instances")] // These may not be needed as class variables.
    private EventInstance footstepInstance;
    private EventInstance wingFlapInstance;
    private EventInstance poopInstance;
    private EventInstance splatInstance;

    [SerializeField] LayerMask surfaceDetectionLayers; // Layers to detect surfaces on
    private bool canPlay;
    private bool mapIsOpen = false;
    private bool invIsOpen = false;
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

    public void WingFlapUp()
    {
        EventInstance wingFlapInstance = RuntimeManager.CreateInstance(wingFlapUpEvent);
        //wingFlapInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        wingFlapInstance.start();
        wingFlapInstance.release();
    }

    public void WingFlapDown()
    {
        EventInstance wingFlapInstance = RuntimeManager.CreateInstance(wingFlapDownEvent);
        //wingFlapInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        wingFlapInstance.start();
        wingFlapInstance.release();
    }

    public void Poop()
    {
        EventInstance poopInstance = RuntimeManager.CreateInstance(poopEvent);
        poopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

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
