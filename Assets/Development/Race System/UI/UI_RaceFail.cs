using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RaceFail : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private TextMeshProUGUI raceNameText;
    [SerializeField] private TextMeshProUGUI raceStatusText;
    [SerializeField] private TextMeshProUGUI raceRequiredTime;

    [SerializeField] private RaceBase race;
    [SerializeField] private UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        race = FindFirstObjectByType<RaceBase>();
        retryButton.onClick.AddListener(RetryRace);
        cancelButton.onClick.AddListener(CloseRace);
        GetRequiredTime();
        GetRaceInfo();

    }
    private void GetRequiredTime()
    {
        raceRequiredTime.SetText(race.raceData.raceTime.ToString());
    }

    private void GetRaceInfo()
    {
        raceNameText.SetText(race.raceData.raceName);
        raceStatusText.SetText("Race Failed!");
    }

    private void RetryRace()
    {
        Destroy(this.gameObject);
        race.ResetRace();
        
    }

    private void CloseRace()
    {
       Destroy(this.gameObject);
    }
}
