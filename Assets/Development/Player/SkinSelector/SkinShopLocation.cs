using UnityEngine;

public class SkinShopLocation : MonoBehaviour
{
    public GameObject playerSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var comp = other.gameObject.GetComponent<PlayerSkinSelector>();
            comp.StartSkinSelector();

            if (SteamManager.Initialized)
            {
                AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_006_Skin");
            }
        }
    }
}
