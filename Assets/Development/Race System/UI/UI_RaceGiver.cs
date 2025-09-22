using TMPro;
using UnityEngine;
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
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        raceBase = FindFirstObjectByType<RaceBase>();
        acceptRaceButton.onClick.AddListener(AcceptRace);
        cancelButton.onClick.AddListener(CancelRace);
        GetRaceInfo();
    }

    // Update is called once per frame
    private void GetRaceInfo()
    {
        Debug.Log("RaceInfo");
        raceNameText.SetText(raceBase.raceData.raceName);
        raceTimeText.SetText(raceBase.raceData.raceTime.ToString());
    }

    private void AcceptRace()
    {
        Debug.Log("RaceAccept");
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
