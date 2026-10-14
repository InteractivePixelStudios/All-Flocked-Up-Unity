using UnityEngine;

public class NPC_IconToggle : MonoBehaviour
{
    SpriteRenderer iconRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iconRenderer = GetComponentInChildren<SpriteRenderer>();   
    }

    public void ShowNPCIcon()
    {
        iconRenderer.enabled = true;

    }

    public void HideNPCIcon()
    {
        iconRenderer.enabled = false;

    }
}
