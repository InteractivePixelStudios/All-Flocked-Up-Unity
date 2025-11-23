using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class SaveSlotManager
{
    private const int maxSlots = 5;

    public static List<SaveSlotInfo> GetAllSlots()
    {
        List<SaveSlotInfo> slots = new();

        for (int i = 1; i < maxSlots; i++)
        {
            string path = GetSlotPath(i);
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    if (json.StartsWith("{") == false)
                        json = SaveLoadBase.DecryptWrapper(json);
                    SaveData data = JsonUtility.FromJson<SaveData>(json);

                    slots.Add(new SaveSlotInfo
                    {
                        slotIndex = i,
                        exists = true,
                        playerName = data.playerName,
                        currentLevel = data.level,
                        lastSaved = data.lastSaved

                    }) ;
                }
                catch
                {
                    slots.Add(new SaveSlotInfo
                    {
                        slotIndex = i,
                        exists = false,
                        playerName = "Cuorrupted",
                        currentLevel = 0,
                        lastSaved = DateTime.MinValue

                    });
                }
            }
            else
            {
                slots.Add(new SaveSlotInfo
                {
                    slotIndex = i,
                    exists = false,
                    playerName = "Empty Slot",
                    currentLevel = 0,
                    lastSaved = DateTime.MinValue

                });
            }
        }
        return slots;
    }

    public static void SaveToSlot(int slot, SaveData data,bool encrypt = true)
    {
        SaveLoadBase.Save(data, slot, encrypt);
    }

    public static SaveData LoadFromSlot(int slot, bool encrypted = true)
    {
        return SaveLoadBase.Load(slot, encrypted);
    }

    public static void DeleteSlot(int slot)
    {
        SaveLoadBase.DeleteSave(slot);
    }

    public static string GetSlotPath(int slot)
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        return Path.Combine(basePath, $"SaveSlot_{slot}.dat");
    }
}
