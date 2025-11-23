using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopConfirmUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject lowTrinketNotif;
    public ShopLocation shopLocation;
    public ShopItem currentItem;
    public UI_CanvasController canvasController;
    private PlayerWingventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopLocation = canvasController.shopLocationRef;
        nameText.SetText(currentItem.itemName);
        descriptionText.SetText(currentItem.itemDescription);
        costText.SetText(currentItem.cost.ToString());
        confirmButton.onClick.AddListener(BuyItem);
        cancelButton.onClick.AddListener(CloseWindow);
        leftButton.onClick.AddListener(shopLocation.MovePlayerToNextSlotLeft);
        inventory = FindFirstObjectByType<PlayerWingventory>();
        
    }

    private async void BuyItem()
    {
        if (inventory.playerTrinketQuantity >= currentItem.cost)
        {
            shopLocation.BuyAndRemoveItem(currentItem);
            inventory.playerTrinketQuantity -= currentItem.cost;
            Destroy(shopLocation.currentBoughtItem);
            canvasController.CloseShopUI();
            Destroy(this.gameObject);
        }
        else if (inventory.playerTrinketQuantity < currentItem.cost) 
        {   ShowLowTrinketNotif();
            await System.Threading.Tasks.Task.Delay(2000);
            HideLowTrinketNotif();
            Debug.Log("Not Enough Trinkets");

        }

    }

    private void CloseWindow()
    {
        canvasController.CloseShopUI();
        Destroy(this.gameObject);
    }

    public void ShowLowTrinketNotif()
    {
        lowTrinketNotif.SetActive(true);
    }

    private void HideLowTrinketNotif()
    {
        lowTrinketNotif.SetActive(false);
    }
}
