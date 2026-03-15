using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class AchievementList : MonoBehaviour
{
    [SerializeField] private List<AchievementInfo> achievementList = new();
    [SerializeField] private Dictionary<string,AchievementInfo> achievementsComplete = new();

    [SerializeField] private bool statsLoaded;

    private Callback<UserStatsReceived_t> statsReceived;



    private void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (!SteamManager.Initialized)
        {
            Debug.LogWarning("Steam not initialized.");
            return;
        }

        statsReceived = Callback<UserStatsReceived_t>.Create(OnStatsReceived);
        BuildDictionary();
        SteamUserStats.RequestUserStats(SteamUser.GetSteamID());
    }

    private void BuildDictionary()
    {
        achievementsComplete.Clear();

        foreach (var ach in achievementList)
        {
            if (!achievementsComplete.ContainsKey(ach.achievementID))
                achievementsComplete.Add(ach.achievementID, ach);
        }
    }

    private void OnStatsReceived(UserStatsReceived_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Failed to load Steam stats.");
            return;
        }

        statsLoaded = true;

        // Sync local data with Steam
        foreach (var ach in achievementsComplete.Values)
        {
            bool unlocked;
            SteamUserStats.GetAchievement(ach.achievementID, out unlocked);
            ach.unlocked = unlocked;
        }

        Debug.Log("Steam achievements loaded.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckAchievement(string name)
    {
        if (!statsLoaded) return false;

        if (!achievementsComplete.TryGetValue(name, out var achievement))
        {
            Debug.LogWarning($"Achievement not found: {name}");
            return false;
        }

        return achievement.unlocked;
    }

   public void CompleteAchievement(string name)
    {
        if (!statsLoaded) return;

        if (!achievementsComplete.TryGetValue(name, out var achievement))
        {
            Debug.LogWarning($"Achievement not found: {name}");
            return;
        }

        if (achievement.unlocked)
            return;

        achievement.unlocked = true;
        TriggerSteamAchievement(achievement.achievementID);
    }

    void TriggerSteamAchievement(string ID)
    {
        Debug.Log("SteamAchCalled");
        if (!SteamManager.Initialized)
            return;
        SteamUserStats.SetAchievement(ID);
        SteamUserStats.StoreStats();
    }
}
