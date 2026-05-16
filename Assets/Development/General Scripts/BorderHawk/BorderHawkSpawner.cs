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

    async void SpawnHawk()
    {
        spawnPoint = playerRef.transform.position + offset;
        if (spawnedHawk == null)
        {
            await Task.Delay(2000);
            spawnedHawk = Instantiate(hawkPrefab, spawnPoint, Quaternion.identity);
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
            ShowWarning();
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hudRef.readyToSpawn)
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
