using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    public Dictionary<GameObject, float> standings = new();
    [SerializeField] private UI_RaceFail raceFailCanvas;
    public UI_RaceFail raceFailInstance;
    [SerializeField] private UI_RaceCountdown raceCountdownCanvas;
    public UI_RaceCountdown activeCountdownInstance;
    [Header("Wingventory")]
    [SerializeField] private WingventoryCanvas wingventoryCanvas;
    public WingventoryCanvas activeWingventory;
    [Header("NestMenu")]
    [SerializeField] private UI_NestMenu nestMenuCanvas;
    public UI_NestMenu activeNestInstance;
    [Header("ShopUI")]
    [SerializeField] private ShopConfirmUI shopUICanvas;
    public ShopConfirmUI activeShopCanvas;
    public ShopLocation shopLocationRef;
    [Header("MainMenu")]
    [SerializeField] private UI_MainMenu mainMenuCanvas;
    public UI_MainMenu activeMainMenu;
    public Transform mainMenuSpawnPoint;
    [Header("PauseMenu")]
    [SerializeField] private UI_PauseMenu pauseMenuCanvas;
    public UI_PauseMenu activePauseMenu;
    [Header("BugReporter")]
    [SerializeField] private UI_BugReporter bugReporterCanvas;
    public UI_BugReporter activeBugReporter;
    [Header("DebugMenu")]
    [SerializeField] private UI_DebugMenu debugMenuCanvas;
    public UI_DebugMenu activeDebugMenu;
    [Header("MainMap")]
    [SerializeField] private UI_MainMap mainMapCanvas;
    public UI_MainMap activeMapCanvas;

    private void Start()
    {
        SpawnMainMenu();
    }
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
        SendStandings();
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
        if (!standings.ContainsKey(racer))
        {
            standings.Add(racer, time);
            Debug.Log("Added to CC");
        }
        //raceRewardInstance.racerList.Add
    }

    public void SendStandings()
    {

        foreach(var racer in  standings)
        {
            raceRewardInstance.GetRaceStandings(racer.Key, racer.Value);
        }
            
    }

    public void OpenWingventory()
    {
        activeWingventory = Instantiate(wingventoryCanvas);
        ShowPlayerCursor();
    }

    public void CloseWingventory()
    {
        if(activeWingventory != null)
        {
            Destroy(activeWingventory);
            activeWingventory = null;
            HidePlayerCursor();
        }
    }

    public void OpenNestMenu()
    {
        activeNestInstance = Instantiate(nestMenuCanvas);
        activeNestInstance.canvasController = this; 
        ShowPlayerCursor();
    }

    public void CloseNestMenu()
    {
        if(activeNestInstance != null)
        {
            Destroy(activeNestInstance);
            activeNestInstance = null;
            HidePlayerCursor();
        }

    }

    public void OpenShopUI(ShopItem item)
    {
        activeShopCanvas = Instantiate(shopUICanvas);
        activeShopCanvas.currentItem = item;
        activeShopCanvas.canvasController = this;
        ShowPlayerCursor();
    }

    public void CloseShopUI()
    {
        if(shopUICanvas != null)
        {
            Destroy(activeShopCanvas);
            activeShopCanvas = null;
            HidePlayerCursor();
        }
    }

    public void PauseGame()
    {
        if(activePauseMenu== null)
        {
            activePauseMenu =Instantiate(pauseMenuCanvas);
            ShowPlayerCursor() ;
            Time.timeScale = 0;
            
        }
    }

    public void ResumeGame()
    {
        if(activePauseMenu!= null)
        {
            Time.timeScale = 1;
            activePauseMenu.ClosePauseUI();
            Destroy(activePauseMenu);
            activePauseMenu = null;
            HidePlayerCursor() ;
        }
    }

    public void SpawnMainMenu()
    {
        if (activeMainMenu == null)
        {
            activeMainMenu = Instantiate(mainMenuCanvas,mainMenuSpawnPoint);
            ShowPlayerCursor();
            Debug.Log("Spawned");
        }
        Debug.Log("Called");
    }

    public void DestroyMainMenu()
    {
        if(activeMainMenu != null)
        {
            Destroy(activeMainMenu);
            activeMainMenu = null;
            HidePlayerCursor();
        }
    }

    public void OpenBugReporter()
    {
        if (activeBugReporter == null)
        {
            activeBugReporter = Instantiate(bugReporterCanvas);
            ShowPlayerCursor();
            Time.timeScale = 0;
        }
    }

    public void CloseBugReporter()
    {
        if(activeBugReporter != null)
        {
            Destroy(activeBugReporter);
            activeBugReporter = null;
            HidePlayerCursor();
            Time.timeScale = 1;
        }
    }

    public void OpenDebugMenu()
    {
        if(activeDebugMenu == null)
        {
            activeDebugMenu = Instantiate(debugMenuCanvas);
            ShowPlayerCursor();
        }
    }

    public void CloseDebugMenu()
    {
        if(activeDebugMenu != null)
        {
            Destroy(activeDebugMenu);
            activeDebugMenu = null;
            HidePlayerCursor();
        }
    }

    public void OpenMainMap()
    {
        if(activeMapCanvas == null)
        {
            activeMapCanvas = Instantiate(mainMapCanvas);
            ShowPlayerCursor();
            Time.timeScale = 0;
        }
    }

    public void CloseMainMap()
    {
        if(activeMapCanvas != null)
        {
            Destroy(activeMapCanvas.gameObject);
            activeMapCanvas = null;
            HidePlayerCursor();
            Time.timeScale = 1;
        }
    }

}
