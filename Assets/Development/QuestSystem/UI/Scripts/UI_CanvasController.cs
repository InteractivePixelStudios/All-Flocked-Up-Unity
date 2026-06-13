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
    public int cachedIntroIndex;
    [Header("SkinSelector")]
    [SerializeField] private UI_SkinSelector skinSelectorPrefab;
    public UI_SkinSelector activeSkinSelector;
    Dictionary<Graphic, Color> cachedUIColors = new();
    public bool uiOpen;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        player = input.gameObject;
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
            if (cam.GetComponent<CinemachineOrbitalFollow>() != null && cam.CompareTag("Player"))
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

    public void SetPlayerMap()
    {
        if (isUIMap)
        {
            input.SwitchCurrentActionMap("Player");
            player.GetComponent<PlayerGroundMovement>().enabled = true;
            player.GetComponent<PlayerFlightMovement>().enabled = true;
            camRef.GetComponent<CinemachineOrbitalFollow>().enabled = true;
            isUIMap = false;
           // Debug.Log("PlayerMAP");

        }
        else return;
    }

    public void SetUIMap()
    {
        if (!isUIMap)
        {
            input.SwitchCurrentActionMap("UI");
            player.GetComponent<PlayerGroundMovement>().enabled = false;
            player.GetComponent<PlayerFlightMovement>().enabled = false;
            camRef.GetComponent<CinemachineOrbitalFollow>().enabled = false;
            isUIMap = true;
            //Debug.Log("UIMAP");
        }
        else return;
    }
    //cursor on
    public void ShowPlayerCursor()
    {
        SetUIMap();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
       // Debug.Log("Showing cursor");
    }
    //cursor off
    public void HidePlayerCursor()
    {
        SetPlayerMap();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
      //  Debug.Log("Hiding cursor");
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
        if (!isUIMap)
        {
            ShowPlayerCursor();
        }
        ApplySavedContrast();
        uiOpen = true;
        
    }

    //quest giver canvas
    public void DestroyQuestGiver()
    {
        if (questGiverCanvas != null)
        {

                HidePlayerCursor();

            Destroy(activeGiverInstance.gameObject);
            uiOpen = false;
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
            if (!isUIMap)
            {
            ShowPlayerCursor();

            }
            uiOpen = true;
        
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
        //if (UI_HudController.Instance.GetIsTDOpen())
        //{
        //    ShowQuestLog();
        //}else
        //{
        //    DestroyQuestLog();   
        //}

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
        if (!uiOpen)
        {
            activeLogInstance = Instantiate(questLogCanvas);
            ApplySavedContrast();
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
            uiOpen = true;
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
            uiOpen = false;
        }
    }

    //dialogue canvas
    public void OpenDialogue()
    {

            if (activeDialogueInstance == null)
            {
                activeDialogueInstance = Instantiate(dialogueCanvas);
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
            ApplySavedContrast();
            uiOpen = true;
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
            uiOpen = false;
        }
    }

    //race giver canvas
    public void OpenRaceGiver()
    {

            raceGiverInstance = Instantiate(raceGiverCanvas);
            ApplySavedContrast();
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
        if (!uiOpen)
        {
            raceRewardInstance = Instantiate(raceRewardCanvas);
            ApplySavedContrast();
            raceRewardInstance.SetCanvasControllerRef(this);
            SendStandings();
            if (!isUIMap)
            {
                ShowPlayerCursor();

            }
            Time.timeScale = 0;
            uiOpen = true;
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
            Time.timeScale = 1;
            uiOpen = false;
        }
    }
    //race fail canvas
    public void OpenRaceFail()
    {
        if (!uiOpen)
        {
            raceFailInstance = Instantiate(raceFailCanvas);
            if (!isUIMap)
            {
                ShowPlayerCursor();

            }
            ApplySavedContrast();
            SendStandings();
            uiOpen = true;
        }
    }
    //race fail canvas
    public void CloseRaceFail(bool retry)
    {
        if (raceFailInstance != null)
        {
            Destroy(raceFailInstance.gameObject);
            if (isUIMap)
            {
                HidePlayerCursor();
            }
            raceFailInstance = null;
            uiOpen = false;
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
        if (!uiOpen)
        {
            activeWingventory = Instantiate(wingventoryCanvas);
            ApplySavedContrast();
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
            uiOpen = true;
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
            uiOpen = false;
        }
    }

    public void OpenNestMenu()
    {
        if (!uiOpen)
        {
            activeNestInstance = Instantiate(nestMenuCanvas);
            ApplySavedContrast();
            activeNestInstance.canvasController = this;
            activeNestInstance.playerStats = player.GetComponent<PlayerCounter>();
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
            uiOpen = true;
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
            uiOpen = false;
        }

    }

    public void OpenShopUI(ShopItem item, ShopLocation location)
    {
        if (!uiOpen)
        {
            activeShopCanvas = Instantiate(shopUICanvas);
            ApplySavedContrast();
            shopLocationRef = location;
            activeShopCanvas.transform.SetParent(shopLocationRef.transform);
            activeShopCanvas.transform.localPosition = Vector3.zero + new Vector3(0, 1.5f, 0);
            activeShopCanvas.currentItem = item;
            activeShopCanvas.canvasController = this;
            shopLocationRef = location;
            if (!isUIMap)
            {
                ShowPlayerCursor();
            }
            uiOpen = true;
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
            uiOpen = false;
        }
    }

    public void PauseGame()
    {
        if (!uiOpen)
        {
            if (activePauseMenu == null && SceneManager.GetActiveScene() !=SceneManager.GetSceneByName("MainMenu"))
            {
                activePauseMenu = Instantiate(pauseMenuCanvas);
                ApplySavedContrast();
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
                Time.timeScale = 0;
                uiOpen = true;
            }

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
            uiOpen = false;
        }

    }


    public void OpenBugReporter()
    {
        if (!uiOpen)
        {
            if (activeBugReporter == null)
            {
                activeBugReporter = Instantiate(bugReporterCanvas);
                ApplySavedContrast();
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
                Time.timeScale = 0;
                uiOpen = true;
            }
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
            uiOpen = false;
        }
    }

    public void OpenDebugMenu()
    {
        if (!uiOpen)
        {
            if (activeDebugMenu == null)
            {
                activeDebugMenu = Instantiate(debugMenuCanvas);
                ApplySavedContrast();
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
                uiOpen = true;
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
            uiOpen = false;
        }
    }

    public void OpenMainMap()
    {
        if (!uiOpen)
        {
            if (activeMapCanvas == null)
            {
                activeMapCanvas = Instantiate(mainMapCanvas);
                ApplySavedContrast();
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
                Time.timeScale = 0;
                uiOpen = true;
            }
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
            uiOpen = false;
        }
    }

    public void OpenLanguageSelect()
    {

            activeLanguageCanvas = Instantiate(languageSelectPrefab);
            ApplySavedContrast();
            if (activeLanguageCanvas != null)
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


        }

    }

    public void OpenRespawn()
    {
        if (!uiOpen)
        {
            activeRespawnCanvas = Instantiate(respawnCanvasPrefab);
            ApplySavedContrast();
            if (activeRespawnCanvas != null)
            {
                activeRespawnCanvas.canvasController = this;
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
                uiOpen = true;
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
            uiOpen = false;
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

            if (activeTutPrompt == null)
            {
                activeTutPrompt = Instantiate(promptPrefab);
                ApplySavedContrast();
                activeTutPrompt.promptIndex = cachedTutPromptIndex;
                activeTutPrompt.arrowIndex = cachedIntroIndex;
                activeTutPrompt.canvasController = this;
            uiOpen = true;
            //if (!isUIMap)
            //{
            //    ShowPlayerCursor();
            //}



        }

    }

        public void DestroyPrompt()
    {
        if(activeTutPrompt != null)
        {

            //if (isUIMap)
            //{
            //    HidePlayerCursor();
            //}
            uiOpen = false;
            Destroy(activeTutPrompt.gameObject);
            cachedTutPromptIndex = -1;

        }
    }

    public void ShowSkinSelector()
    {
        if (!uiOpen)
        {
            if (activeSkinSelector == null)
            {
                activeSkinSelector = Instantiate(skinSelectorPrefab);
                ApplySavedContrast();
                if (!isUIMap)
                {
                    ShowPlayerCursor();
                }
                player.GetComponent<PlayerGroundMovement>().enabled = true;
                player.GetComponent<PlayerFlightMovement>().enabled = true;
                uiOpen = true;
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
            uiOpen = false;
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


}
