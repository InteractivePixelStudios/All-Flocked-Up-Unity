using UnityEngine;
using System.Collections.Generic;

public class MapIcon : MonoBehaviour
{
    private SpriteRenderer renderComp;
    [SerializeField] private List<Sprite> icons = new();
    [SerializeField]private int index;
    private Sprite currentSprite;
    void Start()
    {
        renderComp = GetComponent<SpriteRenderer>();
        currentSprite = icons[index];
        renderComp.sprite = currentSprite;
    }

    public Sprite GetCurrentSprite()
    {
        return currentSprite;
    }
}
