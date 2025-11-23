using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class UI_SaveWindow : MonoBehaviour
{
    [SerializeField] private SaveSlotManager slotManager;
    [SerializeField] private RectTransform saveBox;
    [SerializeField] private GameObject confirmWindow;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private GameObject saveSlotPrefab;
    private UI_SaveSlot pendingSlot;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, 0);
    public bool isSaving = true;
    public bool isQuitting = false;
    public string savePath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        confirmButton.onClick.AddListener(CloseConfirmWindow);
        cancelButton.onClick.AddListener(CancelConfirmWindow);
        InitSaveBox();
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
                slotUI.saveLoadButton.onClick.AddListener(slotUI.CallSave);
                Debug.Log("SaveListenerAdded");
            }
            else { slotUI.saveLoadButton.onClick.AddListener(slotUI.CallLoad); Debug.Log("LoadListenerAdded"); }
            

        }
    }


    public void OpenConfirmWindow(UI_SaveSlot slot)
    {
        pendingSlot = slot;
        confirmWindow.SetActive(true);
    }



    public void CloseConfirmWindow()
    {

    }

    public void CancelConfirmWindow()
    {
        confirmWindow.SetActive(false);
    }

    public async void DestroyWindow()
    {
        await Task.Delay(2000);
        Destroy(this.gameObject);
        if (isQuitting)
        {
            QuitGame();
        }
        else return;
        
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
