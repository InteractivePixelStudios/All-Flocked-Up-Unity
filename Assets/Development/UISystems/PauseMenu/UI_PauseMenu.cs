using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] protected GameObject mainCanvas;
    [SerializeField] protected GameObject settingsCanvas;
    [SerializeField] protected GameObject controlsCanvas;
    [SerializeField] protected Button continueButton;
    [SerializeField] protected Button settingsButton;
    [SerializeField] protected Button controlsButton;
    [SerializeField] protected Button saveQuitButton;
    [SerializeField] protected bool settingsOpen;
    [SerializeField] protected bool controlsOpen;
    [SerializeField] protected GameObject saveWindowPrefab;
    [SerializeField] protected GameObject currentSaveWindow;
    [SerializeField] private RectTransform scrollPanel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        controlsCanvas.SetActive(false);
        continueButton.onClick.AddListener(Unpause);
        settingsButton.onClick.AddListener(OnSettingsOpen);
        controlsButton.onClick.AddListener(OnControlsOpen);
        saveQuitButton.onClick.AddListener(OnSaveAndQuit);
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        settingsCanvas.GetComponent<UI_SettingsMenu>().parent = this.gameObject;

    }
    public void Unpause()
    {
        var controller = FindAnyObjectByType<UI_CanvasController>();
        controller.ResumeGame();
        controller.HidePlayerCursor();
    }

    public virtual void OnSettingsOpen()
    {
        if (!settingsOpen)
        {
            settingsOpen = true;
            //mainCanvas.SetActive(false);
            //settingsCanvas.SetActive(true);
            settingsCanvas.GetComponent<UI_SettingsMenu>().SetFirstSettingsButton();
            Debug.Log("Open");

        }
        else
        {
            settingsOpen = false;
            //mainCanvas.SetActive(true);
            //settingsCanvas.SetActive(false);
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);

        }
    }

    protected virtual void OnControlsOpen()
    {
        if (!controlsOpen)
        {
            controlsOpen = true;
            mainCanvas.SetActive(false);
            controlsCanvas.SetActive(true);
            controlsCanvas.GetComponent<UI_ControlsMenu>().SetFirstControlsButton();

        }
        else
        {
            controlsOpen= false;
            mainCanvas.SetActive(true);
            controlsCanvas.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
        }
    }


    public void ClosePauseUI()
    {
        Destroy(this.gameObject);
    }

    //update this later to save game on close.... should prompt saveconfirm/slot
    protected void OnSaveAndQuit()
    {
        //for testing slots
        //SaveData data = new SaveData();
        //SaveSlotManager.SaveToSlot(0, data, true);

        currentSaveWindow = Instantiate(saveWindowPrefab,mainCanvas.gameObject.transform);
        var comp = currentSaveWindow.GetComponent<UI_SaveWindow>();
        comp.SetFirstSaveButton();
        comp.isQuitting = true;
        

    }


}
