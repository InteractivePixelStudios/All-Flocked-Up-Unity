using System.Collections.Generic;
using UnityEngine;

public class UI_CanvasController : MonoBehaviour
{
    [Header("TimerCanvas")]
    [SerializeField] private UI_QuestTimer timerCanvas;
    public UI_QuestTimer activeTimerInstance;

    [Header("QuestGiverCanvas")]
    [SerializeField] private UI_QuestGiver questGiverCanvas;
    public UI_QuestGiver activeGiverInstance;

    [Header("QuestRewardsCanvas")]
    [SerializeField] private UI_QuestReward questRewardsCanvas;
    public UI_QuestReward activeRewardInstance;

    [Header("QuestTrackerCanvas")]
    [SerializeField] private UI_QuestTracker questTrackerCanvas;
    public UI_QuestTracker activeTrackerInstance;

    [Header("QuestNotifCanvas")]
    [SerializeField] private UI_QuestNotif questNotifCanvas;
    public UI_QuestNotif activeNotifInstance;

    [Header("QuestLocationNotifCanvas")]
    [SerializeField] private UI_QuestLocationNotif questLocationNotifCanvas;
    public UI_QuestLocationNotif activeLocationNotifInstance;

    [Header("QuestLogCanvas")]
    [SerializeField] private UI_QuestLog questLogCanvas;
    public UI_QuestLog activeLogInstance;

    [Header("DialogueCanvas")]
    public UI_DialogueCanvas dialogueCanvas;
    public UI_DialogueCanvas activeDialogueInstance;
    public string[] dialogueResponses;

    [Header("TrashCanvas")]
    [SerializeField] private UI_TrashCanvas trashCanvas;
    public UI_TrashCanvas activeTrashInstance;

    [Header("RaceCanvas")]
    [SerializeField] private UI_RaceGiver raceGiverCanvas;
    public UI_RaceGiver raceGiverInstance;

    [SerializeField] private UI_RaceReward raceRewardCanvas;
    public UI_RaceReward raceRewardInstance;
    public Dictionary<GameObject, float> standings = new Dictionary<GameObject, float>();
   

    [SerializeField] private UI_RaceFail raceFailCanvas;
    public UI_RaceFail raceFailInstance;

    [SerializeField] private UI_RaceCountdown raceCountdownCanvas;
    public UI_RaceCountdown activeCountdownInstance;

