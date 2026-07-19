using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


//this script exists to control the state of the player and alternate between modes like camera, and normal behavior. 
//The First iteration will only have groundmove, camera, and flymove. The controller will be used to regulate what the player can do based on state
//camera state - movement disabled, camera will instead move, can take pictures
//groundmove state - normal movement when not flying
//flymove state - movement while flying


//will require integration into the player movement system and other scripts to function



public enum PlayerState {GroundMove, PhotoMode, FlyMove}
public class PlayerStateController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; } = PlayerState.GroundMove;

    [SerializeField] private string defaultMapName = "UI";
    [SerializeField] private bool logMapSwitches = true;

    [SerializeField] private string[] uiMapScenes = { "MainMenu", "CreditScene" };
    private void Start()
    {
        var actions = InputSystem.actions;
        if (actions == null)
        {
            Debug.LogWarning("Player State Controller: InputSystem.actions is null. No project wide actions asset assigned.");
            return;
        }

        foreach (var map in actions.actionMaps)
        {
            if (map.name == defaultMapName) map.Enable();
            else map.Disable();
            
        }
    }
    
    public void EnterPhotoMode()
    {
        CurrentState = PlayerState.PhotoMode;
    }

    public void ExitPhotoMode()
    {
        CurrentState = PlayerState.GroundMove;
    }

    public void EnterFlyMode()
    {
        CurrentState = PlayerState.FlyMove;
        if (SteamManager.Initialized)
        {
            //AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_003_Fly");
            
            // ^^^^this and other achievement triggers might benefit from using an observer pattern and events
            //
        }
    }

    public void ExitFlyMode()
    {
        CurrentState = PlayerState.GroundMove;
    }
    
    //ACTION MAP HELPER FUNCTIONS

    public void SwitchToPlayerMap([CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
    {
        
        SwitchMap("Player", callerMethod, callerFile, callerLine);
    }

    public void SwitchToUIMap([CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
    {
        SwitchMap("UI", callerMethod, callerFile, callerLine);
    }

    public void SwitchToPhotoModeMap([CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
    {
        SwitchMap("PhotoMode", callerMethod, callerFile, callerLine);
    }

    private void SwitchMap(string mapName, string callerMethod, string callerFile,  int callerLine)
    {
        if (logMapSwitches)
        {
            string fileName = Path.GetFileName(callerFile);
            Debug.Log($"[PSC] Switch -> {mapName} <--- {fileName}:{callerLine}({callerMethod})");
        }
        
        
        var actions = InputSystem.actions;
        if (actions == null) return;
        foreach(var map in actions.actionMaps)
        {
            if (map.name == mapName) map.Enable();
            else  map.Disable();
        }

    }

    private void OnEnable()
    {
        //subscribe to scenemanager, so can auto turn on/off playermaps
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        //unsub
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool wantsUI = System.Array.IndexOf(uiMapScenes, scene.name) >= 0;
        if (wantsUI) SwitchToUIMap();
        else SwitchToPlayerMap();

    }
    
}
