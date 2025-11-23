using Steamworks;
using UnityEngine;

public class SteamInit : MonoBehaviour
{
    private static SteamInit instance;
    private static bool isInitialized = false;

    public static bool Initialized => isInitialized;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        if (Application.isEditor)
        {
            Debug.Log("[Steamworks.NET] Skipping Steam init in Editor.");
            isInitialized = false;
            return;
        }

        try
        {

            if (SteamAPI.RestartAppIfNecessary((AppId_t)480))
            {
                Debug.Log("[Steamworks.NET] Steam not running, quitting build.");
                Application.Quit();
                return;
            }
        }
        catch (System.DllNotFoundException e)
        {
            Debug.LogWarning("[Steamworks.NET] DLL not found: " + e + ". Skipping Steam init.");
            isInitialized = false;
            return;
        }

        // Initialize Steam API
        isInitialized = SteamAPI.Init();
        if (!isInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Steam must be running.");
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
            Debug.Log("[Steamworks.NET] Shutdown complete.");
        }
    }
}