    //cursor on
    public void ShowPlayerCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log("Cursor Toggle ON");
    }
    //cursor off
    public void HidePlayerCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Cursor Toggle OFF");
    }

    //quest timer canvas
    public void ShowTimer()
    {
        activeTimerInstance = Instantiate(timerCanvas);

    }
    //quest timer canvas
    public void EndTimer()
    {
        if (activeTimerInstance != null)
        {
            activeTimerInstance.DestroyTimer();
            activeTimerInstance = null;

        }

    }
    //quest giver canvas
    public void ShowQuestGiver(QuestGiver questGiver)
    {
        ShowPlayerCursor();
        activeGiverInstance = Instantiate(questGiverCanvas);
        activeGiverInstance.currentquestGiver = questGiver;
        
    }

    //quest giver canvas
    public void DestroyQuestGiver()
    {
        if (questGiverCanvas != null)
        {
            activeGiverInstance.CloseQuestGiverUI();
            activeGiverInstance = null;
            HidePlayerCursor();
        }
    }
    //quest reward canvas
    public void ShowQuestReward()
    {
        activeRewardInstance=Instantiate(questRewardsCanvas);
        ShowPlayerCursor();
    }
    //quest reward canvas
    public void DestroyQuestReward()
    {
        if (activeRewardInstance != null)
        {
            activeRewardInstance.AcceptReward();
            activeRewardInstance = null;
            HidePlayerCursor();
        }
    }

    //quest tracker canvas
    public void ShowTracker()
    {
        activeTrackerInstance = Instantiate(questTrackerCanvas);

    }
    //quest tracker canvas
    public void DestroyTracker()
    {
        if(activeTrackerInstance != null)
        {
            activeTrackerInstance.RemoveTracker();
            activeTrackerInstance = null;

        }
    }

    //Timed Destroy
    public void ShowQuestNotif(string text)
    {
        if  (activeNotifInstance != null)
        {
            activeNotifInstance.SetNotifText(text);
            return;
        }
        activeNotifInstance = Instantiate(questNotifCanvas);
        activeNotifInstance.SetNotifText(text);
        activeNotifInstance.ShowQuestNotif();
        
    }

    
    //quest objective complete notif
    public bool OnQuestNotifShown()
    {
        return activeNotifInstance != null && activeNotifInstance.isActiveAndEnabled;
    }
    //quest location notif
    //Timed Destroy
    public void ShowQuestLocationNotif()
    {
        activeLocationNotifInstance = Instantiate(questLocationNotifCanvas);
    }
    //on quest notif shown bool
    public bool OnQuestLocationNotifShown()
    {
        return activeLocationNotifInstance!=null && activeLocationNotifInstance.isActiveAndEnabled;
    }

    //quest log canvas
    public void ShowQuestLog()
    {

            activeLogInstance = Instantiate(questLogCanvas);
            ShowPlayerCursor();

    }

    //quest log canvas
    public void DestroyQuestLog()
    {
        if (activeLogInstance != null)
        {
            activeLogInstance.CloseQuestLog();
            activeLogInstance=null;
            HidePlayerCursor();
        }
    }

    //dialogue canvas
    public void OpenDialogue()
    {

        dialogueCanvas.gameObject.SetActive(true);

    }
    //dialogue response options transfer
    public void SendResponseOptions(string[] responses)
    {
        activeDialogueInstance.responses = responses;
    }
    //dialogue response array transfer
    public string[] GetCurrentResponseOptions()
    {
        return dialogueResponses;
    }
    //dialogue canvas
    public void CloseDialogue()
    {
        if(dialogueCanvas != null || dialogueCanvas.isActiveAndEnabled)
        {
            //activeDialogueInstance.DestroyDialogue();
            dialogueCanvas.gameObject.SetActive(false);
        }
    }
    //trash canvas
    public void ShowTrashPrompt()
    {
       activeTrashInstance= Instantiate(trashCanvas);
        activeTrashInstance.InitCanvas();
        if(activeTrashInstance != null )
        {
            activeTrashInstance.DestroyCanvas();
            activeTrashInstance = null;
        }
    }
    //race giver canvas
    public void OpenRaceGiver()
    {
        raceGiverInstance = Instantiate(raceGiverCanvas);
        ShowPlayerCursor();
    }
    //race giver canvas
    public void CloseRaceGiver()
    {
        if(raceGiverInstance != null)
        {
            raceGiverInstance.CloseRaceGiver();
            raceGiverInstance = null;
            HidePlayerCursor();
        }
    }
    //race rewards canavas
    public void OpenRaceRewards()
    {
        raceRewardInstance = Instantiate(raceRewardCanvas);
        ShowPlayerCursor();
    }
    //race rewards canvas
    public void CloseRaceRewards()
    {
        if(raceRewardInstance != null)
        {
            Destroy(raceRewardInstance);
            raceRewardInstance = null;
            HidePlayerCursor();
        }
    }
    //race fail canvas
    public void OpenRaceFail()
    {
        raceFailInstance = Instantiate(raceFailCanvas);
        SendStandings();
        ShowPlayerCursor() ;
    }
    //race fail canvas
    public void CloseRaceFail()
    {
        if(raceFailInstance != null)
        {
            Destroy(raceFailInstance);
            raceFailInstance = null;
            HidePlayerCursor();
        }
    }

    public void OpenCountdownCanvas()
    {
        activeCountdownInstance = Instantiate(raceCountdownCanvas);
    }

    public void CollectRaceStandings(GameObject racer, float time)
    {
        standings.Add(racer, time);
        //raceRewardInstance.racerList.Add
    }

    public void SendStandings()
    {

        raceRewardInstance.racerList.Equals(standings);
    }

}
