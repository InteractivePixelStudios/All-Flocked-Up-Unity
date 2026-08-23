using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField]bool hasVFX;
    bool isGrabbed;


    public void ToggleVFXOn()
    {
        if (hasVFX && !isGrabbed)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            isGrabbed = true;
        }
        else return;
    }

    public void ToggleVFXOff()
    {
        if (hasVFX && isGrabbed)
        {
            GetComponentInChildren<ParticleSystem>().Stop();
            isGrabbed = false;
        }
        else return;
    }

}
