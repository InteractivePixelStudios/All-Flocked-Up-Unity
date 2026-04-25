using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.ProBuilder.MeshOperations;
using Unity.VisualScripting;


public class PlayerWingventory : MonoBehaviour
{
   public int playerTrinketQuantity = 0;
    public int playerKeychainQuantity = 0;
    public int playerPrestoQuantity = 0;
   public Dictionary<string,int> inventory = new();
    [SerializeField] private List<ConsumableBase> consumables = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //var item = FindAnyObjectByType<ConsumableBase>();
        //AddItemToInv(item.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInv(string item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            foreach(var consumable in consumables)
            {
                if(consumable.name == item) { inventory.Add(consumable.name, quantity); }
            }
        }
    }

    public void RemoveItemFromInv(string item, int quantity)
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

    public void UseConsumeItem(string item)
    {
        foreach(var consumable in consumables)
        {
            if(consumable.name == item)
            {
                if (inventory.ContainsKey(consumable.name))
                {
                    consumable.GetComponent<ConsumableBase>().UseConsumable();
                }
            }
        }

    }

    public void DropItem(string item)
    {
        foreach (var consumable in consumables)
        {
            if (consumable.name == item)
            {
                if (inventory.ContainsKey(consumable.name))
                {
                    inventory.Remove(consumable.name);
                    var newItem = Instantiate(consumable, transform.position - new Vector3(0,-0.1f,4), transform.rotation);
                    newItem.name = item;
                    //Update this later to throw the object
                }
            }
        }
    }

    public void AddTrinketToInv(int amt, int index)
    {
        if (index == 2)
        {
            playerTrinketQuantity += amt;
        }
        else if(index == 1)
        {
            playerKeychainQuantity += amt;
        }
        else if (index == 3)
        {
            playerPrestoQuantity += amt;
        }
        else if (index == 0)
        {
            playerTrinketQuantity += amt;
        }
    }

    public Sprite FindItemSprite(string item)
    {
        Sprite sprite;
        foreach (var consumable in consumables)
        {
            if (consumable.name == item)
            {
                sprite = consumable.invSprite;
                return sprite;
            }
            else continue;
        }return null;
    }

}
