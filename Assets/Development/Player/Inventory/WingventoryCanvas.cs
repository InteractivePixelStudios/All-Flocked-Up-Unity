using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Localization;
using System.Net;

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
    [SerializeField] private Button questPanelButton;
    [SerializeField] private Button mapPanelButton;
    [SerializeField] private Button cameraButton;

    [Header("Inv/Accessory")]
    [SerializeField] private PlayerWingventory playerWingventory;
    public Dictionary<string, int> playerInvItems = new();
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private GameObject questParent;
    [SerializeField] private GameObject mapParent;
    [SerializeField] private GameObject invParent;
    [Header("Trinket")]
    private int currentTrinketCount=>GetTrinketCount();
    private int currentkeyChainCount => GetKeychainCount();
    private int currentPrestoCount => GetPrestoCount();
    private LocalizedString currentObjective => GetCurrentQuestInfo();
    [SerializeField] private TextMeshProUGUI trinketCountText;
    [SerializeField] private TextMeshProUGUI keychainText;
    [SerializeField] private TextMeshProUGUI prestoText;
    [Header("ItemButtons")]
    [SerializeField] private UI_ItemButton itemButtonPrefab;
    public Dictionary<UI_ItemButton, int> currentItemButtons = new();
    [SerializeField] private ScrollRect invBox;
    [SerializeField]private List<ScrollRect> itemBoxes = new();

    [Header("QuestRef")]
    [SerializeField] private QuestLog questLog;
    [SerializeField] private TextMeshProUGUI questObjText;

    [SerializeField] ScreenshotCameraController screenshotController;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questLog = FindAnyObjectByType<QuestLog>();
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerWingventory = FindAnyObjectByType<PlayerWingventory>();
        screenshotController = FindAnyObjectByType<ScreenshotCameraController>();
        GetTrinketCount();
        GetItemBoxes();
        leftBackPageButton.onClick.AddListener(GoCenterPage);
        leftPageButton.onClick.AddListener(GoLeftPage);
        rightPageButton.onClick.AddListener(GoRightPage);
        closeButton.onClick.AddListener(CloseWingventory);
        questPanelButton.onClick.AddListener(OpenQuestPanel);
        mapPanelButton.onClick.AddListener(OpenMapPanel);
        cameraButton.onClick.AddListener(OpenCamera);
        leftCanvas.SetActive(false);
        rightCanvas.SetActive(false);
        SetTrinketText();
        SetKeychainText();
        SetPrestoText();
        SetObjectiveText();
        GetPlayerInv();
        SpawnItemButton();
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    private void GoLeftPage()
    {
        centerCanvas.SetActive(false);
        leftCanvas.SetActive(true);
        rightCanvas.SetActive(false);
    }

    private void GoRightPage()
    {
        centerCanvas.SetActive(false);
        leftCanvas.SetActive(false);
        rightCanvas.SetActive(true);
    }

    private void GoCenterPage()
    {
        centerCanvas.SetActive(true);
        leftCanvas.SetActive(false);
        rightCanvas.SetActive(false);
        Debug.Log("CenterPage");
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

    private int GetKeychainCount()
    {
        int keychainCount = playerWingventory.playerKeychainQuantity;
        return keychainCount;
    }

    private int GetPrestoCount()
    {
        int PrestoCount = playerWingventory.playerPrestoQuantity;
        return PrestoCount;
    }

    private void SetTrinketText()
    {
        trinketCountText.SetText(currentTrinketCount.ToString());
    }

    private void SetKeychainText()
    {
        keychainText.SetText(currentkeyChainCount.ToString());
    }

    private void SetPrestoText()
    {
        prestoText.SetText(currentPrestoCount.ToString());
    }

    private void SetObjectiveText()
    {
        if (currentObjective != null)
        {
            questObjText.SetText(currentObjective.GetLocalizedString());
        }else
        {
            questObjText.SetText("No Current Objective");
        }
    }

    private void OpenQuestPanel()
    {
        if(!questParent.gameObject.activeInHierarchy)
        {
            invParent.SetActive(false);
            mapParent.SetActive(false);
            questParent.SetActive(true);
            Debug.Log("QuestOpen");
        }
        else
        {
            invParent.SetActive(true);
            mapParent.SetActive(false);
            questParent.SetActive(false);
            Debug.Log("QuestClosed");
        }
    }


    private void OpenMapPanel()
    {
        if (!mapParent.gameObject.activeInHierarchy)
        {
            invParent.SetActive(false);
            mapParent.SetActive(true);
            questParent.SetActive(false);
        }
        else
        {
            invParent.SetActive(true);
            mapParent.SetActive(false);
            questParent.SetActive(false);
        }
    }


    private LocalizedString GetCurrentQuestInfo()
    {
        if (questLog.activeQuests.Count > 0)
        {
            var objective = questLog.activeQuests[0].questData.stages[0].objectivesToComplete[0].objectiveDescription;
            if (objective != null)
            {
                return objective;
            }
            else return null;
        }
        else return null;


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
            button.SetWingRef(playerWingventory);

            button.itemQuantityText.SetText(item.Value.ToString());
            button.itemRef = item.Key;
            button.SetWingUIRef(canvasController.activeWingventory);
            button.itemCount = item.Value;

            button.itemImage.sprite = playerWingventory.FindItemSprite(item.Key);
            boxIndex++;
        }
    }

    private void GetPlayerInv()
    {
        var playerInv = FindAnyObjectByType<PlayerWingventory>().inventory;
        foreach (var item in playerInv)
        {
            playerInvItems.Add(item.Key, item.Value);
        }
    }

    public void RemoveItemFromInv(UI_ItemButton button)
    {

            Destroy(button);
            if (currentItemButtons.ContainsKey(button))
            {
                currentItemButtons.Remove(button);
                Debug.Log("Removed from dictionary");

            }

    }

    void OpenCamera()
    {
        screenshotController.CallEnterPhotoMode();
        CloseWingventory();
    }

    void CloseCamera()
    {
        
        //uncertain if we will need this from the inventory - Jacob
        //screenshotController.CallExitPhotoMode();
    }




}
