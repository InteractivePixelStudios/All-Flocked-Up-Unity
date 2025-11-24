using UnityEngine;

public class AnimEventTesting : MonoBehaviour
{
    private AudioWizard audioWizard;
    private Audio_PlayerFootstepSurfaceDetection footstepSurfaceDetection;


    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
        footstepSurfaceDetection = GetComponent<Audio_PlayerFootstepSurfaceDetection>();
    }

    public void PlayFootStepSound() // Testing, no Mat check.
    {
        if (footstepSurfaceDetection != null)
        {
            footstepSurfaceDetection.PlayerFootstep();
        }
    }
}
