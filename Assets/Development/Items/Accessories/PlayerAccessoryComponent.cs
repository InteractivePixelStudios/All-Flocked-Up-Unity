using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum AccessoryList { BottleCap, Monocle, Feather, Anklet, Bread, Rose, Reciept }

public class PlayerAccessoryComponent : MonoBehaviour
{
    public AccessoryList list;
    [SerializeField] private List<AccessoryBase> accessories;
    [SerializeField] private List<AccessoryBase> currentEquippedAccessories = new();
    [SerializeField] private AccessoryBase currentItem;
    [SerializeField] protected Vector3 accessoryOffset;

    [SerializeField] protected Transform headSlot;
    [SerializeField] protected Transform neckSlot;
    [SerializeField] protected Transform monocleSlot;

    private void Start()
    {


    }
    public void EquipAccessory(AccessoryBase accessory, bool isEquip, EAccessoryItems state, Transform slot)
    {
        currentItem = Instantiate(accessory);
        currentItem.accessoryTransform = slot;
        currentItem.itemState = state;
        currentEquippedAccessories.Add(currentItem);
        currentItem.transform.localPosition += accessoryOffset;
        
    }

    public void RemoveAccessory(AccessoryBase accessory, bool isEquip)
    {
        if (currentEquippedAccessories.Contains(accessory))
        {
            currentEquippedAccessories.Remove(accessory);
            Destroy(accessory);
        }
    }
    public void GetAndEquipAccessory()
    {
        switch(list) { 
            case AccessoryList.BottleCap:
                EquipAccessory(accessories[0], true,EAccessoryItems.BottleCap, headSlot);
                break;
            case AccessoryList.Monocle:
                EquipAccessory(accessories[1],true, EAccessoryItems.Monocle, monocleSlot);
                break;
            case AccessoryList.Feather:
                EquipAccessory(accessories[2], true, EAccessoryItems.Feather, headSlot);
                break;
            case AccessoryList.Bread:
                EquipAccessory(accessories[3], true, EAccessoryItems.Bread, neckSlot);
                break;
            case AccessoryList.Anklet:
                EquipAccessory(accessories[4], true, EAccessoryItems.Anklet, neckSlot);
                break;
            case AccessoryList.Rose:
                EquipAccessory(accessories[5], true, EAccessoryItems.Rose, headSlot);
                break;
            case AccessoryList.Reciept:
                EquipAccessory(accessories[6], true, EAccessoryItems.Reciept, neckSlot);
                break;
        }
    }

}
