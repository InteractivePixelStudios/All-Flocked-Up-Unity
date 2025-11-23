using System;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public string version;
    public string playerName;
    public int level;
    public float health;
    public Vector3 position;
    public int trinkets;
    public DateTime lastSaved;

    public SaveData()
    {
        version = SaveLoadBase.currentVersion;
        lastSaved = DateTime.Now;
    }
}
