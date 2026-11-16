using UnityEngine;
public class IconToggle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private Sprite icon;
    void Start()
    {
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
