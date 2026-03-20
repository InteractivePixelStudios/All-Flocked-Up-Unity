using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSaveLoadHandler : MonoBehaviour
{
    public SaveData saveData;
    [SerializeField] private int saveSlot = 0;
    [SerializeField] private float maxTime;
    [SerializeField] private float timer;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveData = new SaveData();
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        if(saveData.sceneName == null)
        {
            saveData.sceneName = SceneManager.GetActiveScene().name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime;
        if( timer < 0)
        {
            UpdateSaveFile();
            timer = maxTime;
        }
    }

    private void UpdateSaveFile()
    {
        if (player == null) return;
        saveData.playerName = "Peep";  
        saveData.sceneName = SceneManager.GetActiveScene().name;
        saveData.health = player.GetComponent<PlayerHealth>().currentHealth;
        saveData.level = player.GetComponent<EXPSystem>().PLAYERLEVEL;
        saveData.position = player.transform.position;
        saveData.rotation = player.transform.rotation;
        saveData.trinkets = player.GetComponent<PlayerWingventory>().playerTrinketQuantity;
        saveData.lastSaved = System.DateTime.Now;
        saveData.version = SaveLoadBase.currentVersion;
        saveData.poop = player.GetComponent<PoopSystem>().GetMaxPoop();
        saveData.stamina = player.GetComponent<StaminaSystem>().GetCurrentStamina();
        saveData.inventory = player.GetComponent<PlayerWingventory>().inventory;
        saveData.activeQuests = player.GetComponent<QuestLog>().activeQuests;
        saveData.completedQuests = player.GetComponent<QuestLog>().completedQuests;
        saveData.completedRaces = FindAnyObjectByType<RaceBase>().completedRaces;
        saveData.timeOfDay = FindAnyObjectByType<S_DayNightCycle>().timeOfDay;
        saveData.playerSkin = player.GetComponent<PlayerSkinSelector>().GetCurrentMaterial();
        SaveSlotManager.SaveToSlot(saveSlot, saveData, true);

        Debug.Log($"Autosaved to slot {saveSlot} at {saveData.lastSaved}");
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void LoadPlayerData()
    {
        SaveData loaded = SaveSlotManager.LoadFromSlot(saveSlot, true);
        if (loaded == null) return;
        LoadLevel(loaded.sceneName);
        player.GetComponent<PlayerHealth>().currentHealth = loaded.health;
        player.GetComponent<StaminaSystem>().SetMaxStamina(loaded.stamina);
        player.GetComponent<PoopSystem>().SetMaxPoop(loaded.poop);
        player.GetComponent<EXPSystem>().GiveLevels(loaded.level);
        player.GetComponent<QuestLog>().activeQuests = loaded.activeQuests;
        player.GetComponent<QuestLog>().completedQuests = loaded.completedQuests;
        FindAnyObjectByType<RaceBase>().completedRaces = loaded.completedRaces;
        FindAnyObjectByType<S_DayNightCycle>().timeOfDay = loaded.timeOfDay;
        player.GetComponent<PlayerWingventory>().playerTrinketQuantity = loaded.trinkets;
        player.GetComponent<PlayerSkinSelector>().SetLoadedMaterial(loaded.playerSkin);
        player.transform.position = loaded.position;
        player.transform.rotation = loaded.rotation;
        saveData = loaded;
        SaveLoadBase.currentVersion = loaded.version;
        Debug.Log($"Loaded player from slot {saveSlot}");
    }

    public void ApplyLoadedData()
    {
        LoadPlayerData();
        player.GetComponent<PlayerGroundMovement>().enabled = true;
        player.GetComponent<PlayerFlightMovement>().enabled = true;
    }
}
