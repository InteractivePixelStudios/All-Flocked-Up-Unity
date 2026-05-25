using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class BorderHawkSpawner : MonoBehaviour
{
    [SerializeField] private List<BorderHawkSpawner> zones = new();
    [SerializeField] private GameObject hawkPrefab;
    [SerializeField] private GameObject playerRef;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject spawnedHawk;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private UI_HudController hudRef;
    bool isSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hudRef = FindAnyObjectByType<UI_HudController>();
        var zoneArray = Object.FindObjectsByType<BorderHawkSpawner>();
        foreach (var zone in zoneArray)
        {
            zones.Add(zone);
        }
    }

    private void Update()
    {

    }

     void SpawnHawk()
    {
        spawnPoint = playerRef.transform.position + offset;
        if (spawnedHawk == null)
        {
            spawnedHawk = Instantiate(hawkPrefab, spawnPoint, Quaternion.identity);
            isSpawned = true;
        }
        else return;

    }

    private void ShowWarning()
    {
        hudRef.ShowHawkWarning();
    }

    private void HideWarning()
    {
        hudRef.HideHawkWarning();
    }

    void DestroyHawk()
    {
        if(spawnedHawk != null)
        {
            
            Destroy(spawnedHawk.gameObject);
            isSpawned = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            ShowWarning();
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hudRef.readyToSpawn && !isSpawned)
            {
                SpawnHawk();
                hudRef.readyToSpawn = false;
            }
            else return;
        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideWarning();
            DestroyHawk();
            hudRef.readyToSpawn = true;
        }
    }


}
