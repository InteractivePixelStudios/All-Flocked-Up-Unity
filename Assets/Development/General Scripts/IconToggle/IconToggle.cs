using UnityEngine;

public class IconToggle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iconRenderer = GetComponentInChildren<SpriteRenderer>();
        iconRenderer.sprite = icon;

    }

    void ShowIcon()
    {
        if(!isActive)
        {
            isActive = true;
        }
    }

    void HideIcon()
    {
        if (isActive)
        {
            isActive = false;
        }
    }
}
