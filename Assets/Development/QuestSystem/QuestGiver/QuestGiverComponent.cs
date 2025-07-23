using UnityEngine;
using System.Collections.Generic;

public class QuestGiver : MonoBehaviour, QuestInteraction
{
    [Header("Quests Offered")]
    public List<QuestDetails> quests = new();
    public bool offerSequentially = true;
    public bool repeatable = false;

    [Header("Prerequisites")]
    public List<QuestDetails> requiredCompletedQuests = new();

    // --- Called when player interacts ---
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
            AcceptQuest(playerQuestLog, quest);
        }
        else
        {
            // Trigger your custom UI prompt (quest title, accept button)
            Debug.Log($"Offer Quest: {quest.questName}");
            // In UI: call AcceptQuest(...) from the accept button.
        }
    }

    public void LookAtNPC()
    {
        // Optional: Show floating UI, play idle animation, etc.
        Debug.Log("Looking at quest giver NPC.");
    }

    private QuestDetails GetNextAvailableQuest(QuestLog playerQuestLog)
    {
        foreach (var q in quests)
        {
            if (!repeatable && playerQuestLog.HasQuestOrCompleted(q)) continue;
            return q;
        }
        return null;
    }

    private bool MeetsPrerequisites(QuestLog playerQuestLog)
    {
        foreach (var req in requiredCompletedQuests)
        {
            if (!playerQuestLog.HasCompleted(req))
                return false;
        }
        return true;
    }

    public void AcceptQuest(QuestLog log, QuestDetails quest)
    {
        log.AcceptQuest(quest);

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
