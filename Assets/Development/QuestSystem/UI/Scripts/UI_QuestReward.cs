using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_QuestReward : MonoBehaviour
{

    [SerializeField] GameObject questRewardCanvas;
    [SerializeField] Button acceptReward;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI trinketText;
    [SerializeField] Image itemImage;
    [SerializeField] List<Sprite> itemSprites = new();
    public UI_CanvasController canvasController;
    public QuestDetails quest;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceptReward.onClick.AddListener(AcceptReward);
        SetQuestNameText(quest.questName);
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

    public void SetRewardText(int trinket, string[] reward)
    {
        trinketText.SetText(trinket.ToString());
        if(reward != null)
        {
            itemImage.sprite = FindItemSprite(reward);
        }
    }

    Sprite FindItemSprite(string[] rewards)
    {
        itemImage.gameObject.SetActive(false);
        foreach (var reward in rewards)
        {
            foreach (var item in itemSprites)
            {
                if (item.name.Equals(reward))
                {
                    Sprite found = item;
                    itemImage.gameObject.SetActive(true);
                    return found;
                }
                else continue;

            }
             return null;

        }
        return null;
    }
}
