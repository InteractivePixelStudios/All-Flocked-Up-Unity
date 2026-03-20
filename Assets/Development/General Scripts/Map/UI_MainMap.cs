using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UI_MainMap : MonoBehaviour
{
    [SerializeField] private List<Sprite> mapImages = new();
    private int levelIndex;
    [SerializeField] private GameObject[] markerOverlays;
    [SerializeField] private GameObject markerIconPrefab;
    [SerializeField] private Image bgImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void SwitchMapImage(int index)
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) { return; }
        bgImage.sprite = mapImages[index];
        if(index > 0)
        {
            markerOverlays[index - 1].SetActive(false);
        }
        markerOverlays[index].SetActive(true);
    }

    void PlaceMarkerIcon()
    {
       
    }

    private void OnLevelWasLoaded(int level)
    {
        levelIndex = level-1;
        SwitchMapImage(levelIndex);
    }
}
