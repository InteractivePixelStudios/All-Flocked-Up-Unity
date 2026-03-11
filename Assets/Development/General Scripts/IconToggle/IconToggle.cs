using UnityEngine;

public class IconToggle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        iconRenderer = GetComponent<SpriteRenderer>();
        iconRenderer.sprite = icon;
        HideIcon();

    }

    public void ShowIcon()
    {

            isActive = true;
            iconRenderer.enabled = true;
        
    }

    public void HideIcon()
    {

            isActive = false;
            iconRenderer.enabled = false;
        
    }
}
