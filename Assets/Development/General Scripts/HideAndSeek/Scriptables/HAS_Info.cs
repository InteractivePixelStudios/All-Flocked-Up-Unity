using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "HideAndSeekInfo", menuName = "Scriptable Objects/HideAndSeek")]
public class HAS_Info : ScriptableObject
{
    [SerializeField] private string HAS_ID;
    [SerializeField] private float hideSpotCount;
    [SerializeField] private List<GameObject> hideSpots = new();

    [SerializeField] private float npcCount;
    [SerializeField] private GameObject spawnLocation;

    [SerializeField] private float gameTimer;
    private void OnEnable()
    {
        FindRequiredObjects();
    }

    void FindRequiredObjects()
    {
        var found = FindObjectsByType<HAS_HideSpot>();
        foreach (var obj in found)
        {
            if(obj.GetHASID().Equals(HAS_ID))
            {
                hideSpots.Add(obj.gameObject);
            }
        }
        var spawn = FindObjectsByType<HAS_SpawnLoc>();
        foreach (var obj in spawn)
        {
            if (obj.GetHASID().Equals(HAS_ID))
            {
                spawnLocation = obj.gameObject;
                Debug.Log(spawnLocation);
            }
        }
    }

    public float GetHideCount()
    {
        return hideSpotCount;
    }

    public List<GameObject> GetHideSpotList()
    {
        return hideSpots;
    }

    public float GetNPCCount()
    {
        return npcCount;
    }

    public GameObject GetSpawnLocation() 
    { 
        return spawnLocation;
    }

    public float GetTimer()
    {
        return gameTimer;
    }

}

