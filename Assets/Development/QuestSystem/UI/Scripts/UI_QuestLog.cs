using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Unity.VisualScripting;

public class UI_QuestLog : MonoBehaviour
{
    [SerializeField] private string currentQuestID;
    [SerializeField] private QuestLog questLog;
    private QuestRuntimeInstance[] instanceArray;


    [Header("MainQuestBox")]
    [SerializeField] private ScrollRect mainQuestBox;
    [SerializeField] private Button[] mainQuestButtons;


    [Header("SideQuestBox")]
    [SerializeField] private ScrollRect sideQuestBox;
    [SerializeField] private Button[] sideQuestButtons;

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
        questLog.activeQuests.CopyTo(instanceArray); 
        GetCurrentQuests();
        trackQuestButton.onClick.AddListener(()=>TrackQuest(currentQuestID));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrackQuest(string questID)
    {

    }

    public void ShowQuestDestails(string questID)
    {
        currentQuestID = questID;
    }

    public void UpdateQuestInfoPanel(string questID)
    {
        
    }

    public void GetCurrentQuests()
    {
        foreach (QuestRuntimeInstance item in instanceArray)
        {
            var tempQuestData = item.questData;
            questNameText.SetText(tempQuestData.questName);
            questDescriptionText.SetText(tempQuestData.questLogDescription);
            objectivesBox.content.AddComponent<TextMeshProUGUI>().SetText(tempQuestData.questID);//change me


        }
    }

    public void CloseQuestLog()
    {
        Destroy(this.gameObject);
    }

}
