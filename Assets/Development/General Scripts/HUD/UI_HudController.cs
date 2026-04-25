using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_HudController : Singleton<UI_HudController>
{
    GameObject playerRef;
    [SerializeField] private StatInfo playerStats;
    [SerializeField] private float hawkTimer = 5f;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isWarningShown;
    public bool readyToSpawn;

    [SerializeField] private GameObject warningObj;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image warningImage;
    private float fadeInAlpha = 200f;

    [SerializeField] private List<Sprite> healthImages = new();
    [SerializeField] private Image shownHealthImage;
    [SerializeField] private Image staminaBarImage;
    [SerializeField] private Image poopBarImage;

    [SerializeField] private PlayerHealth healthComp;
    private float health => playerStats.GetStat(StatInfo.stats.Health);
    [SerializeField] private float currentHealth;
    [SerializeField] private float startHealth;
    [SerializeField] private StaminaSystem staminaComp;
    private float stamina => playerStats.GetStat(StatInfo.stats.Stamina);
    [SerializeField] private float currentStamina;
    [SerializeField] private float startStamina;
    [SerializeField] private PoopSystem poopComp;
    private float poop => playerStats.GetStat(StatInfo.stats.PoopAmount);
    [SerializeField] private float currentPoop;
    [SerializeField] private float startPoop;

    [SerializeField] private Image levelUpIcon;
    [SerializeField] private EXPSystem expComp;
    [SerializeField] private int cachedLevel;
    float iconTimer = 5f;
    float fadeTimer = 2f;
    float visibleTime = 2f;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject camPanel;
    [SerializeField] private GameObject reticle;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        InitHUD();
    }

    void InitHUD()
    { 
        currentTime = hawkTimer;
        playerRef = FindAnyObjectByType<PlayerHealth>().gameObject;
        healthComp = playerRef.GetComponent<PlayerHealth>();
        staminaComp = playerRef.GetComponent<StaminaSystem>();
        expComp = playerRef.GetComponent<EXPSystem>();
        cachedLevel = expComp.PLAYERLEVEL;
        poopComp = playerRef.GetComponent<PoopSystem>();
        startHealth = healthComp.maxHealth;
        currentHealth = healthComp.currentHealth;
        currentStamina = staminaComp.GetCurrentStamina();
        startStamina = staminaComp.GetMaxStamina();
        startPoop = poopComp.GetMaxPoop();
        currentPoop = poopComp.GetCurrentPoop();
        UpdateHealth();
        HideIcon();
        HideReticle();
    }

    public void ShowReticle()
    {
        reticle.SetActive(true);
    }

    public void HideReticle()
    {
        reticle.SetActive(false);
    }

    void HideIcon()
    {
        var color = levelUpIcon.color;
        color.a = 0f;
        levelUpIcon.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth !=  healthComp.currentHealth) { currentHealth = healthComp.currentHealth; UpdateHealth(); } 
        if(currentStamina != staminaComp.GetCurrentStamina()) { currentStamina = staminaComp.GetCurrentStamina(); UpdateStamina(); }
        if (currentPoop != poopComp.GetCurrentPoop()) { currentPoop = poopComp.GetCurrentPoop(); UpdatePoop(); }
        if (expComp.PLAYERLEVEL != cachedLevel)
        {
            cachedLevel = expComp.PLAYERLEVEL;

            iconTimer = visibleTime + fadeTimer;

            var color = levelUpIcon.color;
            color.a = 1f;          // show instantly
            levelUpIcon.color = color;
        }
        if (iconTimer > 0f)
        {
            iconTimer -= Time.deltaTime;

            if (iconTimer <= fadeTimer)
            {
                var color = levelUpIcon.color;

                float time = 1f - (iconTimer / fadeTimer);
                color.a = Mathf.Lerp(1f, 0f, time);

                levelUpIcon.color = color;
            }
        }

        if (isWarningShown)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null)
            {
                timerText.SetText(currentTime.ToString());
            }
            if (currentTime < 0)
            {
                readyToSpawn = true;
                timerText.SetText("!");

            }
            else
            {

                readyToSpawn = false;
                ShowHawkWarning();
            }
        }
    }

    private void UpdateHealth()
    {

        if(currentHealth == startHealth)
        {
            shownHealthImage.sprite = healthImages[0];
         }
        else if (currentHealth == startHealth * 0.8)
        {
            shownHealthImage.sprite = healthImages[1];
        }
        else if (currentHealth == startHealth * 0.6)
        {
            shownHealthImage.sprite = healthImages[2];
        }
        else if (currentHealth == startHealth * 0.4)
        {
            shownHealthImage.sprite = healthImages[3];
        }
        else if (currentHealth == startHealth * 0.2)
        {
            shownHealthImage.sprite = healthImages[4];
        }
        else if (currentHealth == 0)
        {
            shownHealthImage.sprite = healthImages[5];
        }


    }

    private void UpdateStamina()
    {
        staminaBarImage.fillAmount = currentStamina/startStamina;
    }

    private void UpdatePoop()
    {
        poopBarImage.fillAmount = currentPoop/startPoop;
    }

    public void ShowHawkWarning()
    {
        isWarningShown = true;
        warningObj.SetActive(true);
    }

    public void HideHawkWarning()
    {
        isWarningShown = false;
        currentTime = hawkTimer;
        warningObj?.SetActive(false);
    }

    public void ShowLevelUpIcon()
    {

        iconTimer = visibleTime + fadeTimer;

        var color = levelUpIcon.color;
        color.a = 1f;
        levelUpIcon.color = color;

    }

    private void OnLevelWasLoaded(int level)
    {
        InitHUD();
    }

    public void ToggleCameraOverlay(bool isOn)
    {
        if (isOn)
        {
            camPanel.SetActive(true);
        }else camPanel.SetActive(false);
    }

    public void ToggleMainHUD(bool isOn)
    {
        if (isOn)
        {
            mainPanel.SetActive(true);
        }
        else mainPanel.SetActive(false);
    }
}
