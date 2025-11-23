using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopLocation : ShopManager
{
    [SerializeField] protected Dictionary<GameObject, int> currentItemList = new();
    [SerializeField] protected List<ShopItem> items = new();
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private PlayerWingventory inventoryRef;
    public ShopItem currentBoughtItem;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        SpawnItemsInSlots();
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        inventoryRef = FindFirstObjectByType<PlayerWingventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(resetFlag)UpdateItemInventory();
    }
    protected void SpawnItemsInSlots()
    {
        foreach (var slot in slots)
        {
            var randomIndex = Random.Range(0, base.itemDictionary.Count);
            var item = base.itemDictionary.ElementAt(randomIndex);
            var spawnedItem = Instantiate(item.Key).GetComponent<ShopItem>();
            currentItemList.TryAdd(item.Key,item.Value);
            items.Add(spawnedItem);
            spawnedItem.transform.position = slot.transform.position;
            Debug.Log("ItemSpawned:" + item.Key);
        }
    }

    public void InteractWithShop(BoxCollider box)
    {
        Debug.Log(box);
        var interacted = box;
        int index = 0;
        foreach (var slot in slots)
        {
            if (slot.GetComponent<BoxCollider>() == interacted)
            {
                currentBoughtItem = items[index];
                Debug.Log(currentBoughtItem);
                Debug.Log(items[index]);
                canvasController.OpenShopUI(currentBoughtItem);
                canvasController.shopLocationRef = this.GetComponent<ShopLocation>(); 
            }
            index++;
        }


    }

    protected void ExitShop()
    {
        canvasController?.CloseShopUI();
        canvasController.shopLocationRef = null;
    }

    public void MovePlayerToNextSlotLeft()
    {
        
    }

    public void MovePlayerToNextSlotRight()
    {

    }

    public void BuyAndRemoveItem(ShopItem item)
    {
        inventoryRef.inventory.Add(item.gameObject,1);
    }


    protected void UpdateItemInventory()
    {
        foreach (var item in items)
        {
            if (item != null) continue;
            var randomIndex = Random.Range(0, base.itemDictionary.Count);
            var newItem = base.itemDictionary.ElementAt(randomIndex);
            var spawnedItem = Instantiate(newItem.Key);
            spawnedItem.transform.position = item.transform.position;


        }
    }


}
