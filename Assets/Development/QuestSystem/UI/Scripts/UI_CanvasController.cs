using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using Unity.Cinemachine;
using UnityEngine.AI;
using System.Linq;

public class UI_CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private List<NavMeshAgent> enemies;

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
    [Header("LanguageSelect")]
    [SerializeField] private UI_LanguageSelector languageSelectPrefab;
    public UI_LanguageSelector activeLanguageCanvas;
    [Header("PlayerInputComponent")]
    [SerializeField] private PlayerInput input;
    [Header("Health")]
    [SerializeField] private RespawnController respawnCanvasPrefab;
    public RespawnController activeRespawnCanvas;

    private void Start()
    {
        input = FindFirstObjectByType<PlayerInput>();
        player = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
        var agents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);
        foreach (var agent in agents)
        {
            enemies.Add(agent);  
        }

        SpawnMainMenu();
        OpenLanguageSelect(); //remove after testing
    }

    public void FreezeEnemies()
    {
        
        foreach (var enemy in enemies)
        {
            if(enemy.gameObject != null)
            {
                enemy.enabled = false;
            }
            else enemies.Remove(enemy);
        }
    }

    public void ResumeEnemy()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject != null)
            {
                enemy.enabled = true;
            }
            else enemies.Remove(enemy);
        }
    }

    public void SetPlayerMap()
    {
        input.SwitchCurrentActionMap("Player");
        player.GetComponent<PlayerGroundMovement>().enabled = true;
        player.GetComponent<PlayerFlightMovement>().enabled = true;
       // ResumeEnemy();
        Debug.Log("PLAYERMAP");
    }

    public void SetUIMap()
    {
        input.SwitchCurrentActionMap("UI");
        player.GetComponent<PlayerGroundMovement>().enabled = false;
        player.GetComponent<PlayerFlightMovement>().enabled = false;
        //FreezeEnemies();
        Debug.Log("UIMAP");
    }
    //cursor on
    public void ShowPlayerCursor()
    {
        SetUIMap();
        if (Mouse.current != null &&  Mouse.current.wasUpdatedThisFrame)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        Debug.Log("Cursor Toggle ON");
    }
    //cursor off
    public void HidePlayerCursor()
    {
        SetPlayerMap();
        //if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        //{
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

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
        activeGiverInstance.canvasController = this;
        activeGiverInstance.UpdateUIText(questGiver.quests[0].questName, questGiver.quests[0].questLogDescription, questGiver.quests[0].questName); // change last one to rewards
        
    }

    //quest giver canvas
    public void DestroyQuestGiver()
    {
        if (questGiverCanvas != null)
        {
            HidePlayerCursor();
            Destroy(activeGiverInstance.gameObject);
            activeGiverInstance = null;
        }
    }
    //quest reward canvas
    public void ShowQuestReward(QuestDetails quest)
    {
        activeRewardInstance=Instantiate(questRewardsCanvas);
        activeRewardInstance.quest = quest;
        activeRewardInstance.canvasController = this;
        ShowPlayerCursor();
    }
    //quest reward canvas
    public void DestroyQuestReward()
    {
        if (activeRewardInstance != null)
        {
            HidePlayerCursor();
            Destroy(activeRewardInstance.gameObject);
            activeRewardInstance = null;
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

        if(activeDialogueInstance == null)
        {
            activeDialogueInstance = Instantiate(dialogueCanvas);
            ShowPlayerCursor();
        }

    }
    //dialogue response options transfer
    public void SendResponseOptions(LocalizedString[] responses)
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
        if(activeDialogueInstance != null)
        {
            Debug.Log("DialogueCLosedFromCanvas");
            HidePlayerCursor() ;
            //activeDialogueInstance.DestroyDialogue();
            Destroy(activeDialogueInstance.gameObject);

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
        Time.timeScale = 0;
    }
    //race giver canvas
    public void CloseRaceGiver()
    {
        if(raceGiverInstance != null)
        {
            raceGiverInstance.CloseRaceGiver();
            raceGiverInstance = null;
            HidePlayerCursor();
            Time.timeScale = 1;
        }
    }
    //race rewards canavas
    public void OpenRaceRewards()
    {
        raceRewardInstance = Instantiate(raceRewardCanvas);
        SendStandings();
        ShowPlayerCursor();
        Time.timeScale = 0;
    }
    //race rewards canvas
    public void CloseRaceRewards()
    {
        if(raceRewardInstance != null)
        {
            Destroy(raceRewardInstance.gameObject);
            raceRewardInstance = null;
            HidePlayerCursor();
            Time.timeScale = 1;

        }
    }
    //race fail canvas
    public void OpenRaceFail()
    {
        raceFailInstance = Instantiate(raceFailCanvas);
        SendStandings();
        ShowPlayerCursor() ;
        Time.timeScale = 0;
    }
    //race fail canvas
    public void CloseRaceFail()
    {
        if(raceFailInstance != null)
        {
            Destroy(raceFailInstance.gameObject);
            raceFailInstance = null;
            HidePlayerCursor();
            Time.timeScale = 1;
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
            Destroy(activeWingventory.gameObject);
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
            Destroy(activeNestInstance.gameObject);
            activeNestInstance = null;
            HidePlayerCursor();
        }

    }

    public void OpenShopUI(ShopItem item, ShopLocation location)
    {
        activeShopCanvas = Instantiate(shopUICanvas);
        shopLocationRef = location;
        activeShopCanvas.transform.SetParent(shopLocationRef.transform);
        activeShopCanvas.transform.localPosition = Vector3.zero + new Vector3(0,1.5f,0);
        activeShopCanvas.currentItem = item;
        activeShopCanvas.canvasController = this;
        shopLocationRef = location;
        ShowPlayerCursor();
    }

    public void CloseShopUI()
    {
        if(shopUICanvas != null)
        {
            Destroy(activeShopCanvas.gameObject);
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
            Destroy(activePauseMenu.gameObject);
            activePauseMenu = null;
            HidePlayerCursor() ;
        }

    }

    public void SpawnMainMenu()
    {
        if (activeMainMenu == null  && SceneManager.GetActiveScene().name == "Cootorial Island")
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
            Destroy(activeMainMenu.gameObject);
            activeMainMenu = null;
            HidePlayerCursor();
           // Object.FindFirstObjectByType<PlayerSkinSelector>().StartSkinSelector();
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
            Destroy(activeBugReporter.gameObject);
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
            Destroy(activeDebugMenu.gameObject);
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

    public void OpenLanguageSelect()
    {
        activeLanguageCanvas = Instantiate(languageSelectPrefab);
        if(activeLanguageCanvas != null)
        {
            ShowPlayerCursor();
        }
    }

    public void CloseLanguageSelect()
    {
        if(activeLanguageCanvas != null)
        {
            HidePlayerCursor();
            Destroy(activeLanguageCanvas.gameObject);
        }

    }

    public void OpenRespawn()
    {
        activeRespawnCanvas = Instantiate(respawnCanvasPrefab);
        ShowPlayerCursor() ;
    }

    public void CloseRespawn()
    {
        if(activeRespawnCanvas != null)
        {
            Destroy(activeRespawnCanvas.gameObject); 
            HidePlayerCursor();
        }
    }

}
