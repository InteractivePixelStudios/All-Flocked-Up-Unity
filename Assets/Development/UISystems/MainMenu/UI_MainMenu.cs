using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject controlsCanvas;
    [SerializeField] private GameObject skipPanel;
    public Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button skipButton;
    private bool settingsOpen;
    private bool controlsOpen;
    private GameObject playerRef;
    private CameraController cameraRef;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject saveWindowPrefab;
    [SerializeField] private GameObject currentSaveWindow;
    private string savePath;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] UI_LanguageSelector lang;
    public bool firstSelected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        startButton.onClick.AddListener(ShowSkipPanel);
        loadButton.onClick.AddListener(CheckForSavedGame);
        settingsButton.onClick.AddListener(OnSettingsOpen);
        controlsButton.onClick.AddListener(OnControlsOpen);
        creditsButton.onClick.AddListener(PlayCredits);
        quitButton.onClick.AddListener(QuitGame);
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerRef = FindAnyObjectByType<PlayerFlightMovement>().gameObject;
        cameraRef = FindAnyObjectByType<CameraController>();
        playerRef.transform.position = playerSpawnPoint.transform.position ;
        playerRef.transform.rotation  = playerSpawnPoint.transform.rotation;
        cameraRef.transform.forward = playerSpawnPoint.transform.forward;
        cameraRef.transform.position = playerRef.transform.position+ cameraOffset;
        settingsCanvas.GetComponent<UI_SettingsMenu>().parent = this.gameObject;
    }
    public void SetSelectedObject(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
    }


    private void Update()
    {

        if (lang == null && !firstSelected)
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.leftStick.ReadValue() != Vector2.zero)
                {
                    SetSelectedObject(startButton.gameObject);
                    firstSelected = true;
                }
                else return;
            }
            else return;
        }
        else return;
    }

    void ShowSkipPanel()
    {
        if (skipPanel != null)
        {
            playButton.onClick.AddListener(StartNewGame);
            skipButton.onClick.AddListener(SkipTutorial);
            skipPanel.SetActive(true);
        }
    }

    private void SkipTutorial()
    {
        canvasController.HidePlayerCursor();
        playerRef.GetComponent<PlayerGroundMovement>().enabled = true;
        playerRef.GetComponent<PlayerFlightMovement>().enabled = true;
        if (SteamManager.Initialized)
        {
            AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_000_Support");
        }
        SceneManager.LoadScene("KensingtonMarket");
    }

    public void OnSettingsOpen()
    {
        if (!settingsOpen)
        {
            mainCanvas.SetActive(false);
            settingsCanvas.SetActive(true);
            settingsOpen = true;
        }
        else
        {
            mainCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
            settingsOpen = false;
        }
    }

    protected void OnControlsOpen()
    {
        if (!controlsOpen)
        {
            mainCanvas.SetActive(false);
            controlsCanvas.SetActive(true);
            controlsOpen = true;
        }
        else
        {
            mainCanvas.SetActive(!controlsOpen);
            controlsCanvas.SetActive(false);
            controlsOpen= false;
        }
    }

    protected void PlayCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    protected void StartNewGame()
    {
        
        canvasController.HidePlayerCursor();
        playerRef.GetComponent<PlayerGroundMovement>().enabled = true;
        playerRef.GetComponent<PlayerFlightMovement>().enabled = true;
        if (SteamManager.Initialized)
        {
            AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_000_Support");
        }
        SceneManager.LoadScene("TutorialIsland");
        RemoveAllListeners();


    }

    protected void CheckForSavedGame()
    {
        string[] saves = SaveLoadBase.GetAllSaves();

        if (saves.Length > 0)
        {
            Debug.Log("Save files found. Loading save window...");
            loadButton.interactable = true;
            LoadGame();
        }
        else
        {
            Debug.Log("No save files found. Load button disabled.");
            loadButton.interactable = false;
        }
    
    }

    protected void LoadGame()
    {
        mainCanvas.SetActive(false);
        GameObject canvasObj = new("SaveWindowCanvas");
        canvasObj.transform.SetParent(transform, false);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        currentSaveWindow = Instantiate(saveWindowPrefab, canvasObj.transform);
        var comp = currentSaveWindow.GetComponent<UI_SaveWindow>();
        comp.isSaving = false;
    }

    private void RemoveAllListeners()
    {
        startButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        controlsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
    }

    public void CloseSaveWindow()
    {
        Destroy(currentSaveWindow);
        mainCanvas.SetActive(true);
    }

    protected void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else        
Application.Quit();
#endif
    }
}
