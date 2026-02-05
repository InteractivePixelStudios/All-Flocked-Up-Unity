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
    [SerializeField] protected Vector3 scrollStartPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        continueButton.onClick.AddListener(Unpause);
        settingsButton.onClick.AddListener(OnSettingsOpen);
        controlsButton.onClick.AddListener(OnControlsOpen);
        saveQuitButton.onClick.AddListener(OnSaveAndQuit);
        EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
        scrollStartPosition = scrollPanel.transform.position;
    }

    void ScrollCanvasUp()
    {

        Vector2 pos = scrollPanel.anchoredPosition;
        pos.y = Mathf.Lerp(pos.y, scrollPanel.anchoredPosition.y + 2000f, Time.unscaledDeltaTime * 1.5f);
        scrollPanel.anchoredPosition = pos;

    }

    void ScrollCanvasDown()
    {
        Vector2 pos = scrollPanel.anchoredPosition;
        pos.y = Mathf.Lerp(pos.y, scrollStartPosition.y, Time.unscaledDeltaTime * 1.5f);
        scrollPanel.anchoredPosition = pos;




    }
    public void Unpause()
    {
        var controller = FindFirstObjectByType<UI_CanvasController>();
        controller.ResumeGame();
        controller.HidePlayerCursor();
    }

    protected virtual void OnSettingsOpen()
    {
        if (!settingsOpen)
        {
            settingsOpen = true;
            ScrollCanvasUp();
            settingsCanvas.SetActive(true);
            settingsCanvas.GetComponent<UI_SettingsMenu>().SetFirstSettingsButton();
            Debug.Log("Open");

        }
        else
        {
            settingsOpen = false;
            ScrollCanvasDown();
            settingsCanvas.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
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
