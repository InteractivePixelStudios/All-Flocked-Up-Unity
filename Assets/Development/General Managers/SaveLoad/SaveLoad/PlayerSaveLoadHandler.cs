using UnityEngine;

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
        player = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
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
        saveData.health = player.GetComponent<PlayerHealth>().currentHealth;
        //saveData.level = player.GetComponent<EXPSystem>().;
        saveData.position = player.transform.position;
        saveData.trinkets = player.GetComponent<PlayerWingventory>().playerTrinketQuantity;
        saveData.lastSaved = System.DateTime.Now;
        saveData.version = SaveLoadBase.currentVersion;
        SaveSlotManager.SaveToSlot(saveSlot, saveData, true);

        Debug.Log($"Autosaved to slot {saveSlot} at {saveData.lastSaved}");
    }

    public void LoadPlayerData()
    {
        SaveData loaded = SaveSlotManager.LoadFromSlot(saveSlot, true);
        if (loaded == null) return;
        //player.GetComponent<PlayerHealth>().currentHealth = loaded.health;
        //player.currentLevel = loaded.level;
        player.GetComponent<PlayerWingventory>().playerTrinketQuantity = loaded.trinkets;
        player.transform.position = loaded.position;
        saveData = loaded;
        Debug.Log($"Loaded player from slot {saveSlot}");
    }

    public void ApplyLoadedData()
    {

    }
}
