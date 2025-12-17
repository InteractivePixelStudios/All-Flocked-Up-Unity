using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class UI_HudController : MonoBehaviour
{
    [SerializeField] private StatInfo playerStats;
    [SerializeField] private float hawkTimer = 5f;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isWarningShown;
    public bool readyToSpawn;

    [SerializeField] private GameObject warningObj;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image warningImage;
    private float fadeInAlpha = 200f;

    [SerializeField] private float health => playerStats.GetStat(StatInfo.stats.Health);
    [SerializeField] private float stamina => playerStats.GetStat(StatInfo.stats.Stamina);
    [SerializeField] private float poop => playerStats.GetStat(StatInfo.stats.PoopAmount);

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = hawkTimer;
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    private void UpdateStamina()
    {
       
    }

    private void UpdatePoop()
    {

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
