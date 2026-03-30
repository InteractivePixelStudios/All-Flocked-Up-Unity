using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject controlsCanvas;
    public Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    private bool settingsOpen;
    private bool controlsOpen;
    private GameObject playerRef;
    private CameraController cameraRef;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject saveWindowPrefab;
    [SerializeField] private GameObject currentSaveWindow;
    private string savePath;
    private bool menuOpen;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] UI_LanguageSelector lang;
    public bool firstSelected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        startButton.onClick.AddListener(StartNewGame);
        loadButton.onClick.AddListener(CheckForSavedGame);
        settingsButton.onClick.AddListener(OnSettingsOpen);
        controlsButton.onClick.AddListener(OnControlsOpen);
        creditsButton.onClick.AddListener(PlayCredits);
        quitButton.onClick.AddListener(QuitGame);
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerRef = FindAnyObjectByType<PlayerFlightMovement>().gameObject;
        cameraRef = FindAnyObjectByType<CameraController>();
        menuOpen = true;
        playerRef.transform.position = playerSpawnPoint.transform.position ;
        playerRef.transform.rotation  = playerSpawnPoint.transform.rotation;
        cameraRef.transform.forward = playerSpawnPoint.transform.forward;
        cameraRef.transform.position = playerRef.transform.position+ cameraOffset;
        //playerRef.GetComponent<PlayerGroundMovement>().enabled = false;
        //playerRef.GetComponent<PlayerFlightMovement>().enabled = false;
        //cameraRef.enabled = false;
        canvasController.ShowPlayerCursor();
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
            SetSelectedObject(startButton.gameObject);
            firstSelected = true;
        }
        else return;
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
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
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
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    protected void PlayCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    protected void StartNewGame()
    {

        //playerRef.GetComponent<PlayerGroundMovement>().enabled = true;
        //playerRef.GetComponent<PlayerFlightMovement>().enabled = true;
        if (SteamManager.Initialized)
        {
            AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_000_Support");
        }
        Debug.Log("Loading Scene");
        canvasController.HidePlayerCursor();
        SceneManager.LoadScene("TutorialIsland"); // change after build
        //cameraRef.enabled = true;


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
        GameObject canvasObj = new GameObject("SaveWindowCanvas");
        canvasObj.transform.SetParent(transform, false);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        currentSaveWindow = Instantiate(saveWindowPrefab,canvasObj.transform);
        var comp = currentSaveWindow.GetComponent<UI_SaveWindow>();
        comp.isSaving = false;

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
