using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum AccessoryList { BottleCap, Monocle, Feather, Anklet, Bread, Rose, Reciept }

public class PlayerAccessoryComponent : MonoBehaviour
{
    public AccessoryList list;
    [SerializeField] private List<AccessoryBase> accessories;
    public List<AccessoryBase> currentEquippedAccessories = new();
    public AccessoryBase currentItem;
    public string currentItemName => currentItem.GetName();
    public string currentItemDesc =>currentItem.GetDescription();
    [SerializeField] protected Vector3 accessoryOffset;

    [SerializeField] protected GameObject headSlot;
    [SerializeField] protected GameObject neckSlot;
    [SerializeField] protected GameObject monocleSlot;

    private void Start()
    {


    }

    public string SendItemName()
    {
        Debug.Log("Called");
        return currentItemName;
    }

    public string SendItemDesc()
    {
        Debug.Log("DescCalled");
        return currentItemDesc;
    }
    public void EquipAccessory(AccessoryBase accessory, bool isEquip, EAccessoryItems state, GameObject slot)
    {
        currentItem = Instantiate(accessory);
        currentItem.isEquip = isEquip;
        currentItem.itemState = state;
        currentItem.transform.SetParent(slot.transform, false);
        currentItem.transform.position = slot.transform.position;
        currentItem.transform.rotation = slot.transform.rotation;
        currentEquippedAccessories.Add(currentItem);
        
        
    }

    public void RemoveAccessory(AccessoryBase accessory, bool isEquip)
    {
        if (currentEquippedAccessories.Contains(accessory))
        {
            accessory.isEquip = isEquip;
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
