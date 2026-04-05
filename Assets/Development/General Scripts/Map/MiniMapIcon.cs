using UnityEngine;
using UnityEngine.UI;

public class MiniMapIcon : MonoBehaviour
{
    [SerializeField] private Image comp;
    [SerializeField]Sprite savedIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
