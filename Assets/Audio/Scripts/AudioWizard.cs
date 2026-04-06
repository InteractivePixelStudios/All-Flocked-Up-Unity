using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

/*
---Notes---
- Yes, I use a Coroutine - if this is an issue, I will change it to a better alternative - IPM
- Getting all buttons on load is not efficient, I will be changing this to a Editor system where all buttons are pre-registered with the AudioWizard - IPM
-
-
-
*/

[RequireComponent(typeof(SurfaceType))]
public class AudioWizard : MonoBehaviour // This is being made in a way where the AudioWizard can just be dropped into a scene and it will handle everything.
{

    public static AudioWizard Instance { get; private set; }
    [SerializeField] private GameObject ambienceLogicPrefab;

    [Header("Volume Settings")] // Volume will start at 50% by default - Master will start at 100%
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 0.7f;
    [Range(0, 1)] public float sfxVolume = 0.4f;
    [Range(0, 1)] public float ambienceVolume = 0.5f;

    [Header("Bus References")]
    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus ambienceBus;

    [Header("Event References")]
    [SerializeField] private EventReference buttonHoverEvent; // Assign in Inspector
    [SerializeField] private EventReference buttonClickEvent; // Assign in Inspector


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
        Debug.Log("Scene loaded: " + scene.name + ". Checking for buttons to add listeners to.");
        StartCoroutine(GetAllButtons()); // Start the coroutine to get all buttons in the scene and add listeners to them.
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

        InstantiateAmbienceLogic();
    }

    private void Start()
    {
        StartCoroutine(GetAllButtons()); // Start the coroutine to get all buttons in the scene and add listeners to them.
    }

    private void Update() // For now volume updates will be done in Update, later this can be event driven.
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambienceBus.setVolume(ambienceVolume);
    }

    public void PlayOneshotSound(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    // Create an instance of the AmbienceLogic prefab - Keeping the logic separate for organization and future updates.
    private void InstantiateAmbienceLogic()
    {
        GameObject ambienceLogic = Instantiate(ambienceLogicPrefab);
        ambienceLogic.name = "AmbienceLogic"; // Just a precaution.
        ambienceLogic.transform.parent = this.transform;
        //DontDestroyOnLoad(ambienceLogic);
    }

    public IEnumerator GetAllButtons()
    {
        yield return null; // Wait a frame to ensure all buttons are loaded in the scene.
        Debug.Log("Getting all buttons in the scene and adding listeners if missing.");
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsInactive.Include);

        foreach (var button in allButtons)
        {
            Debug.Log("Found button: " + button.name);
        }

        foreach (var button in allButtons)
        {
            // Add listener script if missing
            var listener = button.GetComponent<Audio_UI_Listener>();
            if (listener == null)
                listener = button.gameObject.AddComponent<Audio_UI_Listener>();

            // Hook click
            button.onClick.RemoveListener(listener.OnButtonClick); // Prevent duplicate listeners
            button.onClick.AddListener(listener.OnButtonClick);
        }
    }

    public void PlayButtonHoverSound()
    {
        RuntimeManager.PlayOneShot(buttonHoverEvent);
    }

    public void PlayButtonClickSound()
    {
        RuntimeManager.PlayOneShot(buttonClickEvent);
    }
}