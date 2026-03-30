using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class UI_MainMap : MonoBehaviour
{
    [SerializeField] private List<Sprite> mapImages = new();
    [SerializeField] private GameObject[] markerOverlays;
    [SerializeField] private GameObject markerIconPrefab;
    [SerializeField] private Image bgImage;

    private void Start()
    {
        SwitchMapImage();
    }
    private void SwitchMapImage()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) { return; }
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TutorialIsland"))
        {
            markerOverlays[0].SetActive(true);
            markerOverlays[1].SetActive(false);
            bgImage.sprite = mapImages[0];
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("KensingtonMarket"))
        {
            markerOverlays[0].SetActive(false);
            markerOverlays[1].SetActive(true);
            bgImage.sprite = mapImages[1];
        }
        else
        {
            markerOverlays[0].SetActive(false);
            markerOverlays[1].SetActive(false);
            bgImage.sprite = mapImages[1];
        }
    }

    void PlaceMarkerIcon()
    {
       
    }

}
