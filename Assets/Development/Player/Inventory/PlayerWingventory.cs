using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class PlayerWingventory : MonoBehaviour
{
   public int playerTrinketQuantity = 0;
   public Dictionary<GameObject,int> inventory = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var item = FindAnyObjectByType<ConsumableBase>();
        AddItemToInv(item.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInv(GameObject item, int quantity)
    {
        if (inventory.ContainsKey(item) && item.GetComponent<ConsumableBase>())
        {
            inventory[item] += quantity;
        }
        else inventory.Add(item, quantity);
    }

    public void RemoveItemFromInv(GameObject item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= quantity;
        }
        else inventory.Remove(item);
    }

    private void UpdateInv()
    {
        if (inventory.Count <= 0) return;
        foreach (var item in inventory)
        {
            if(item.Value < 0)
            {
                RemoveItemFromInv(item.Key, item.Value);
            }
        }

    }

    public void UseConsumeItem(GameObject item)
    {
        if (inventory.ContainsKey(item))
        {
            item.GetComponent<ConsumableBase>().UseConsumable();
        }
    }

    public void DropItem(GameObject item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory.Remove(item);
            Instantiate(item);
            //Update this later to throw the object
        }
    }

    public void AddTrinketToInv(int amt)
    {
        playerTrinketQuantity += amt;
    }

    
}
