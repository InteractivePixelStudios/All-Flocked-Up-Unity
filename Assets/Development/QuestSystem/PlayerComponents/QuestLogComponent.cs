using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public List<QuestRuntimeInstance> activeQuests = new();
    public List<QuestDetails> completedQuests = new();
    [SerializeField] private UI_QuestNotif questNotif;
    [SerializeField] private UI_QuestReward questRewardUI;
    public bool hasQuest = false;
    public QuestGiver currentQuestGiver;
    public QuestLogMenuEvents logMenuEvents;
    private bool questTimerStarted;
    public float currentTime;
    [SerializeField] private UI_CanvasController canvasController;

    //checks if quest in timed and shows the timer when  quest started
    private void GetIsQuestTimed(QuestDetails quest, QuestRuntimeInstance instance)
    {
        if (quest.isQuestTimed)
        {
            StartQuestTimer(quest.questTime, instance);
            canvasController.ShowTimer();
        }
    }
    //starts the timer
    private void StartQuestTimer(float questTime, QuestRuntimeInstance instance)
    {
        currentTime = questTime;
        questTimerStarted = true;

    }
    //checks if timer = 0 and updates time... call for if quest failed to remove quest from activeQuest list 
    private void CheckTimerState(QuestRuntimeInstance quest)
    {

            if (currentTime == 0f)
            {
                activeQuests.Remove(quest);

                canvasController.EndTimer();
                //quest.QuestFailed();
                //OnQuestFailed(quest);
                //doesn't remove when timer is done?
        }
            else
            {
                Debug.Log("Quest Not Timed");
            }
        } 

    private void Update()
    {
        //needs to check for if quest is completed before timer done
        if (questTimerStarted == true&& currentTime>0) { 
        currentTime -= Time.deltaTime;
            if(currentTime == 0f)
            {
                //chnage later...this can only check against first index.... probably will add timer variables to questdetails so multiple quests can be timed at once or make an enum for quest states
                //Active/Completed/Failed...
                CheckTimerState(activeQuests[0]);
                canvasController.EndTimer();
                
                
            }
            else
            {
                //Debug.Log(currentTime);
            }
    }
    }


    //if playuer doesnt have quest already it creates QuestRuntimeInstance from QuestData and calls StartQuest() in that instance
    public void AcceptQuest(QuestDetails questData,QuestGiver questGiver)
    {
        if (hasQuest) { return; }
        if (HasQuestOrCompleted(questData))
        {
            Debug.LogWarning($"Quest '{questData.questName}' already accepted or completed.");
            return;
        }

        QuestRuntimeInstance instance = new()
        {
            questData = questData,
            questID = questData.questID
            
        };
        instance.StartQuest();
        activeQuests.Add(instance);
        hasQuest = true;
        currentQuestGiver = questGiver;
        GetIsQuestTimed(questData, instance);
        
    }
    //updates the quest Objective... call this on quest mechanics or anytime you want to complete an objective... send the objectiveID and number of times completed (usually 1 but can be other if needed)
    //otherwise checks if quest is completed
    public void UpdateQuestObjective(string objectiveID, int amount)
    {
        foreach (var quest in activeQuests)
        {
            //throws an error when a quest is complete...stupid
                quest.UpdateObjective(objectiveID, amount);
            
        }

        CheckForCompletedQuests();
    }

    //shows the quest notif when an objective is completed
    public void OnObjectiveUpdated(QuestRuntimeInstance quest, string objectiveID, int newValue)
    {
        // purely update UI / notify player
        canvasController.ShowQuestNotif("Objective Complete");
        
        Debug.Log($"Quest {quest.questData.questName} objective {objectiveID} progress: {newValue}");
    }

    //checks through activeQuests for completed quest and shows rewards.Removed from activeQuests and added to completedQuests list... also destroys the questgiver component form the NPC so it remains just a dialogue NPC
    public void CheckForCompletedQuests()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            if (activeQuests[i].IsComplete)
            {
                completedQuests.Add(activeQuests[i].questData);
                activeQuests.RemoveAt(i);
                hasQuest = false;
                canvasController.ShowQuestReward();
                currentQuestGiver.gameObject.layer = LayerMask.NameToLayer("Dialogue");
                Debug.Log(currentQuestGiver.gameObject.layer);
                Destroy(currentQuestGiver);
                canvasController.EndTimer();

            }
        }
    }

    //checks if player has quest and returns bool
    public bool HasQuest(QuestDetails quest)
    {
        return activeQuests.Exists(q => q.questData == quest);
    }

    //checks if quest is in completedQuests list
    public bool HasCompleted(QuestDetails quest)
    {
        return completedQuests.Contains(quest);
    }

    //checks both above functions at the same time
    public bool HasQuestOrCompleted(QuestDetails quest)
    {
        return HasQuest(quest) || HasCompleted(quest);
    }

    //returns bool for if quest is completed
    public bool IsQuestCompleted(QuestDetails quest)
    {
        QuestRuntimeInstance instance = GetQuestInstance(quest);
        return instance != null && instance.IsComplete;
    }

    //returns the current quest instance (runtime instance)
    public QuestRuntimeInstance GetQuestInstance(QuestDetails quest)
    {
        return activeQuests.Find(q => q.questData == quest);
    }
    //marks the quest turned in as adds to completedQuest list
    public void MarkQuestTurnedIn(QuestDetails quest)
    {
        var questInstance = GetQuestInstance(quest);
        if (questInstance != null)
        {
            activeQuests.Remove(questInstance);
            if (!completedQuests.Contains(quest))
                completedQuests.Add(quest);


        }
    }

    public void OnQuestFailed(QuestRuntimeInstance quest)
    {
        if (quest != null)
        {
            activeQuests.Remove(quest);
            
            
        }
    }
}
