using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveSlot : MonoBehaviour
{
    [SerializeField] private SaveSlotInfo data;
    public SaveData saveData;
    private SaveSlotManager slotManager;
    public Image saveImage;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI lastSaveText;
    public Button saveLoadButton;

    private SaveSlotInfo slotInfo;
    public UI_SaveWindow saveWindow;
    public bool isSaving;



    private void CheckSaveLoad()
    {
        if (!isSaving)
        {
            CallLoad();
        }else { CallSave(); }
    }

    public void Init(SaveSlotInfo info, SaveSlotManager manager, UI_SaveWindow window, bool saving = true)
    {
        slotInfo = info;
        slotManager = manager;
        saveWindow = window;
        isSaving = saving;

        if (slotInfo.exists)
            UpdateSlotUI(slotInfo.playerName, slotInfo.lastSaved.ToString("g"));
        else
            UpdateSlotUI("Empty Slot", "--/--/--");

    }

    private void Start()
    {
        if (isSaving)
        {
            playerNameText.text = slotInfo.exists ? slotInfo.playerName : "Empty Slot";
            lastSaveText.text = slotInfo.exists ? slotInfo.lastSaved.ToString() : "--/--/--";
        }
        else
        {
            playerNameText.text = slotInfo.exists ? slotInfo.playerName : "Empty Slot";
            lastSaveText.text = slotInfo.exists ? slotInfo.lastSaved.ToString() : "--/--/--";
        }
    }
    private void GetSlot()
    {

    }

    public void CallSave()
    {
        SaveData data = FindFirstObjectByType<PlayerSaveLoadHandler>().saveData;
        SaveSlotManager.SaveToSlot(slotInfo.slotIndex, data, true);
        UpdateSlotUI(data.playerName,data.lastSaved.ToString());
        Debug.Log("SaveButtonPressed");
        saveWindow.DestroyWindow();
    }

    public void CallLoad()
    {
        {
            Debug.Log("ButtonPressed");

            SaveData loadedData = SaveSlotManager.LoadFromSlot(slotInfo.slotIndex, true);
            Debug.Log(loadedData);
            if (loadedData != null)
            {
                Debug.Log("SlotFound");
                var handler = FindFirstObjectByType<PlayerSaveLoadHandler>();
                handler.saveData = loadedData;
                handler.ApplyLoadedData(); 

                saveWindow.DestroyWindow();
                Debug.Log("SlotLoaded");
            }
            else
            {
                Debug.LogWarning($"No save found for slot {slotInfo.slotIndex}");
            }
        }
    }

    private void UpdateSlotUI(string name, string time)
    {
        playerNameText.text = name;
        lastSaveText.text = time;
    }



}
