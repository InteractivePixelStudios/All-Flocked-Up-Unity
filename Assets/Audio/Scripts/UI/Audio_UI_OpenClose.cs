using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.CompilerServices;

public class Audio_UI_OpenClose : MonoBehaviour
{
    public enum UIElement
    {
        Map,
        Inventory
    }

    //[SerializeField] private EventReference mapOpenCloseEvent; // Assign in Inspector
    //[SerializeField] private EventReference invOpenCloseEvent; // Assign in Inspector
    public UIElement uiElement; // Set in Inspector to specify which UI element this script is attached to
    private bool mapIsOpen = false;
    private bool invIsOpen = false;

    private void Start()
    {
        PlaySound(); // Play sound on start to set initial state
    }

    private void OnDestroy()
    {
        PlaySound(); // Play sound on destroy to set closing state
    }

    private void PlaySound()
    {
        switch (uiElement)
        {
            case UIElement.Map:
                mapIsOpen = !mapIsOpen; // Toggle map state
                MapOpenClose();
                break;
            case UIElement.Inventory:
                invIsOpen = !invIsOpen; // Toggle inventory state
                InvOpenClose();
                break;
        }
    }

    public void MapOpenClose()
    {
        EventInstance instance = RuntimeManager.CreateInstance("event:/UI/Menus/Map"); // Replace with your actual event path
        instance.setParameterByName("MenuOpenClose", mapIsOpen ? 0f : 1f);
        instance.start();
        instance.release();
    }

    public void InvOpenClose()
    {
        EventInstance instance = RuntimeManager.CreateInstance("event:/UI/Menus/Inventory"); // Replace with your actual event path
        instance.setParameterByName("MenuOpenClose", invIsOpen ? 0f : 1f);
        instance.start();
        instance.release();
    }
}
