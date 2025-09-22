using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

public class QuestLogMenuEvents : MonoBehaviour
{

    [SerializeField] private UIDocument questLogDocument;
    [SerializeField] private QuestLog questLogComponent;

    [SerializeField] private Label questTitleLabel;
    [SerializeField] private Label questDescriptionLabel;
    [SerializeField] private Label questRewardsLabel;

    [SerializeField] private ScrollView mainQuestBox;
    [SerializeField] private ScrollView sideQuestBox;

    private Dictionary<string, Button> currentActiveQuests = new();
    public string tempName;





    private void Awake()
    {
        GetQuestLog();

        questTitleLabel = questLogDocument.rootVisualElement.Q("QuestName") as Label;
        questDescriptionLabel = questLogDocument.rootVisualElement.Q("QuestDescription") as Label;
        questRewardsLabel = questLogDocument.rootVisualElement.Q("QuestRewards") as Label;

        mainQuestBox = questLogDocument.rootVisualElement.Q<ScrollView>("MainQuestBox") as ScrollView;
        sideQuestBox = questLogDocument.rootVisualElement.Q<ScrollView>("SideQuestBox") as ScrollView;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetCurrentQuests();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void GetQuestLog()
    {
        questLogComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestLog>();
    }

    [ContextMenu("Get Current Quests")]
    public void GetCurrentQuests()
    {
        GetQuestLog();
        mainQuestBox.Clear();
        currentActiveQuests.Clear();

        foreach(var quest in questLogComponent.activeQuests)
        {
            if (!currentActiveQuests.ContainsKey(quest.questData.questName))
            {
                //this creates a new button and automatically assigns an onClick handler****
                var localQuest = quest;
                tempName = quest.questData.questName;
                Button newQuestButton = new Button(() =>
                {
                    questTitleLabel.text = localQuest.questData.questName;
                    questDescriptionLabel.text = localQuest.questData.questLogDescription;
                    Debug.Log(localQuest.questData.questName);

                })
                { text = tempName };

                newQuestButton.AddToClassList("questButton");
                AddQuest(tempName, newQuestButton);
                Debug.Log("Button Spawn");

            }
        }
    }

    public void AddQuest(string questName,Button newQuestButton)
    {

        currentActiveQuests.Add(questName, newQuestButton);
        mainQuestBox.Add(newQuestButton);
        Debug.Log(newQuestButton);
        newQuestButton.style.width = Length.Percent(100);
        newQuestButton.style.height = 40;

    }

    public void RemoveQuest(string questName)
    {

        if(currentActiveQuests.TryGetValue(questName, out var button))
        {
            currentActiveQuests.Remove(questName);
            mainQuestBox.Remove(button);
        }
    }
}
