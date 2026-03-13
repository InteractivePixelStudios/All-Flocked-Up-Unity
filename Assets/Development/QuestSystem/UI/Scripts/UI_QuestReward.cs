using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UI_QuestReward : MonoBehaviour
{

    [SerializeField] GameObject questRewardCanvas;
    [SerializeField] Button acceptReward;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI rewardText;
    public UI_CanvasController canvasController;
    public QuestDetails quest;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceptReward.onClick.AddListener(AcceptReward);
        SetQuestNameText(quest.questName);
        SetRewardText(quest.itemRewards.ToString());
        EventSystem.current.SetSelectedGameObject(acceptReward.gameObject);

    }

    public void AcceptReward()
    {
        canvasController.DestroyQuestReward();

    }

    public void SetQuestNameText(LocalizedString name)
    {
        name.StringChanged += value =>
        {
            questName.SetText(value);
        };
    }

    public void SetRewardText(string reward)
    {
        rewardText.SetText(reward);
    }
}
