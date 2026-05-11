using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapIcon : MonoBehaviour
{
    private SpriteRenderer renderComp;
    [SerializeField] private List<Sprite> icons = new();
    [SerializeField]private int index;
    private Sprite currentSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
