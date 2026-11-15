using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_NestMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button closeMainButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button skinButton;
    [SerializeField] private Button closeShopButton;
    [SerializeField] private Button statsButton;
    [SerializeField] private Button closeStatsButton;
    [SerializeField] private Button scrapbookButton;
    [SerializeField] private Button sbNextPage;
    [SerializeField] private Button sbPrevPage;
    [SerializeField] private Button closeSbButton;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI poopText;
    [Header("ScrapBookRefs")]
    [SerializeField] private List<Image> photoList;
    [SerializeField] private List<Sprite> photos;
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject skinPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject scrapbookPanel;
    
    [Header("Components")]
    public UI_CanvasController canvasController;
    public PlayerCounter playerStats;

    [Header("Counters")]
    [SerializeField] private Dictionary<string, int> counterDictionary = new();
    [SerializeField] private RectTransform statBox;
    [SerializeField] private CounterTextBox counterTextPrefab;
    private List<CounterTextBox> statTextPrefabs = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();

    }
    void Start()
    {
        closeMainButton.onClick.AddListener(CloseNestMenu);
        shopButton.onClick.AddListener(OpenShopMenu);
        skinButton.onClick.AddListener(OpenSkinSelector);
        closeShopButton.onClick.AddListener(CloseShopMenu);
        statsButton.onClick.AddListener(OpenStats);
        closeStatsButton.onClick.AddListener(CloseStats);
        scrapbookButton.onClick.AddListener(OpenScrapbook);
        closeSbButton.onClick.AddListener(CloseScrapbook);
        //sbNextPage.onClick.AddListener(SBNextPage);
        //sbPrevPage.onClick.AddListener(SBPrevPage);
        GetStats();
        if (Gamepad.current.IsActuated())
        {
            EventSystem.current.SetSelectedGameObject(shopButton.gameObject);
        }
        else return;


    }

    private void CloseNestMenu()
    {
        canvasController.CloseNestMenu();
    }

    private void OpenShopMenu()
    {
        mainPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void CloseShopMenu()
    {
        mainPanel?.SetActive(true);
        shopPanel.SetActive(false);
    }

    private void OpenSkinSelector()
    {
        mainPanel?.SetActive(false);
        skinPanel.SetActive(true);
        if (SteamManager.Initialized)
        {
            AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_006_Skin");
        }
    }

    public void CloseSkinSelector()
    {
        mainPanel?.SetActive(true);
        skinPanel.SetActive(false);
    }


    private void OpenStats()
    {
        mainPanel.SetActive(false);
        statsPanel.SetActive(true);
        UpdateStats();
    }

    private void CloseStats()
    {
        mainPanel?.SetActive(true);
        statsPanel.SetActive(false);
    }

    private void OpenScrapbook()
    {
        mainPanel.SetActive(false);
        scrapbookPanel.SetActive(true);
    }

    private void CloseScrapbook()
    {
        mainPanel?.SetActive(true);
        scrapbookPanel.SetActive(false);
    }

    private void SBNextPage()
    {

    }

    private void SBPrevPage()
    {

    }

    void GetStats()
    {
        counterDictionary = playerStats.SendStats();
        Debug.Log("MenuGotStats");
    }

    void UpdateStats()
    {
        foreach(var stat in statTextPrefabs)
        {
            Destroy(stat.gameObject);
        }
        foreach(var item in counterDictionary)
        {
            
            var text =Instantiate(counterTextPrefab);
            text.transform.SetParent(statBox, false);
            text.statName = item.Key;
            text.statNumber = item.Value;
            statTextPrefabs.Add(text);
            Debug.Log(item.Key + item.Value);

        }
    }
}
