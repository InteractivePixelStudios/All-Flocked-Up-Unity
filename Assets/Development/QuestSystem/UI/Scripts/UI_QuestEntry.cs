using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UI_QuestEntry : MonoBehaviour
{
    private Button button;
    public LocalizedString questName;
    public string questID;
    public UI_QuestLog log;

    private void Start()
    {
        button = GetComponent<Button>();
        TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();

        questName.StringChanged += value =>
        {
            label.text = value;
        };
        button.onClick.AddListener(SelectQuest);
    }

    private void SelectQuest()
    {
        log.ShowQuestDetails(questID);
    }

}
