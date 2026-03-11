using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{

    public string version;
    public string playerName;
    public string sceneName;
    public int level;
    public int health;
    public Vector3 position;
    public Quaternion rotation;
    public int trinkets;
    public DateTime lastSaved;
    public int poop;
    public float stamina;
    public Dictionary<GameObject, int> inventory = new();
    public List<QuestRuntimeInstance> activeQuests = new();
    public List<QuestDetails> completedQuests = new();
    public List<RaceData> completedRaces = new();
    public float timeOfDay;
    public Material playerSkin;
    public PlayerPrefs playerPrefs;
    

    public SaveData()
    {
        version = SaveLoadBase.currentVersion;
        lastSaved = DateTime.Now;
    }
}
