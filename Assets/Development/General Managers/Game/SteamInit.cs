using Steamworks;
using UnityEngine;

public class SteamInit : MonoBehaviour
{
    private static SteamInit instance;
    private static bool isInitialized = false;
    public static bool Initialized => isInitialized;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (Application.isEditor)
        {
            isInitialized = false;
            return;
        }
        try
        {
            if (SteamAPI.RestartAppIfNecessary((AppId_t)4502950))
            {
                Application.Quit();
                return;
            }
        }
        catch (System.DllNotFoundException e)
        {
            isInitialized = false;
            return;
        }

        isInitialized = SteamAPI.Init();
        if (!isInitialized)
        {
            Debug.LogError("SteamAPI_Init() failed. Steam must be running.");
        }
        else
        {
            Debug.Log("[Steamworks.NET] Initialized successfully!");
        }
    }

    private void OnEnable()
    {
        if (isInitialized)
            SteamAPI.RunCallbacks();
    }

    private void Update()
    {
        if (isInitialized)
            SteamAPI.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        if (isInitialized)
        {
            SteamAPI.Shutdown();
            isInitialized = false;
        }
    }
}
