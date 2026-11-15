using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ShopManager : MonoBehaviour
{
    [SerializeField] protected List<UI_ShopItemButton> shopItems = new();
    [SerializeField] protected Dictionary<UI_ShopItemButton, int> itemDictionary = new();
    [SerializeField] protected List<Transform> slots = new();
    [SerializeField] protected List<Sprite> itemSprites = new();


    protected virtual void Start()
    {
        foreach (var item in shopItems)
        {
            itemDictionary.Add(item, item.cost);
        }
    }



}
