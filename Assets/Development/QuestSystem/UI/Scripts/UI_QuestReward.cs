using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestReward : MonoBehaviour
{

    [SerializeField] GameObject questRewardCanvas;
    [SerializeField] Button acceptReward;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI rewardText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceptReward.onClick.AddListener(AcceptReward);

    }

    public void OpenQuestRewardsUI()
    {
        questRewardCanvas.SetActive(true);
    }
    public void AcceptReward()
    {
        Destroy(questRewardCanvas);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetQuestNameText(string name)
    {
        questName.SetText(name);
    }

    public void SetRewardText(string reward)
    {
        rewardText.SetText(reward);
    }
}
