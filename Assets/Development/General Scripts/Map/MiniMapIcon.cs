using UnityEngine;
using UnityEngine.UI;
public class MiniMapIcon : MonoBehaviour
{
    [SerializeField] private Image comp;
    [SerializeField]Sprite savedIcon;
    void Awake()
    {
        comp = GetComponentInChildren<Image>();
    }
    public void SetIcon(Sprite icon)
    {
        savedIcon = icon;
        SetSprite();
    }

    private void SetSprite()
    {
        comp.sprite = savedIcon;
    }   
}
