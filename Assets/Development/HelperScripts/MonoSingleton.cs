using UnityEngine;

public class MonoSingleton : MonoBehaviour
{
    private static GameObject instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this.gameObject;
        DontDestroyOnLoad(gameObject);
    }
}
