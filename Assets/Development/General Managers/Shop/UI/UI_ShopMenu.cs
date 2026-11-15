using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class UI_ShopMenu : ShopManager
{
    [SerializeField] protected Dictionary<UI_ShopItemButton, int> currentItemList = new();
    [SerializeField] protected List<UI_ShopItemButton> items = new();
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private PlayerWingventory inventoryRef;
    [SerializeField] private UI_ShopItemButton shopButtonPrefab;
    [SerializeField] private Button closeShopButton;
    [SerializeField] private TextMeshProUGUI trinketCountText;
    [SerializeField] private UI_NestMenu nestMenu;
    [SerializeField] private List<Button> itemButtons = new(); 
    public UI_ShopItemButton currentItem;
    private int trinketCount => inventoryRef.playerTrinketQuantity;
    [SerializeField] private GameObject confirmBuyPanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject trinketWarningPanel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        SpawnItemsInSlots();
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        inventoryRef = FindAnyObjectByType<PlayerWingventory>();
        closeShopButton.onClick.AddListener(ExitShop);
        trinketCountText.SetText(trinketCount.ToString());
        cancelButton.onClick.AddListener(HideConfirmBuyPanel);

    }

    protected void SpawnItemsInSlots()
    {
        foreach (var slot in slots)
        {
            var randomIndex = Random.Range(0, base.itemDictionary.Count);
            var item = base.itemDictionary.ElementAt(randomIndex);
            var spawnedItem = Instantiate(item.Key);
            spawnedItem.transform.SetParent(slot, false);
            spawnedItem.itemButton.onClick.AddListener(() => { ShowConfirmBuyPanel(item.Key); });
           // FindItemSprite(spawnedItem);
            currentItemList.TryAdd(item.Key, item.Value);
            items.Add(spawnedItem);
        }
    }

    //protected void FindItemSprite(UI_ShopItemButton item)
    //{
    //    var name = item.
    //    itemSprites.Contains()
    //}


    public void BuyAndRemoveItem(UI_ShopItemButton item)
    {
        if (inventoryRef.playerTrinketQuantity < item.cost)
        {
            ShowTrinketWarning(); 
            return;
        }
        inventoryRef.playerTrinketQuantity -= item.cost;
        inventoryRef.inventory.Add(item.name, 1);
        trinketCountText.SetText(trinketCount.ToString());
    }

    private async void ShowTrinketWarning()
    {
        trinketWarningPanel.SetActive(true);
        await Task.Delay(1000);
        trinketWarningPanel.SetActive(false);
    }

    private void ShowConfirmBuyPanel(UI_ShopItemButton item)
    {
        confirmButton.onClick.RemoveAllListeners();
        confirmBuyPanel.SetActive(true);
        currentItem = item;
        confirmButton.onClick.AddListener(() => { BuyAndRemoveItem(currentItem); });
    }

    private void HideConfirmBuyPanel()
    {
        confirmBuyPanel.SetActive(false);
    }

    protected void ExitShop()
    {
        nestMenu.CloseShopMenu();
    }

}
