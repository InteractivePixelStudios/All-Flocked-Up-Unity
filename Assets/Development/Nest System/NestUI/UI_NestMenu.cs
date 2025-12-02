using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_NestMenu : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject dayIcon;
    [SerializeField] private GameObject nightIcon;
    [Header("Buttons")]
    [SerializeField] private Button closeMainButton;
    [SerializeField] private Button invButton;
    [SerializeField] private Button sleepButton;
    [SerializeField] private Button closeInvButton;
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
    [SerializeField] private GameObject invPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject scrapbookPanel;
    
    [Header("Components")]
    [SerializeField] private S_DayNightCycle dayNightSystem;
    public UI_CanvasController canvasController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        dayNightSystem = FindFirstObjectByType<S_DayNightCycle>();
        canvasController = FindFirstObjectByType<UI_CanvasController>();
    }
    void Start()
    {
        closeMainButton.onClick.AddListener(CloseNestMenu);
        invButton.onClick.AddListener(OpenNestInventory);
        sleepButton.onClick.AddListener(SleepAtNest);
        closeInvButton.onClick.AddListener(CloseNestInventory);
        statsButton.onClick.AddListener(OpenStats);
        closeStatsButton.onClick.AddListener(CloseStats);
        scrapbookButton.onClick.AddListener(OpenScrapbook);
        closeSbButton.onClick.AddListener(CloseScrapbook);
        sbNextPage.onClick.AddListener(SBNextPage);
        sbPrevPage.onClick.AddListener(SBPrevPage);

    }

    private void Update()
    {
        GetCurrentTime();
    }

    private void GetCurrentTime()
    {
        if(dayNightSystem != null&& dayNightSystem.isDay != true)
        {
            nightIcon.SetActive(true);
            dayIcon.SetActive(false);
        }
        else
        {
            nightIcon.SetActive(false);
            dayIcon.SetActive(true);
        }
    }


    private void CloseNestMenu()
    {
        canvasController.CloseNestMenu();
        Destroy(this.gameObject);
    }

    private void OpenNestInventory()
    {
        mainPanel.SetActive(false);
        invPanel.SetActive(true);
    }

    private void CloseNestInventory()
    {
        mainPanel?.SetActive(true);
        invPanel.SetActive(false);
    }

    private void SleepAtNest()
    {
        dayNightSystem.timeOfDay += 0.5f;
    }


    private void OpenStats()
    {
        mainPanel.SetActive(false);
        statsPanel.SetActive(true);
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
}
