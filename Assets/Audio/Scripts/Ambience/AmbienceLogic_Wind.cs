using UnityEngine;
using FMODUnity;
using System;
using UnityEngine.SceneManagement;

public class AmbienceLogic_Wind : MonoBehaviour
{
    private String CurrentSceneName;
    [SerializeField] EventReference windTimelineEvent; // Assign in Inspector
    private FMOD.Studio.EventInstance windTimelineInstance;
    private bool windCreated = false; // This is to prevent multiple instances of the wind timeline from being created.

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
        CurrentSceneName = scene.name;
        if (CurrentSceneName == "MainMenu")
        {
            return; // Don't play wind ambience in the main menu.
        }

        CreateWindTimeline();
    }

    private void CreateWindTimeline()
    {
        if (!windCreated)
        {
            windTimelineInstance = RuntimeManager.CreateInstance(windTimelineEvent);
            windTimelineInstance.start();
            windCreated = true;
        }
    }
}
