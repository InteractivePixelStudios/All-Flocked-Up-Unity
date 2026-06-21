using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class PoopSystem : MonoBehaviour
{
    [Header("Poop Settings")]
    [SerializeField] private int maxPoop = 10;
    [SerializeField] private float poopCooldown = 2.0f;
    [SerializeField] private int poopBonus = 0;
    [SerializeField] private PlayerAccessoryComponent accessoryComponent;
    private List<AccessoryBase> equippedAccessories = new();

    private int currentPoop;
    [SerializeField] private float cooldownTimer = 1.5f;
    [SerializeField] private float updateItemsTimer = 2f;

    public bool CanPoop => cooldownTimer <= 0f;

    public int GetCurrentPoop()
    {
        return currentPoop;
    }

    public int GetMaxPoop()
    {
        return maxPoop;
    }

    public void SetMaxPoop(int poop)
    {
        maxPoop = poop;
    }

    private void Awake()
    {
        currentPoop = maxPoop;
        accessoryComponent = GetComponentInParent<PlayerAccessoryComponent>();
    }



    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else return;
        if (updateItemsTimer > 0f)
        {
            updateItemsTimer -= Time.deltaTime;
        }
        else GetCurrentAccessories();
    }

    public void AddMaxPoop(int value)
    {
        maxPoop += value;
    }

    public void GainPoop(int poop)
    {
        currentPoop += poop;
    }

    public bool TryPoop()
    {
        if (!CanPoop || currentPoop<=0) return false;
        currentPoop--;
        cooldownTimer = poopCooldown;
        return true;
    }

    //Logic to restore poop, ensure does not go past max poop count
    private void RestorePoop(int amount) => currentPoop = (int)Mathf.Lerp(currentPoop,Mathf.Min(currentPoop + amount, maxPoop),Time.deltaTime);
    //logic to increase the maximum poop count
    private void IncreaseMaxPoop(int amount) => maxPoop += amount;

    private void GetCurrentAccessories()
    {
        foreach (var item in accessoryComponent.currentEquippedAccessories)
        {
            if (accessoryComponent.currentEquippedAccessories.Contains(item))
            {
                equippedAccessories.Add(item);
            }
            else
            {
                equippedAccessories.Remove(item);
            }
        };
        AddAccessoryBonus();
    }

    private void AddAccessoryBonus()
    {
        foreach(var item in equippedAccessories)
        {
            poopBonus += item.poopStatBonus;
        }

        maxPoop += poopBonus;
        updateItemsTimer = 30f;
    }

    


}
