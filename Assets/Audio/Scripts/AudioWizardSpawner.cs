using Steamworks;
using UnityEngine;

public class AudioWizardSpawner : Singleton<AudioWizardSpawner>
{
    [SerializeField]AudioWizard prefab;
    AudioWizard spawned;
    bool isSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isSpawned)
        {
            SpawnWizard();
        }
        else return;
    }

    void SpawnWizard()
    {
        spawned = Instantiate(prefab);
        isSpawned = true;
    }
}
