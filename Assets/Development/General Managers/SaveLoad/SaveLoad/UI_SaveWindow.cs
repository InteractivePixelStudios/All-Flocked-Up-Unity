using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_SaveWindow : MonoBehaviour
{
    [SerializeField] private SaveSlotManager slotManager;
    [SerializeField] private RectTransform saveBox;
    [SerializeField] private GameObject confirmWindow;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button closeWindowButton;

    [SerializeField] private GameObject saveSlotPrefab;
    private UI_SaveSlot pendingSlot;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, 0);
    public bool isSaving = true;
    public bool isQuitting = false;
    public string savePath;
    [SerializeField] private GameObject warningPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        confirmButton.onClick.AddListener(ConfirmAction);
        cancelButton.onClick.AddListener(CancelConfirmWindow);
        closeWindowButton.onClick.AddListener(CloseWindow);
        InitSaveBox();
    }

    public void SetFirstSaveButton()
    {
        EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
    }

    private void InitSaveBox()
    {
        List<SaveSlotInfo> slots = SaveSlotManager.GetAllSlots();
        foreach (Transform child in saveBox)
            Destroy(child.gameObject);

        for (int i = 0; i < slots.Count; i++)
        {
            var obj = Instantiate(saveSlotPrefab, saveBox);
            var slotUI = obj.GetComponent<UI_SaveSlot>();
            slotUI.Init(slots[i], slotManager, this,isSaving);
            slotUI.saveWindow = this;
            if (isSaving)
            {
                slotUI.saveLoadButton.onClick.AddListener(() =>
                {
                    OpenConfirmWindow(slotUI);
                });
                Debug.Log("SaveListenerAdded");
            }
            else
            {
                slotUI.saveLoadButton.onClick.AddListener(() =>
                {
                    OpenConfirmWindow(slotUI);
                });
            }


        }
    }


    public void OpenConfirmWindow(UI_SaveSlot slot)
    {
        pendingSlot = slot;
        confirmWindow.SetActive(true);
    }



    public void CloseConfirmWindow()
    {
        CloseWindow();
    }

    public void CancelConfirmWindow()
    {
        confirmWindow.SetActive(false);
    }

    public async void ShowWarning()
    {
        warningPanel.SetActive(true);
        await Task.Delay(2000);
        warningPanel.SetActive(false);
    }

    void CloseWindow()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            var menu = FindAnyObjectByType<UI_MainMenu>();
            menu.CloseSaveWindow();
        }
        else { 
            var menu = FindAnyObjectByType<UI_PauseMenu>();
            menu.CloseSaveWindow();
        }

    }
    public void ConfirmAction()
    {
        if (pendingSlot == null) return;

        if (isSaving)
        {
            pendingSlot.CallSave();
        }
        else
        {
            pendingSlot.CallLoad();
        }
        confirmWindow.SetActive(false);
    }

    public async void DestroyWindow()
    {
        await Task.Delay(2000);
        if (isQuitting)
        {
            QuitGame();
        }
        else Destroy(this.gameObject);

    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
