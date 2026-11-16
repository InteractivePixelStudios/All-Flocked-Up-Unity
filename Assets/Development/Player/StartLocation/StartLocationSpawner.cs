using Unity.Cinemachine;
using UnityEngine;

public class StartLocationSpawner : Singleton<StartLocationSpawner>
{
    [SerializeField] private StartLocationComponent startLocation;

    void FindLocationAndMove()
    {
        startLocation = FindAnyObjectByType<StartLocationComponent>();
        gameObject.transform.position = startLocation.transform.position;
    }

    private void OnLevelWasLoaded(int level)
    {
        FindLocationAndMove();
        enabled = true;
    }

}
