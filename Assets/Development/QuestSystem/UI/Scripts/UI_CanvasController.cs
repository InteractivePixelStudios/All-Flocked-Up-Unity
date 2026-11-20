using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using Unity.Cinemachine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using NUnit.Framework;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class UI_CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineCamera camRef;
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
    [SerializeField] private PlayerStateController playerState;
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
    public int cachedIntroIndex;
    [Header("SkinSelector")]
    [SerializeField] private UI_SkinSelector skinSelectorPrefab;
    public UI_SkinSelector activeSkinSelector;
    Dictionary<Graphic, Color> cachedUIColors = new();
    public bool uiOpen;
    [Header("HideAndSeek")]
    [SerializeField] private UI_HideSeekCountdown hasCountdownPrefab;
    public UI_HideSeekCountdown currentHASCountdown;

    [SerializeField] private UI_HASComplete hasCompletePrefab;
    public UI_HASComplete currentHASComplete;

    [SerializeField] private UI_PaintCanvas paintCanvasPrefab;
    public UI_PaintCanvas paintInstance;

    private void Start()
    {
        playerState = FindAnyObjectByType<PlayerStateController>();
        player = playerState.gameObject;
        var found = FindObjectsByType<CinemachineCamera>();
        foreach(var cam in found)
        {
            if(cam.GetComponent<CinemachineOrbitalFollow>() != null && cam.CompareTag("Player"))
            {
                camRef = cam;
            }
        }
        var ui = FindObjectsByType<UnityEngine.UI.Graphic>();
        CacheUIColors(ui);


    }

    private void OnLevelWasLoaded(int level)
    {
        var found = FindObjectsByType<CinemachineCamera>();
        foreach (var cam in found)
        {
            if (cam.CompareTag("Player"))
            {
                camRef = cam;
            }
        }
        var ui = FindObjectsByType<UnityEngine.UI.Graphic>();
        CacheUIColors(ui);
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
//   \/ \/ \/ \/ this stuff targeted for refactoring \/ \/ \/ \/
    public void SetPlayerMap()
    {
        playerState.SwitchToPlayerMap();
    }

    public void SetUIMap()
    {
        playerState.SwitchToUIMap();
    }
    //cursor on
    public void ShowPlayerCursor()
    {
        SetUIMap();

    }
    //cursor off
    public void HidePlayerCursor()
    {
        SetPlayerMap();

    }
    

    //quest timer canvas
    public void ShowTimer()
    {
        activeTimerInstance = Instantiate(timerCanvas);
        ApplySavedContrast();
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
            activeGiverInstance = Instantiate(questGiverCanvas);
            activeGiverInstance.currentquestGiver = questGiver;
            activeGiverInstance.canvasController = this;
            activeGiverInstance.UpdateUIText(questGiver.quests[0].questName, questGiver.quests[0].questLogDescription, questGiver.quests[0].questName); // change last one to rewards
            ShowPlayerCursor();
        ApplySavedContrast();      
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
            activeRewardInstance = Instantiate(questRewardsCanvas);
            ApplySavedContrast();
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
            uiOpen = false;
        }
    }

    //quest tracker canvas
    public void ShowTracker()
    {
        activeTrackerInstance = Instantiate(questTrackerCanvas);
        ApplySavedContrast();

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
        if (activeDialogueInstance != null) return;
        activeNotifInstance = Instantiate(questNotifCanvas);
        ApplySavedContrast();
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

    public void ShowToDoPanel()
    {
        UI_HudController.Instance.ShowToDoPanel();
        ApplySavedContrast();      
    }

    public void HideToDoPanel()
    {
        UI_HudController.Instance.HideToDoPanel();
    }

    //quest log canvas
    public void ShowQuestLog()
    {
            activeLogInstance = Instantiate(questLogCanvas);
            ApplySavedContrast();
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
        if (activeDialogueInstance == null)
        {
            activeDialogueInstance = Instantiate(dialogueCanvas);
            ShowPlayerCursor();
            ApplySavedContrast();          
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
            HidePlayerCursor();
            Destroy(activeDialogueInstance.gameObject);
            activeDialogueInstance = null;
        }
    }

    //race giver canvas
    public void OpenRaceGiver()
    {
        raceGiverInstance = Instantiate(raceGiverCanvas);
        ApplySavedContrast();
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
            ApplySavedContrast();
            raceRewardInstance.SetCanvasControllerRef(this);
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
            HidePlayerCursor();
            raceRewardInstance = null;
            Time.timeScale = 1;
        }
    }
    //race fail canvas
    public void OpenRaceFail()
    {
        raceFailInstance = Instantiate(raceFailCanvas);
        ShowPlayerCursor();
        ApplySavedContrast();
        SendStandings();
    }
    //race fail canvas
    public void CloseRaceFail(bool retry)
    {
        if (raceFailInstance != null)
        {
            Destroy(raceFailInstance.gameObject);
            HidePlayerCursor();
            raceFailInstance = null;
        }
        if (retry)
        {
            var raceBase = FindAnyObjectByType<RaceBase>();
            raceBase.ResetRace();
        }
        else return;
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
        }
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
        ApplySavedContrast();
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
        ApplySavedContrast();
        activeNestInstance.canvasController = this;
        activeNestInstance.playerStats = player.GetComponent<PlayerCounter>();
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
        ApplySavedContrast();
        shopLocationRef = location;
        activeShopCanvas.transform.SetParent(shopLocationRef.transform);
        activeShopCanvas.transform.localPosition = Vector3.zero + new Vector3(0, 1.5f, 0);
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

        if (activePauseMenu == null && SceneManager.GetActiveScene() !=SceneManager.GetSceneByName("MainMenu"))
        {
            activePauseMenu = Instantiate(pauseMenuCanvas);
            ApplySavedContrast();
            ShowPlayerCursor();
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
            HidePlayerCursor();
        }
    }


    public void OpenBugReporter()
    {
        if (activeBugReporter == null)
        {
            activeBugReporter = Instantiate(bugReporterCanvas);
            ApplySavedContrast();
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
        if (activeDebugMenu == null)
        {
            activeDebugMenu = Instantiate(debugMenuCanvas);
            ApplySavedContrast();
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
        if (activeMapCanvas == null)
        {
            activeMapCanvas = Instantiate(mainMapCanvas);
            ApplySavedContrast();
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
            ApplySavedContrast();
            if (activeLanguageCanvas != null)
            {
                ShowPlayerCursor();             
            }
        
    }

    public void CloseLanguageSelect()
    {
        if(activeLanguageCanvas != null)
        {
            Destroy(activeLanguageCanvas.gameObject);
        }

    }

    public void OpenRespawn()
    {
            activeRespawnCanvas = Instantiate(respawnCanvasPrefab);
            ApplySavedContrast();
            if (activeRespawnCanvas != null)
            {
                activeRespawnCanvas.canvasController = this;
                ShowPlayerCursor();
            }     
    }

    public void CloseRespawn()
    {
        if(activeRespawnCanvas != null)
        {
            Destroy(activeRespawnCanvas.gameObject);
            HidePlayerCursor();
        }
    }

    public void OpenLevelTransition()
    {
            if (activeLevelTransition == null)
            {
                activeLevelTransition = Instantiate(levelTransitionPrefab);
                ApplySavedContrast();
                activeLevelTransition.sceneName = cachedLevelName;
                activeLevelTransition.canvasController = this;
                activeLevelTransition.transitionObj = transitionObj;
                ShowPlayerCursor();              
            }
    }

    public void CloseLevelTransition()
    {
        if (activeLevelTransition != null)
        {
            Destroy(activeLevelTransition.gameObject);
            HidePlayerCursor();            
        }
    }

    public void ShowTutorialPrompt()
    {
            if (activeTutPrompt == null)
            {
                activeTutPrompt = Instantiate(promptPrefab);
                ApplySavedContrast();
                activeTutPrompt.promptIndex = cachedTutPromptIndex;
                activeTutPrompt.arrowIndex = cachedIntroIndex;
                activeTutPrompt.canvasController = this;
        }
    }

    public void DestroyPrompt()
    {
        if(activeTutPrompt != null)
        {
            Destroy(activeTutPrompt.gameObject);
            cachedTutPromptIndex = -1;
        }
    }

    public void ShowSkinSelector()
    {
            if (activeSkinSelector == null)
            {
                activeSkinSelector = Instantiate(skinSelectorPrefab);
                ApplySavedContrast();
                ShowPlayerCursor();               
                player.GetComponent<PlayerGroundMovement>().enabled = true;
                player.GetComponent<PlayerFlightMovement>().enabled = true;
            }      
    }

    public void HideSkinSelector()
    {
        if(activeSkinSelector != null)
        {
            Destroy(activeSkinSelector.gameObject);
            HidePlayerCursor();
        }
    }

    public void SetContrastMode(bool value)
    {
        var ui = FindObjectsByType<UnityEngine.UI.Graphic>();

        foreach (var element in ui)
        {
            if (element == null)
                continue;

            if (!cachedUIColors.ContainsKey(element))
            {
                cachedUIColors[element] = element.color;
            }

            if (!value)
            {
                if (cachedUIColors.TryGetValue(element, out var originalColor))
                {
                    element.color = originalColor;
                }
            }
            else
            {
                if (element is UnityEngine.UI.Text ||
                    element is TMPro.TextMeshProUGUI)
                {
                    element.color = Color.white;
                }
                else
                {
                    element.color = Color.gray;
                }
            }
        }
    }

    protected void CacheUIColors(Graphic[] ui)
    {

        foreach (var element in ui)
        {
            if (!cachedUIColors.ContainsKey(element))
            {
                cachedUIColors.Add(element, element.color);
            }

        }
    }

    protected void CacheIfMissing(Graphic element)
    {
        if (!cachedUIColors.ContainsKey(element))
        {
            cachedUIColors.Add(element, element.color);
        }
    }

     void ApplySavedContrast()
    {
        bool value = PlayerPrefs.GetInt("HighContrastMode", 0) == 1;
        SetContrastMode(value);
    }

    public void SpawnHASCountdown()
    {
        if(currentHASCountdown== null)
        {
            currentHASCountdown = Instantiate(hasCountdownPrefab);

        }
    }

    public void SpawnHASComplete()
    {
        if(currentHASComplete==null)
        {
            currentHASComplete = Instantiate(hasCompletePrefab);
            ShowPlayerCursor();         
        }
    }

    public void CloseHASComplete()
    {
        if(currentHASComplete!= null)
        {
            Destroy(currentHASComplete.gameObject);
            HidePlayerCursor();       
        }
    }

    public void OpenPaint(PaintInteraction interaction,PaintDecal decal)
    {
        if (paintInstance == null)
        {
            paintInstance = Instantiate(paintCanvasPrefab);
            paintInstance.SetCanvasController(this);
            paintInstance.SetPaintInteraction(interaction);
            paintInstance.SetDecal(decal);
            ShowPlayerCursor();
        }
    }

    public void ClosePaint()
    {
        if(paintInstance != null)
        {
            Destroy(paintInstance.gameObject);
            HidePlayerCursor();
        }
    }
}
