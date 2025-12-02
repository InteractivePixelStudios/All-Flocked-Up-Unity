using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class PoopSystem : MonoBehaviour
{
    [Header ("Poop Settings")]
    [SerializeField] private int maxPoop = 5;
    [SerializeField] private float poopCooldown = 2.0f;
    [SerializeField] private int poopBonus = 0;
    [SerializeField] private PlayerAccessoryComponent accessoryComponent;
    private List<AccessoryBase> equippedAccessories = new();

    private int currentPoop;
    [SerializeField] private float cooldownTimer = 1.5f;
    [SerializeField] private float updateItemsTimer = 30f;

    public bool CanPoop => cooldownTimer <= 0f && currentPoop > 0;

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
        else GetCurrentAccessories() ;
    }

    public void GainPoop(int poop)
    {
        currentPoop += poop;
    }

    public bool TryPoop()
    {
        if (!CanPoop) return false;

        currentPoop--;
        cooldownTimer = poopCooldown;
        return true;
    }

    //Logic to restore poop, ensure does not go past max poop count
    private void RestorePoop(int amount) => currentPoop = Mathf.Min(currentPoop + amount, maxPoop);
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
