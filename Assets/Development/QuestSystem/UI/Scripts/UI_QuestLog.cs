using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UI_QuestLog : MonoBehaviour
{
    [SerializeField] private string currentQuestID;
    private QuestRuntimeInstance currentQuest;
    [SerializeField] private QuestLog questLog;
    [SerializeField]private List<QuestRuntimeInstance> instanceList = new();
    [SerializeField] private UI_QuestEntry entryPrefab;
    [SerializeField] private UI_QuestLogEntryObjectives objectiveEntryPrefab;


    [Header("MainQuestBox")]
    [SerializeField] private ScrollRect mainQuestBox;
    [SerializeField] private List<UI_QuestEntry> mainQuestButtons = new();


    [Header("SideQuestBox")]
    [SerializeField] private ScrollRect sideQuestBox;
    [SerializeField] private List<UI_QuestEntry> sideQuestButtons = new();

    [Header("QuestInfoPanel")]
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private ScrollRect objectivesBox;
    [SerializeField] private Image reward1;
    [SerializeField] private Image reward2;
    [SerializeField] private Image reward3;
    [SerializeField] private Image reward4;
    [SerializeField] private Button trackQuestButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questLog = FindFirstObjectByType<QuestLog>();
        foreach(var quest in questLog.activeQuests)
        {
            instanceList.Add(quest);
        }
        GetCurrentQuests();
        trackQuestButton.onClick.AddListener(()=>TrackQuest(currentQuest));
    }

    public void TrackQuest(QuestRuntimeInstance questID)
    {
        questLog.TrackQuest(questID);
    }

    public void ShowQuestDetails(string questID)
    {
        currentQuestID = questID;
        UpdateQuestInfoPanel(currentQuestID);
    }

    public void UpdateQuestInfoPanel(string questID)
    {
        questLog.activeQuests.ForEach(quest => { if (quest.questID == questID) { currentQuestID = quest.questID; currentQuest = quest; } });
        questNameText.SetText(currentQuest.questData.questName);
        questDescriptionText.SetText(currentQuest.questData.questLogDescription);
        UpdateCurrentObjectives(currentQuest);
        int trinkets = 0;
        int exp = 0;
        foreach (var item in currentQuest.questData.stages)
        {
            trinkets += item.trinketReward;
            exp += item.expReward;
        }
    }

    private void UpdateCurrentObjectives(QuestRuntimeInstance quest)
    {
        float offset=-50f;
        List<string> strings = new List<string>();
        foreach (StageDetails item in quest.questData.stages)
        {
            foreach(ObjectiveDetails objectives in item.objectivesToComplete)
            {
                var entry = Instantiate(objectiveEntryPrefab);
                entry.transform.SetParent(objectivesBox.content.transform, false);
                entry.transform.localPosition += new Vector3(0, offset, 0);
                offset -= 50f;
                entry.description = objectives.objectiveDescription;
                entry.quantity = objectives.quantityToComplete;
            }
        }
    }

    public void GetCurrentQuests()
    {
        foreach (QuestRuntimeInstance item in instanceList)
        {
            var tempQuestData = item.questData;
            if(tempQuestData != null&& tempQuestData.isMainQuest)
            {
                var button = Instantiate(entryPrefab);
                mainQuestButtons.Add(button);
                button.questName = tempQuestData.questName;
                button.questID = tempQuestData.questID;
                button.log = this;
                AddToQuestBoxes(true, button);
            }else if(tempQuestData != null && !tempQuestData.isMainQuest)
            {
                var button = Instantiate(entryPrefab);
                sideQuestButtons.Add(button);
                button.questName = tempQuestData.questName;
                button.questID = tempQuestData.questID;
                button.log = this;
                AddToQuestBoxes(true, button);
            }


        }
    }

    private void AddToQuestBoxes(bool isMain, UI_QuestEntry button)
    {
        if (isMain)
        {
            button.transform.SetParent(mainQuestBox.transform, false);
        }
        else
        {
            button.transform.SetParent(sideQuestBox.transform, false);
        }
        Debug.Log("Added To Box");
    }

    public void CloseQuestLog()
    {
        Destroy(this.gameObject);
    }

}
