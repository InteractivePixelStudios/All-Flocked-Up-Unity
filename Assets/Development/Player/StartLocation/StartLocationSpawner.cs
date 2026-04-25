using Unity.Cinemachine;
using UnityEngine;

public class StartLocationSpawner : Singleton<StartLocationSpawner>
{

    [SerializeField] private StartLocationComponent startLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void FindLocationAndMove()
    {
        startLocation = FindAnyObjectByType<StartLocationComponent>();
        this.gameObject.transform.position = startLocation.transform.position;
    }

    private void OnLevelWasLoaded(int level)
    {
        FindLocationAndMove();
        this.enabled = true;
    }

}
