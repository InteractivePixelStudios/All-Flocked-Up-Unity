using UnityEngine;

/*
---Notes---
-  Add safety checks
-
-
-
-
*/

public class Audio_PlayerAnimEvents : MonoBehaviour
{
    private AudioWizard audioWizard;
    private Audio_Player audioPlayer;
    private FootstepLogicV2 footstep;


    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Access the singleton instance
        footstep = GetComponent<FootstepLogicV2>();
        audioPlayer = GetComponent<Audio_Player>();
    }

    public void CallStepSound() // Testing, no Mat check.
    {
        if (footstep != null)
        {
            footstep.FootstepEvent();
        }
    }

    public void CallWingFlapUpSound()
    {
        if (audioPlayer != null)
        {
            audioPlayer.WingFlapUp();
        }
    }

    public void CallWingFlapDownSound()
    {
        if (audioPlayer != null)
        {
            audioPlayer.WingFlapDown();
        }
    }
}
