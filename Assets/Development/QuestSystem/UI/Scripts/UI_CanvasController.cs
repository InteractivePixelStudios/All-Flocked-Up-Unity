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
    bool isUIMap;
    [Header("Health")]
    [SerializeField] private RespawnController respawnCanvasPrefab;
    public RespawnController activeRespawnCanvas;
    [Header("LevelTransition")]
    [SerializeField] private UI_LevelTransition levelTransitionPrefab;
    public UI_LevelTransition activeLevelTransition;
    public string cachedLevelName;
    public LevelTransition transitionObj;
    [Header("TutorialPrompt")]
    [SerializeField] private TutorialPrompt promptPrefab;
    public TutorialPrompt activeTutPrompt;
    public int cachedTutPromptIndex;
    [Header("SkinSelector")]
    [SerializeField] private UI_SkinSelector skinSelectorPrefab;
    public UI_SkinSelector activeSkinSelector;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        player = input.gameObject;
        //var agents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);
        //foreach (var agent in agents)
        //{
        //    enemies.Add(agent);  
        //}
        //if (SceneManager.GetActiveScene().name != "MainMenu")
        //{
        //    //ShowPlayerCursor();
        //    //HidePlayerCursor();
        //}

        //SpawnMainMenu();
        //OpenLanguageSelect(); //remove after testing
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
        isUIMap = false;
       // ResumeEnemy();
        Debug.Log("PLAYERMAP");
    }

    public void SetUIMap()
    {
        input.SwitchCurrentActionMap("UI");
        player.GetComponent<PlayerGroundMovement>().enabled = false;
        player.GetComponent<PlayerFlightMovement>().enabled = false;
        isUIMap = true;
        //FreezeEnemies();
        Debug.Log("UIMAP");
    }
    //cursor on
    public void ShowPlayerCursor()
    {
        SetUIMap();
        if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }else if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        Debug.Log("Cursor Toggle ON");
    }
    //cursor off
    public void HidePlayerCursor()
    {
        SetPlayerMap();
        if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        if (!isUIMap && activeDialogueInstance != null)
        {
            ShowPlayerCursor();
        }
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
            if (isUIMap)
            {
                HidePlayerCursor();
            }
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
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
    }
    //quest reward canvas
    public void DestroyQuestReward()
    {
        if (activeRewardInstance != null)
        {
            if (isUIMap)
            {
                HidePlayerCursor();
            }
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
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }

    }

    //quest log canvas
    public void DestroyQuestLog()
    {
        if (activeLogInstance != null)
        {
            activeLogInstance.CloseQuestLog();
            activeLogInstance=null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }

    //dialogue canvas
    public void OpenDialogue()
    {

        if(activeDialogueInstance == null)
        {
            activeDialogueInstance = Instantiate(dialogueCanvas);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
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
            if (isUIMap)
            {
                HidePlayerCursor();
            }
            //activeDialogueInstance.DestroyDialogue();
            Destroy(activeDialogueInstance.gameObject);
            activeDialogueInstance = null;
        }
    }
    //trash canvas
    public void ShowTrashPrompt(TrashCanInteraction trashCan)
    {
        if (activeTrashInstance == null)
        {
            activeTrashInstance = Instantiate(trashCanvas);
            activeTrashInstance.SetTrashInstance(trashCan);
            activeTrashInstance.SetCanvasReference(this);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
        }
    }

    public void CloseTrashPrompt()
    {
        if (activeTrashInstance != null)
        {
            Destroy(activeTrashInstance.gameObject);
            activeTrashInstance = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }
    //race giver canvas
    public void OpenRaceGiver()
    {
        raceGiverInstance = Instantiate(raceGiverCanvas);
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }

    }
    //race giver canvas
    public void CloseRaceGiver()
    {
        if(raceGiverInstance != null)
        {
            raceGiverInstance.CloseRaceGiver();
            raceGiverInstance = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }

        }
    }
    //race rewards canavas
    public void OpenRaceRewards()
    {
        raceRewardInstance = Instantiate(raceRewardCanvas);
        raceRewardInstance.SetCanvasControllerRef(this);
        SendStandings();
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
    }
    //race rewards canvas
    public void CloseRaceRewards()
    {
        if(raceRewardInstance != null)
        {
            Destroy(raceRewardInstance.gameObject);
            if (isUIMap)
            {
                HidePlayerCursor();
            }
            raceRewardInstance = null;
        }
    }
    //race fail canvas
    public void OpenRaceFail()
    {
        raceFailInstance = Instantiate(raceFailCanvas);
        SendStandings();
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
    }
    //race fail canvas
    public void CloseRaceFail()
    {
        if(raceFailInstance != null)
        {
            Destroy(raceFailInstance.gameObject);
            if (isUIMap)
            {
                HidePlayerCursor();
            }
            raceFailInstance = null;
        }
    }

    public void OpenCountdownCanvas()
    {
        if (activeCountdownInstance == null)
        {
            activeCountdownInstance = Instantiate(raceCountdownCanvas);
            Debug.Log("Countdown");
        }
        else return;
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
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
    }

    public void CloseWingventory()
    {
        if(activeWingventory != null)
        {
            Destroy(activeWingventory.gameObject);
            activeWingventory = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }

    public void OpenNestMenu()
    {
        activeNestInstance = Instantiate(nestMenuCanvas);
        activeNestInstance.canvasController = this; 
        activeNestInstance.playerStats = player.GetComponent<PlayerCounter>();
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
    }

    public void CloseNestMenu()
    {
        if(activeNestInstance != null)
        {
            Destroy(activeNestInstance.gameObject);
            activeNestInstance = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
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
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
    }

    public void CloseShopUI()
    {
        if(shopUICanvas != null)
        {
            Destroy(activeShopCanvas.gameObject);
            activeShopCanvas = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }

    public void PauseGame()
    {
        if(activePauseMenu== null)
        {
            activePauseMenu =Instantiate(pauseMenuCanvas);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
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
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }

    }


    public void OpenBugReporter()
    {
        if (activeBugReporter == null)
        {
            activeBugReporter = Instantiate(bugReporterCanvas);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
            Time.timeScale = 0;
        }
    }

    public void CloseBugReporter()
    {
        if(activeBugReporter != null)
        {
            Destroy(activeBugReporter.gameObject);
            activeBugReporter = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
            Time.timeScale = 1;
        }
    }

    public void OpenDebugMenu()
    {
        if(activeDebugMenu == null)
        {
            activeDebugMenu = Instantiate(debugMenuCanvas);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
        }
    }

    public void CloseDebugMenu()
    {
        if(activeDebugMenu != null)
        {
            Destroy(activeDebugMenu.gameObject);
            activeDebugMenu = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }

    public void OpenMainMap()
    {
        if(activeMapCanvas == null)
        {
            activeMapCanvas = Instantiate(mainMapCanvas);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
            Time.timeScale = 0;
        }
    }

    public void CloseMainMap()
    {
        if(activeMapCanvas != null)
        {
            Destroy(activeMapCanvas.gameObject);
            activeMapCanvas = null;
            if (isUIMap)
            {
                HidePlayerCursor();
            }
            Time.timeScale = 1;
        }
    }

    public void OpenLanguageSelect()
    {
        activeLanguageCanvas = Instantiate(languageSelectPrefab);
        if(activeLanguageCanvas != null)
        {
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
        }
    }

    public void CloseLanguageSelect()
    {
        if(activeLanguageCanvas != null)
        {
            Destroy(activeLanguageCanvas.gameObject);
            //HidePlayerCursor();
            var menu = FindAnyObjectByType<UI_MainMenu>();
            if(menu != null)
            {
                menu.SetSelectedObject(menu.startButton.gameObject);
            }
            
        }

    }

    public void OpenRespawn()
    {
        activeRespawnCanvas = Instantiate(respawnCanvasPrefab);
        if(activeRespawnCanvas != null)
        {
            activeRespawnCanvas.canvasController = this;
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
        }
    }

    public void CloseRespawn()
    {
        if(activeRespawnCanvas != null)
        {
            Destroy(activeRespawnCanvas.gameObject);
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }

    public void OpenLevelTransition()
    {
        if (activeLevelTransition == null)
        {
            activeLevelTransition = Instantiate(levelTransitionPrefab);
            activeLevelTransition.sceneName = cachedLevelName;
            activeLevelTransition.transitionObj = transitionObj;
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
        }
    }

    public void CloseLevelTransition()
    {
        if (activeLevelTransition != null)
        {
            Destroy(activeLevelTransition.gameObject);
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }

    public void ShowTutorialPrompt()
    {
        if(activeTutPrompt == null)
        {
            activeTutPrompt = Instantiate(promptPrefab);
            activeTutPrompt.promptIndex = cachedTutPromptIndex;
            activeTutPrompt.canvasController = this;
            //ShowPlayerCursor() ;
        }
    }

    public void DestroyPrompt()
    {
        if(activeTutPrompt != null)
        {
           // HidePlayerCursor();
            Destroy(activeTutPrompt.gameObject);
            cachedTutPromptIndex = -1;
        }
    }

    public void ShowSkinSelector()
    {
        if(activeSkinSelector == null)
        {
            activeSkinSelector = Instantiate(skinSelectorPrefab);
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
        }
    }

    public void HideSkinSelector()
    {
        if(activeSkinSelector != null)
        {
            Destroy(activeSkinSelector.gameObject);
            if (isUIMap)
            {
                HidePlayerCursor();
            }
        }
    }


}
