using UnityEngine;

public class IconToggle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private Sprite icon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        //iconRenderer = GetComponent<SpriteRenderer>();
        iconRenderer.sprite = icon;
        HideIcon();

    }

    public void ShowIcon()
    {
            iconRenderer.enabled = true;
        
    }

    public void HideIcon()
    {
            iconRenderer.enabled = false;
        
    }
}
