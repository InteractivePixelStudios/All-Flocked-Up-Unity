using UnityEngine;

public class NPC_IconToggle : MonoBehaviour
{
    SpriteRenderer iconRenderer;

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
