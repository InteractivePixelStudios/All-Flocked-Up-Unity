using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_RaceReward: MonoBehaviour
{
    [SerializeField] private Button acceptRewardButton;


    [SerializeField] private TextMeshProUGUI raceNameText;
    [SerializeField] private TextMeshProUGUI raceRewardText;
    [SerializeField] private Image newRecordImage;

    [SerializeField] private RaceBase raceBase;
    public Dictionary<GameObject, float> racerList = new();

    [SerializeField] private ScrollRect nameTextBox;
    [SerializeField] private ScrollRect timeTextBox;

    [SerializeField] private float currentY = 0f;

    [SerializeField] private GameObject textPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newRecordImage.gameObject.SetActive(false);
        acceptRewardButton.onClick.AddListener(AcceptReward);
        raceBase = FindFirstObjectByType<RaceBase>();
        GetReward();
        UpdateStandings();
        UpdateRaceBestTime();

    }

    public void GetRaceStandings(GameObject racer,float time)
    {
        if (!racerList.ContainsKey(racer))
        {
            racerList.Add(racer, time);
            Debug.Log($"Added {racer.name} to dictionary");
        }
    }

    private void UpdateStandings()
    {
        float startY = -40f;
        float offset = racerList.Count > 3 ? 40f : 20f;

        foreach (var racer in racerList)
        {
            Debug.Log(racer.Key.name);
            CreateRacerText(racer.Key.name, startY, offset);
            CreateTimeText(racer.Value.ToString(), startY, offset);
            startY -= offset;
        }
    }

    // Update is called once per frame
    public void GetReward()
    {
            raceNameText.SetText(raceBase.raceData.raceName);
            raceRewardText.SetText(raceBase.raceData.raceRewards.ToString()); 
    }
    //compares best race time with given and updates if faster
    private void UpdateRaceBestTime()
    {
        if(racerList.ElementAt(0).Value >= raceBase.recordTime)
        {
            raceBase.recordTime = racerList.ElementAt(0).Value;
            newRecordImage.gameObject.SetActive(true);
        }
    }

    //gives rewards to player
    private void AcceptReward()
    {
        raceBase.completedRaces.Add(raceBase.raceData);
        Destroy(raceBase.currentRaceGiver.GetComponent<RaceGiver>());
        GiveRaceRewards();
        Destroy(this.gameObject);
        raceBase.StartPlayerMove();
    }

    private TextMeshProUGUI CreateRacerText(string text, float startY, float offset)
    {
        GameObject textObject = Instantiate(textPrefab, nameTextBox.content);
        TextMeshProUGUI name = textObject.GetComponent<TextMeshProUGUI>();
        RectTransform textTransform = textObject.GetComponent<RectTransform>();
        SetRacerTextTransform(textTransform, startY,offset);

        SetRacerTextTransform(textTransform, startY, offset);
        SetRacerText(name, text);

        return name;
    }

    private void SetRacerTextTransform(RectTransform transform, float startY, float offset)
    {
        transform.anchorMin = new Vector2(0.5f, 1);
        transform.anchorMax = new Vector2(0.5f, 1);
        transform.pivot = new Vector2(0.5f, 1);
        transform.anchoredPosition = new Vector2(0, startY);

    }

    private void SetRacerText(TextMeshProUGUI text, string name)
    {
        var labelText = text;
        if (labelText != null) labelText.SetText(name);
    }

    private TextMeshProUGUI CreateTimeText(string racerTime,float startY, float offset)
    {
        GameObject textObject = Instantiate(textPrefab, timeTextBox.content);
        TextMeshProUGUI timeText = textObject.GetComponent<TextMeshProUGUI>();
        RectTransform textTransform = textObject.GetComponent<RectTransform>();

        SetTimeTextTransform(textTransform, startY, offset);
        SetTimeText(timeText, racerTime);

        return timeText;
    }

    private void SetTimeTextTransform(RectTransform transform, float startY, float offset)
    {
        transform.anchorMin = new Vector2(0.5f, 1);
        transform.anchorMax = new Vector2(0.5f, 1);
        transform.pivot = new Vector2(0.5f, 1);
        transform.anchoredPosition = new Vector2(0, startY);

    }

    private void SetTimeText(TextMeshProUGUI timeText, string time)
    {
        var labelText = timeText.GetComponentInChildren<TextMeshProUGUI>();
        if (labelText != null) labelText.SetText(time);
    }

    private void GiveRaceRewards()
    {
        var EXP = FindFirstObjectByType<EXPSystem>();
        EXP.IncrementXP(raceBase.raceData.raceRewards);
    }

}
