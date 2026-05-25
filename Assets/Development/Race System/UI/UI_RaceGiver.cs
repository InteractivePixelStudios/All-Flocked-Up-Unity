using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UI_RaceGiver : MonoBehaviour
{
    [SerializeField] private Button acceptRaceButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private TextMeshProUGUI raceNameText;
    [SerializeField] private TextMeshProUGUI raceTimeText;

    [SerializeField] private RaceBase raceBase;
    [SerializeField] private UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        raceBase = FindAnyObjectByType<RaceBase>();
        acceptRaceButton.onClick.AddListener(AcceptRace);
        cancelButton.onClick.AddListener(CancelRace);
        GetRaceInfo();
        EventSystem.current.SetSelectedGameObject(acceptRaceButton.gameObject);
    }

    // Update is called once per frame
    private void GetRaceInfo()
    {
        Debug.Log("RaceInfo");
        LocalizedString localizedString = new LocalizedString
        {
            TableReference = "AFU_Races",
            TableEntryReference = raceBase.raceData.name
        };

        localizedString.GetLocalizedStringAsync().Completed += handle =>
        {
            raceNameText.SetText(handle.Result);
        };
        LocalizedString localizedDescString = new LocalizedString
        {
            TableReference = "AFU_Races",
            TableEntryReference = raceBase.raceData.raceDescription.GetLocalizedString()
        };

        localizedDescString.GetLocalizedStringAsync().Completed += handle =>
        {
            raceTimeText.SetText(handle.Result);
        };
        // raceTimeText.SetText(raceBase.raceData.raceTime.ToString());
    }

    private void AcceptRace()
    {
        Debug.Log("RaceAccept");
        canvasController.CloseRaceGiver();
        raceBase.InteractWithRaceGiver();
    }

    private void CancelRace()
    {
        canvasController.CloseRaceGiver();
    }

    public void CloseRaceGiver()
    {
        Destroy(this.gameObject);

    }
}
