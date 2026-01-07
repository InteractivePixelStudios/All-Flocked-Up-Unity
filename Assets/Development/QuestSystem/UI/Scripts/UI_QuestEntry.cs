using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestEntry : MonoBehaviour
{
    private Button button;
    public string questName;
    public string questID;
    public UI_QuestLog log;

    private void Start()
    {
        button = GetComponent<Button>();
        button.GetComponentInChildren<TextMeshProUGUI>().SetText(questName);
        button.onClick.AddListener(SelectQuest);
    }

    private void SelectQuest()
    {
        log.ShowQuestDetails(questID);
    }

}
