using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Localization;

public class UI_QuestLog : MonoBehaviour
{
    [SerializeField] private string currentQuestID;
    private QuestRuntimeInstance currentQuest;
    [SerializeField] private QuestLog questLog;
    [SerializeField]private List<QuestRuntimeInstance> instanceList = new();
    [SerializeField] private UI_QuestEntry entryPrefab;
    [SerializeField] private UI_SideQuestEntry sideEntryPrefab;
    [SerializeField] private UI_QuestLogEntryObjectives objectiveEntryPrefab;


    [Header("MainQuestBox")]
    [SerializeField] private ScrollRect mainQuestBox;
    [SerializeField] private List<UI_QuestEntry> mainQuestButtons = new();


    [Header("SideQuestBox")]
    [SerializeField] private ScrollRect sideQuestBox;
    [SerializeField] private List<UI_SideQuestEntry> sideQuestButtons = new();

    [Header("QuestInfoPanel")]
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private ScrollRect objectivesBox;
    [SerializeField] private Image reward1;
    [SerializeField] private Image reward2;
    [SerializeField] private Image reward3;
    [SerializeField] private Image reward4;
    [SerializeField] private Button trackQuestButton;

    LocalizedString boundQuestName;
    LocalizedString boundQuestDescription;
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
        HideRewardImages(currentQuest);
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

    void BindLocalizedText(
    ref LocalizedString current,
    TextMeshProUGUI label,
    LocalizedString loc)
    {
        if (current != null)
            current.StringChanged -= OnStringChanged;

        current = loc;
        current.StringChanged += OnStringChanged;

        void OnStringChanged(string value)
        {
            label.text = value;
        }
    }

    public void UpdateQuestInfoPanel(string questID)
    {
        questLog.activeQuests.ForEach(quest => { if (quest.questID == questID) { currentQuestID = quest.questID; currentQuest = quest; } });
        BindLocalizedText(
            ref boundQuestName,
            questNameText,
            currentQuest.questData.questName
        );

        BindLocalizedText(
            ref boundQuestDescription,
            questDescriptionText,
            currentQuest.questData.questLogDescription
        );
        UpdateCurrentObjectives(currentQuest);
        int trinkets = 0;
        int exp = 0;
        foreach (var item in currentQuest.questData.stages)
        {
            trinkets += item.trinketReward;
            exp += item.expReward;
        }
        ShowRewardImages(currentQuest);
    }

    private void UpdateCurrentObjectives(QuestRuntimeInstance quest)
    {
        float offset=-50f;
        List<LocalizedString> strings = new List<LocalizedString>();
        foreach (StageDetails item in quest.questData.stages)
        {
            foreach(ObjectiveDetails objectives in item.objectivesToComplete)
            {
                var entry = Instantiate(objectiveEntryPrefab,objectivesBox.content.transform.position,objectivesBox.content.transform.rotation );
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
                AddToQuestBox(button);
            }else if(tempQuestData != null && !tempQuestData.isMainQuest)
            {
                var button = Instantiate(sideEntryPrefab);
                sideQuestButtons.Add(button);
                button.questName = tempQuestData.questName;
                button.questID = tempQuestData.questID;
                button.log = this;
                AddToSideQuestBox(button);
            }


        }
    }

    private void AddToQuestBox( UI_QuestEntry button)
    {

            button.transform.SetParent(mainQuestBox.transform, false);

        Debug.Log("Added To Box");
    }

    private void AddToSideQuestBox(UI_SideQuestEntry button)
    {
        button.transform.SetParent(sideQuestBox.transform, false);
    }

    public void CloseQuestLog()
    {
        Destroy(this.gameObject);
    }

    private void HideRewardImages(QuestRuntimeInstance quest)
    {
        if (quest.questData.itemRewards[0] != null) reward1.enabled = false;
        if (quest.questData.itemRewards[1] != null) reward2.enabled = false;
        if (quest.questData.itemRewards[2] != null) reward3.enabled = false;
        if (quest.questData.itemRewards[3] != null) reward4.enabled = false;
        else
        {
            reward1.enabled = false;
            reward2.enabled = false;
            reward3.enabled = false;
            reward4.enabled = false;
        }

    }

    private void ShowRewardImages(QuestRuntimeInstance quest)
    {
        if (quest.questData.itemRewards[0]!=null) reward1.enabled = true;
        if (quest.questData.itemRewards[1] != null) reward2.enabled = true;
        if (quest.questData.itemRewards[2] != null) reward3.enabled = true;
        if (quest.questData.itemRewards[3] != null) reward4.enabled = true;
        else
        {
            reward1.enabled = true;
            reward2.enabled = true;
            reward3.enabled = true;
            reward4.enabled = true;
        }
    }

}
