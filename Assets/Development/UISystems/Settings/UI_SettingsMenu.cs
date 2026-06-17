
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SettingsMenu : MonoBehaviour
{
    public GameObject parent;
    [Header("Settings")]
    [SerializeField] private Button closeSettingsButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Button videoBackButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button audioBackButton;
    [SerializeField] private Button accessButton;
    [SerializeField] private Button accessBackButton;

    [SerializeField] private GameObject videoParent;
    private bool videoOpen;
    [SerializeField] private GameObject audioParent;
    private bool audioOpen;
    [SerializeField] private GameObject accessParent;
    private bool accessOpen;
    UI_PauseMenu pauseRef;
    UI_MainMenu mainRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeSettingsButton.onClick.AddListener(CloseSettings);
        videoButton.onClick.AddListener(OpenVideoOptions);
        videoBackButton.onClick.AddListener(OpenVideoOptions);
        audioButton.onClick.AddListener(OpenAudioOptions);
        audioBackButton.onClick.AddListener(OpenAudioOptions);
        accessButton.onClick.AddListener(OpenAccessOptions);
        accessBackButton.onClick.AddListener(OpenAccessOptions);
        CheckIfMainMenu();
        SetFirstSettingsButton();
    }

    void CheckIfMainMenu()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            mainRef = FindAnyObjectByType<UI_MainMenu>();
        }
        else
        {
            pauseRef = FindAnyObjectByType<UI_PauseMenu>();
        }
    }

    public void SetFirstSettingsButton()
    {

            if (videoButton.gameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(videoButton.gameObject);
            }

    }

    protected void CloseSettings()
    {
        if (mainRef != null)
        {
            mainRef.OnSettingsOpen();
        }else if (pauseRef != null)
        {
            pauseRef.OnSettingsOpen();
        }
    }
    protected void OpenVideoOptions()
    {
        if (!videoOpen)
        {
            videoOpen = true;
            videoParent.gameObject.SetActive(true);
            videoParent.gameObject.GetComponent<UI_VideoOptions>().SetFirstVideoButton();
            Debug.Log("Hello");
        }
        else if (videoOpen)
        {
            videoOpen = false;
            videoParent.gameObject.SetActive(false);
            SetFirstSettingsButton();
        }
    }

    protected void OpenAudioOptions()
    {
        if (!audioOpen)
        {
            audioOpen = true;
            audioParent.gameObject.SetActive(true);
            audioParent.gameObject.GetComponent<UI_AudioOptions>().SetFirstAudioButton();
        }
        else if (audioOpen) 
        {
            audioOpen = false;
            audioParent.gameObject.SetActive(false);
            SetFirstSettingsButton();
        }
    }

    protected void OpenAccessOptions()
    {
        if (!accessOpen)
        {
            accessOpen= true;
            accessParent.gameObject.SetActive(true);
            accessParent.gameObject.GetComponent<UI_AccessOptions>().SetFirstAccessButton();
        }
        else if (accessOpen)
        {
            accessOpen = false;
            accessParent.gameObject.SetActive(false);
            SetFirstSettingsButton();
        }
    }


}
