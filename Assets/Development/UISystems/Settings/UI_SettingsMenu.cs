
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsMenu : UI_PauseMenu
{
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeSettingsButton.onClick.AddListener(OnSettingsOpen);
        videoButton.onClick.AddListener(OpenVideoOptions);
        videoBackButton.onClick.AddListener(OpenVideoOptions);
        audioButton.onClick.AddListener(OpenAudioOptions);
        audioBackButton.onClick.AddListener(OpenAudioOptions);
        accessButton.onClick.AddListener(OpenAccessOptions);
        accessBackButton.onClick.AddListener(OpenAccessOptions);
    }


    protected new void OnSettingsOpen()
    {
        base.OnSettingsOpen();
    }

    protected void OpenVideoOptions()
    {
        if (!accessOpen)
        {
            accessOpen = true;
            videoParent.gameObject.SetActive(true);
            Debug.Log("Hello");
        }
        else if (accessOpen)
        {
            accessOpen = false;
            videoParent.gameObject.SetActive(false);
        }
    }

    protected void OpenAudioOptions()
    {
        if (!audioOpen)
        {
            audioOpen = true;
            audioParent.gameObject.SetActive(true);
        }
        else if (audioOpen) 
        {
            audioOpen = false;
            audioParent.gameObject.SetActive(false); }
    }

    protected void OpenAccessOptions()
    {
        if (!accessOpen)
        {
            accessOpen= true;
            accessParent.gameObject.SetActive(true);
        }
        else if (accessOpen)
        {
            accessOpen = false;
            accessParent.gameObject.SetActive(false);
        }
    }


}
