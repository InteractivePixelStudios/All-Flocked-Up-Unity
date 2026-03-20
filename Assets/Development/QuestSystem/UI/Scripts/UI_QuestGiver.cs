using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.EventSystems;

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
        questLog = FindAnyObjectByType<QuestLog>();
        acceptQuestButton.onClick.AddListener(AddQuestToLog);
        cancelButton.onClick.AddListener(CloseQuestGiverUI);
        Debug.Log(currentquestGiver.quests[0].ToString());
        EventSystem.current.SetSelectedGameObject(acceptQuestButton.gameObject);
        
    }

    public void UpdateUIText(LocalizedString name, LocalizedString description, LocalizedString rewards)
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

    public void SetQuestNameText(LocalizedString name)
    {
        name.StringChanged += value => questNameText.text = value;
    }

    public void SetQuestDescriptionText(LocalizedString description)
    {
        description.StringChanged += value => questDescription.text = value;
    }

    public void SetRewardsText(LocalizedString rewards)
    {
        rewards.StringChanged += value => rewardsText.text = value;
    }
}
