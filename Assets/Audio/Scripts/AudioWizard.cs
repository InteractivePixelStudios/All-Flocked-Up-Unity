using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioWizard : MonoBehaviour // This is being made in a way where the AudioWizard can just be dropped into a scene and it will handle everything.
{
    public static AudioWizard Instance { get; private set; }
    [SerializeField] private GameObject ambienceLogicPrefab;

    [Header("Volume Settings")] // Volume will start at 50% by default - Master will start at 100%
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 0.5f;
    [Range(0, 1)] public float sfxVolume = 0.5f;
    [Range(0, 1)] public float ambienceVolume = 0.5f;

    [Header("Bus References")]
    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus ambienceBus;


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
}