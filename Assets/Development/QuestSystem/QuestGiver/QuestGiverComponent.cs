using UnityEngine;
using System.Collections.Generic;

public class QuestGiver : MonoBehaviour, IQuestInteraction
{
    //Quest Giver should be attached to any NPC the should give a quest. Also inherits the QuestInteraction interface to use those functions.
    [Header("Quests Offered")]
    public List<QuestDetails> quests = new(); // List of quest the NPCwill give
    public bool offerSequentially = true; // Are quests given in order?
    public bool repeatable = false; //Is Quest repeatable?
    public string questName;
    public bool hasQuest;

    [Header("Prerequisites")]
    public List<QuestDetails> requiredCompletedQuests = new(); //List of REQUIRED COMPLETED QUESTS.



    // *** Called when player interacts with the NPC***
    public void InteractWithNPC(QuestLog playerQuestLog)
    {
        if (!MeetsPrerequisites(playerQuestLog))
        {
            Debug.Log("Player does not meet quest prerequisites.");
            return;
        }

        QuestDetails quest = GetNextAvailableQuest(playerQuestLog);
        if (quest == null)
        {
            Debug.Log("No quest available.");
            return;
        }

        if (quest.autoAcceptQuest)
        {
          
            AcceptQuest(playerQuestLog, quest,this);
        }
        else
        {
            //ADD QUEST GIVER UI HERE TO DISPLAY THE QUESTS. CALL AcceptQuest() from UI ACCEPT BUTTON.
            Debug.Log($"Offer Quest: {quest.questName}");
            
        }
        //quest.questName = questName;
    }

    public void LookAtNPC()
    {
        // ADD ONSCREEN PROMPT HERE? MAYBE UI TEXT OR NOTIF.
        //Debug.Log("Looking at quest giver NPC.");
    }

    //gets next quest from quest giver if multiple are avail
    private QuestDetails GetNextAvailableQuest(QuestLog playerQuestLog)
    {
        foreach (var q in quests)
        {
            if (!repeatable && playerQuestLog.HasQuestOrCompleted(q)) continue;
            return q;
        }
        return null;
    }

    //check quest prereq if quest needs to be complete before given quest
    private bool MeetsPrerequisites(QuestLog playerQuestLog)
    {
        foreach (var req in requiredCompletedQuests)
        {
            if (!playerQuestLog.HasCompleted(req))
                return false;
        }
        return true;
    }
    //sends the accepted quest to the quest log... if completed but not in completed quest it auto turns in quest
    public void AcceptQuest(QuestLog log, QuestDetails quest,QuestGiver questGiver)
    {
        log.AcceptQuest(quest,questGiver);

        if (quest.autoCompleteQuest && log.IsQuestCompleted(quest))
        {
            log.MarkQuestTurnedIn(quest);
            Debug.Log("Quest auto-completed and turned in.");
           
        }
        else
        {
            Debug.Log("Quest accepted.");
        }
    }
}
