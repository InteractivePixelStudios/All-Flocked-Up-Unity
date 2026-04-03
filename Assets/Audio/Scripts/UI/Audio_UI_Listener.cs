using UnityEngine;
using UnityEngine.EventSystems;

public class Audio_UI_Listener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    private AudioWizard audioWizard;
    private bool canPlaySound;

    private void Start()
    {
        audioWizard = AudioWizard.Instance; // Get the instance of the AudioWizard
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioWizard != null && canPlaySound)
        {
            canPlaySound = false;
            audioWizard.PlayButtonHoverSound();
        }
            
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canPlaySound = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (audioWizard != null)
            audioWizard.PlayButtonHoverSound();
    }

    public void OnButtonClick()
    {
        if (audioWizard != null)
        {
            audioWizard.PlayButtonClickSound(); // Call the method to play the button click sound
        }
    }
}
