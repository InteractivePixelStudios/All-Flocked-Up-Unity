using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
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
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        race = FindAnyObjectByType<RaceBase>();
        retryButton.onClick.AddListener(RetryRace);
        cancelButton.onClick.AddListener(CloseRace);
        GetRequiredTime();
        GetRaceInfo();
        EventSystem.current.SetSelectedGameObject(retryButton.gameObject);
    }
    private void GetRequiredTime()
    {
        raceRequiredTime.SetText(race.raceData.raceTime.ToString());
    }

    private void GetRaceInfo()
    {
        LocalizedString localizedString = new LocalizedString
        {
            TableReference = "AFU_Races",
            TableEntryReference = race.raceData.name
        };

        localizedString.GetLocalizedStringAsync().Completed += handle =>
        {
            raceNameText.SetText(handle.Result);
        };

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
        race.StartPlayerMove();
    }
}
