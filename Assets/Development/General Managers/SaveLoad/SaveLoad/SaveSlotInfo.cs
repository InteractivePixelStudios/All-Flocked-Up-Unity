using System;

[System.Serializable]
public class SaveSlotInfo
{
    public int slotIndex;
    public bool exists;
    public string playerName;
    public int currentLevel;
    public DateTime lastSaved;

    public string GetFormattedDate()
    {
        return exists ? lastSaved.ToString("yyyy-MM-dd HH:mm") : "—";
    }
}