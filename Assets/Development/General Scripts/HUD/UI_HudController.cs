using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_HudController : MonoBehaviour
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
    [SerializeField] private float health => playerStats.GetStat(StatInfo.stats.Health);
    [SerializeField] private float currentHealth;
    [SerializeField] private float startHealth;
    [SerializeField] private StaminaSystem staminaComp;
    [SerializeField] private float stamina => playerStats.GetStat(StatInfo.stats.Stamina);
    [SerializeField] private float currentStamina;
    [SerializeField] private float startStamina;
    [SerializeField] private PoopSystem poopComp;
    [SerializeField] private float poop => playerStats.GetStat(StatInfo.stats.PoopAmount);
    [SerializeField] private float currentPoop;
    [SerializeField] private float startPoop;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = hawkTimer;
        playerRef = FindFirstObjectByType<PlayerHealth>().gameObject;
        healthComp = playerRef.GetComponent<PlayerHealth>();
        staminaComp = playerRef.GetComponent<StaminaSystem>();
        poopComp = playerRef.GetComponent<PoopSystem>();
        startHealth = healthComp.maxHealth;
        currentHealth = healthComp.currentHealth;
        startStamina = staminaComp.GetCurrentStamina();
        currentStamina = staminaComp.GetCurrentStamina();
        startPoop = poopComp.GetMaxPoop();
        currentPoop = poopComp.GetCurrentPoop();
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth !=  healthComp.currentHealth) { currentHealth = healthComp.currentHealth; UpdateHealth(); } 
        if(currentStamina != staminaComp.GetCurrentStamina()) { currentStamina = staminaComp.GetCurrentStamina(); UpdateStamina(); }
        if (currentPoop != poopComp.GetCurrentPoop()) { currentPoop = poopComp.GetCurrentPoop(); UpdatePoop(); }
        {
            
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
}
