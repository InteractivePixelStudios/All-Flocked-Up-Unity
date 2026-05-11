using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;

/*
---Notes---
- This script is almost identical to my others, I will consolidate them at some point.
- OnSceneLoaded is NOT trigger when the game starts, just so everyone is aware.
- This script will be an active living one, as I am not sure how we want music to be handled. Each level has a different music track? Day/Night? So on. 
    I will be changing this script as we go.
-
*/

public class AmbienceLogic_Music : MonoBehaviour
{
    [SerializeField] EventReference gameMusicTimelineEvent; // Assign in Inspector
    [SerializeField] EventReference mainMenuMusicTimelineEvent; // Assign in Inspector  
    [SerializeField] private List<string> nonGameSceneNames = new List<string> { "MainMenu" }; // Assign the name of your main menu scene in the Inspector
    [SerializeField] private String creditsSceneName = "CreditScene";
    private EventInstance gameTimeLineInstance;
    private EventInstance mainMenuTimeLineInstance;

    private bool gameMusicCreated = false; // This is to prevent multiple instances of the music timeline from being created.
    private bool mainMenuMusicCreated = false; // This is to prevent multiple instances of the main menu music timeline from being created.

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (gameMusicCreated) // I do not like this, at all. I will be changing this.
        {
            StopCurrentTimeline(gameTimeLineInstance);
            gameMusicCreated = false;
        }

        if (mainMenuMusicCreated)
        {
            if (currentScene.name == creditsSceneName)
            {
                return; // Don't stop the main menu music when loading the credits scene - for now this feels logical.
            }
            StopCurrentTimeline(mainMenuTimeLineInstance);
            mainMenuMusicCreated = false;
        }

        if (nonGameSceneNames.Contains(scene.name))
        {
            CreateMainMenuMusicTimeline();
            return; // Don't play game music in the main menu.
        }

        CreateGameMusicTimeline();
    }

    private void Start()
    {
        // Check the active scene at the start of the game and play the appropriate music.
        Scene currentScene = SceneManager.GetActiveScene();
        if (nonGameSceneNames.Contains(currentScene.name))
        {
            CreateMainMenuMusicTimeline();
        }
        else
        {
            CreateGameMusicTimeline();
        }
    }

    private void CreateGameMusicTimeline()
    {
        if (!gameMusicCreated)
        {
            gameTimeLineInstance = RuntimeManager.CreateInstance(gameMusicTimelineEvent);
            //gameTimeLineInstance.setTimelinePosition(390000);
            gameTimeLineInstance.start();
            gameMusicCreated = true;
        }
    }

    private void CreateMainMenuMusicTimeline()
    {
        if (!mainMenuMusicCreated)
        {
            mainMenuTimeLineInstance = RuntimeManager.CreateInstance(mainMenuMusicTimelineEvent);
            mainMenuTimeLineInstance.start();
            mainMenuMusicCreated = true;
        }
    }

    private void StopCurrentTimeline(EventInstance timelineInstance)
    {
        if (timelineInstance.isValid())
        {
            timelineInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            timelineInstance.release();
        }
    }
}
