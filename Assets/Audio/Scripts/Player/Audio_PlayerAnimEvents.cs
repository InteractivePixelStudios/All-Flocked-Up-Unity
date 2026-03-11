using UnityEngine;

public class Audio_PlayerAnimEvents : MonoBehaviour
{
    private AudioWizard audioWizard;
    private Audio_Player footstep;


    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
        footstep = GetComponent<Audio_Player>();
    }

    public void PlayStepSound() // Testing, no Mat check.
    {
        if (footstep != null)
        {
            footstep.PlayerFootstep();
        }
    }
}
