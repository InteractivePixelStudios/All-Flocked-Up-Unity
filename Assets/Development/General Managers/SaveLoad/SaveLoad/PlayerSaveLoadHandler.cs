using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSaveLoadHandler : MonoBehaviour
{
    public SaveData saveData;
    [SerializeField] private int saveSlot = 0;
    [SerializeField] private float maxTime;
    [SerializeField] private float timer;
    private GameObject player;
    SaveData pendingData;
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
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void CallUpdateSave()
    {
        UpdateSaveFile();
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
        saveData.npcData = FindObjectsByType<NPCBase>().ToList();
        //var questobj = GameObject.FindObjectsByType(typeof(IQuestMechanic));
        //foreach(var item in questobj) { var trans = GameObject.item.transform; saveData.questObjects.Add(item) }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TutorialIsland"))
        {
            if (saveData.tutData != null)
            {
                saveData.tutData.Clear();
                var manager = GameObject.FindAnyObjectByType<TutorialManager>();
                if (manager != null)
                {
                    saveData.tutData = manager.ReturnTutData();
                }
            }
        }
        SaveSlotManager.SaveToSlot(saveSlot, saveData, true);

        Debug.Log($"Autosaved to slot {saveSlot} at {saveData.lastSaved}");
    }

    public void LoadLevel()
    {
        pendingData = SaveSlotManager.LoadFromSlot(saveSlot, true);
        if (pendingData == null) return;
        Debug.Log(pendingData.sceneName);

        SceneManager.LoadSceneAsync(pendingData.sceneName);
    }

    //add in list for saving NPC states....
    public async void LoadPlayerData()
    {
        await Task.Yield();
        if (pendingData == null)
        {
            pendingData = SaveSlotManager.LoadFromSlot(saveSlot, true);
            if (pendingData == null) return;
        }
        if(pendingData!=null)
        {
            player.GetComponent<PlayerHealth>().currentHealth = pendingData.health;
            player.GetComponent<StaminaSystem>().SetMaxStamina(pendingData.stamina);
            player.GetComponent<PoopSystem>().SetMaxPoop(pendingData.poop);
            player.GetComponent<EXPSystem>().GiveLevels(pendingData.level);
            player.GetComponent<QuestLog>().activeQuests = pendingData.activeQuests;
            player.GetComponent<QuestLog>().completedQuests = pendingData.completedQuests;
            FindAnyObjectByType<RaceBase>().completedRaces = pendingData.completedRaces;
            FindAnyObjectByType<S_DayNightCycle>().timeOfDay = pendingData.timeOfDay;
            player.GetComponent<PlayerWingventory>().playerTrinketQuantity = pendingData.trinkets;
            player.GetComponent<PlayerSkinSelector>().SetLoadedMaterial(pendingData.playerSkin);
            player.transform.position = pendingData.position;
            player.transform.rotation = pendingData.rotation;
            InitNPC();
            if (pendingData.tutData != null)
            {
                    var manager = GameObject.FindAnyObjectByType<TutorialManager>();
                    if (manager != null)
                    {
                        manager.LoadSavedTut(pendingData.tutData);
                        Debug.Log("loaded tut info");
                    }
            }
            saveData = pendingData;
            SaveLoadBase.currentVersion = pendingData.version;
            Debug.Log($"Loaded player from slot {saveSlot}");
        }
    }

    private void InitNPC()
    {
        var found = FindObjectsByType<NPCBase>();
        NPCBase tempData;
        foreach(var saved in saveData.npcData)
        {
            foreach (var npc in found)
            {
                if (npc != null && saved == npc)
                {
                    tempData = saved;
                    npc.LoadData(saved);
                    Debug.Log(npc + "has loaded" + saved);
                }
            }
        }

    }

    public void ApplyLoadedData()
    {
        var canvas = FindAnyObjectByType<UI_CanvasController>();
        canvas.HidePlayerCursor();
        player.GetComponent<PlayerGroundMovement>().enabled = true;
        player.GetComponent<PlayerFlightMovement>().enabled = true;
        if (pendingData == null)
        {
            pendingData = SaveSlotManager.LoadFromSlot(saveSlot, true);
        }
        if (pendingData != null)
        {
            LoadLevel();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pendingData == null) return;

        player = FindAnyObjectByType<PlayerGroundMovement>()?.gameObject;

        if (player == null) return;

        LoadPlayerData();
    }
}
