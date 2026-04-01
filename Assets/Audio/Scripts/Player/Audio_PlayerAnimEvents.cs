using UnityEngine;

public class Audio_PlayerAnimEvents : MonoBehaviour
{
    private AudioWizard audioWizard;
    private FootstepLogicV2 footstep;


    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
        footstep = GetComponent<FootstepLogicV2>();
    }

    public void PlayStepSound() // Testing, no Mat check.
    {
        if (footstep != null)
        {
            footstep.FootstepEvent();
        }
    }
}
