using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RaceWallSpawner : MonoBehaviour
{

    public float wallHeight = 3f;
    public float wallThickness = 0.2f;
    public Material wallMaterial;
    public bool wallsVisible = false;
    public string wallLayerName = "TrackWall";
    private List<RaceCheckpoint> checkpoints = new();
    private List<GameObject> activeWalls = new();
    private RaceBase race;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkpoints = race.activeCheckpoints;
        race = GetComponentInParent<RaceBase>();
        foreach(var point in checkpoints)
        {
            Debug.Log(point.name);
        }
    }

    public void GetCheckpointLocations()
    {
        //checkpoints = race.activeCheckpoints;
        foreach(var point in race.activeCheckpoints)
        {
            checkpoints.Add(point);
            Debug.Log(point.name);
        }
    }
    public void SpawnAllRaceWalls()
    {

    }

    public void DestroyRaceWalls()
    {

    }

    private void SpawnWall(GameObject wall, Vector3 start, Vector3 end)
    {

    }

}
