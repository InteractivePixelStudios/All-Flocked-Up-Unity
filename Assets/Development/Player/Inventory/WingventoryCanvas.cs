using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WingventoryCanvas : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject centerCanvas;
    [SerializeField] private GameObject leftCanvas;
    [SerializeField] private GameObject rightCanvas;

    [Header("Buttons")]
    [SerializeField] private Button leftPageButton;
    [SerializeField] private Button rightPageButton;
    [SerializeField] private Button leftBackPageButton;
    [SerializeField] private Button rightBackPageButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button accessoryPanelButton;
    [SerializeField] private Button mapButton;
    [Header("Inv/Accessory")]
    [SerializeField] private PlayerWingventory playerWingventory;
    [SerializeField] private Dictionary<GameObject, int> playerInvItems = new();
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private AccessoryPanelCanvas accessoryPanelPrefab;
    [Header("Trinket")]
    [SerializeField] private int currentTrinketCount=>GetTrinketCount();
    [SerializeField] private TextMeshProUGUI trinketCountText;
    [Header("ItemButtons")]
    [SerializeField] private UI_ItemButton itemButtonPrefab;
    public Dictionary<UI_ItemButton, int> currentItemButtons = new();
    [SerializeField] private ScrollRect invBox;
    [SerializeField]private List<ScrollRect> itemBoxes = new();

    [Header("QuestRef")]
    [SerializeField] private QuestLog questLog;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questLog = FindFirstObjectByType<QuestLog>();
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        playerWingventory = FindFirstObjectByType<PlayerWingventory>();
        GetTrinketCount();
        GetItemBoxes();
        leftBackPageButton.onClick.AddListener(GoCenterPage);
        rightBackPageButton.onClick.AddListener(GoCenterPage);
        leftPageButton.onClick.AddListener(GoLeftPage);
        rightPageButton.onClick.AddListener(GoRightPage);
        closeButton.onClick.AddListener(CloseWingventory);
        accessoryPanelButton.onClick.AddListener(OpenAccessoryPanel);
        mapButton.onClick.AddListener(OpenMap);
        leftCanvas.SetActive(false);
        rightCanvas.SetActive(false);
        SetTrinketText();
        GetPlayerInv();
        SpawnItemButton();
        GoCenterPage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GoLeftPage()
    {

        leftCanvas.SetActive(true);
        rightCanvas.SetActive(false);
    }

    private void GoRightPage()
    {

        leftCanvas.SetActive(false);
        rightCanvas.SetActive(true);
    }

    private void GoCenterPage()
    {

        leftCanvas.SetActive(false);
        rightCanvas.SetActive(false);
    }

    private void CloseWingventory()
    {
        canvasController.CloseWingventory();
        Destroy(this.gameObject);
    }

    private int GetTrinketCount()
    {
        int trinketCount= playerWingventory.playerTrinketQuantity;
            return trinketCount;
    }

    private void SetTrinketText()
    {
        trinketCountText.SetText(currentTrinketCount.ToString());
    }

    private void OpenAccessoryPanel()
    {
        Instantiate(accessoryPanelPrefab);
    }


    private void GetCurrentQuestInfo()
    {
        //questLog.activeQuests[0].
    }

    private void OpenMap()
    {

    }

    private void UpdateMapLocation()
    {

    }

    private void CloseMap()
    {

    }

    private void GetItemBoxes()
    {
        ScrollRect[] boxes = GetComponentsInChildren<ScrollRect>();
        foreach(var box in boxes)
        {
            itemBoxes.Add(box);
        }
    }

    private void SpawnItemButton()
    {
        var boxIndex = 0;
        foreach(var item in playerInvItems)
        {
            var buttonObj = Instantiate(itemButtonPrefab, itemBoxes[boxIndex].viewport.transform,false);
            var button = buttonObj.GetComponent<UI_ItemButton>();
            buttonObj.transform.localPosition = Vector3.zero;
            buttonObj.transform.localRotation = Quaternion.identity;


            button.itemQuantityText.SetText(item.Value.ToString());
            button.itemRef = item.Key;
            //button.itemImage = item.Key;
            boxIndex++;
        }
    }

    private void GetPlayerInv()
    {
        var playerInv = FindFirstObjectByType<PlayerWingventory>().inventory;
        foreach (var item in playerInv)
        {
            playerInvItems.Add(item.Key, item.Value);
        }
    }
}
