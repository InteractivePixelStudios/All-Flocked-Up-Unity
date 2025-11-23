using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BorderHawkSpawner : MonoBehaviour
{
    [SerializeField] private List<BorderHawkSpawner> zones = new();
    [SerializeField] private GameObject hawkPrefab;
    [SerializeField] private GameObject playerRef;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject spawnedHawk;
    [SerializeField] private Vector3 spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var zoneArray = Object.FindObjectsByType<BorderHawkSpawner>(FindObjectsSortMode.None);
        foreach (var zone in zoneArray)
        {
            zones.Add(zone);
        }
    }

    async void SpawnHawk()
    {
        spawnPoint = playerRef.transform.position + offset;
        if(spawnedHawk == null)
        {
            await Task.Delay(2000);
            spawnedHawk = Instantiate(hawkPrefab, spawnPoint, Quaternion.identity);
        }

    }

    async void DestroyHawk()
    {
        if(spawnedHawk != null)
        {
            await Task.Delay(2000);
            Destroy(spawnedHawk.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            SpawnHawk();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DestroyHawk();
        }
    }


}
