using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestGiver : MonoBehaviour
{

    [SerializeField] GameObject questGiverCanvas;
    public QuestGiver currentquestGiver;
    public QuestLog questLog;

    [SerializeField] Button acceptQuestButton;
    [SerializeField] Button cancelButton;
    public UI_CanvasController canvasController;

    [SerializeField] TextMeshProUGUI questNameText;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] TextMeshProUGUI rewardsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questLog = FindFirstObjectByType<QuestLog>();
        acceptQuestButton.onClick.AddListener(AddQuestToLog);
        cancelButton.onClick.AddListener(CloseQuestGiverUI);
        Debug.Log(currentquestGiver.quests[0].ToString());
        
    }

    public void UpdateUIText(string name, string description, string rewards)
    {
        SetQuestNameText(name);
        SetQuestDescriptionText(description);
        SetRewardsText(rewards);
    }

    public void OpenQuestGiverUI(QuestGiver questGiver)
    {
        currentquestGiver = questGiver;
        if (questLog.hasQuest == true) { return; }
        
    }

    public void CloseQuestGiverUI()
    {
        canvasController.DestroyQuestGiver();

    }

    private void AddQuestToLog()
    {
        currentquestGiver.AcceptQuest(questLog, currentquestGiver.quests[0],currentquestGiver);
        CloseQuestGiverUI();
    }

    public void SetQuestNameText(string name)
    {
        questNameText.SetText(name);
    }

    public void SetQuestDescriptionText(string description)
    {
        questDescription.SetText(description);
    }

    public void SetRewardsText(string rewards)
    {
        rewardsText.SetText(rewards);
    }
}
