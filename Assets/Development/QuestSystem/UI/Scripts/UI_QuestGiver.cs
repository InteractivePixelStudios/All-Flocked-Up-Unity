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

    [SerializeField] TextMeshProUGUI questNameText;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] TextMeshProUGUI rewardsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questLog = FindFirstObjectByType<QuestLog>();
        acceptQuestButton.onClick.AddListener(AddQuestToLog);
        cancelButton.onClick.AddListener(CloseQuestGiverUI);
        
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
        Destroy(this.gameObject);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void AddQuestToLog()
    {
      currentquestGiver.InteractWithNPC(questLog);
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
