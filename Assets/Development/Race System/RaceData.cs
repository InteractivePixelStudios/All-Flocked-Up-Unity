using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.Localization;

//this is what holds all of the race info... to create a race, create>scriptableObjects>races and make a race data
//then fill in info

[CreateAssetMenu(fileName = "RaceData", menuName = "Scriptable Objects/RaceData")]
public class RaceData : ScriptableObject
{
    public string raceID;
    public float raceTime;
    public bool raceActive;
    public LocalizedString raceName;
    public LocalizedString raceDescription;
    public int raceRewards;
    public List<Transform> checkpointLocations = new();
    public List< RaceCheckpoint>  checkpointSpawns = new();
    public int numberOfCPURacers;
    public StartingLine startLine;

    //gets the checkpoints for given race based on raceID
    public void GetCheckPoints()
    {
        checkpointSpawns.Clear();
        RaceCheckpoint[] checkpoints = Object.FindObjectsByType<RaceCheckpoint>();
        foreach (var point in checkpoints)
        {
            if(point.raceID == raceID)
            {
                checkpointSpawns.Add(point);
            }
            else continue;
        }
        checkpointSpawns.OrderBy(cpoint => cpoint.checkpointNumber);

    }

    //gets startLine gameobject and uses transform for starting location spawning
    public StartingLine GetStartLine()
    {
        StartingLine[] line = Object.FindObjectsByType<StartingLine>();
        foreach(var obj in line)
        {
            if (obj.raceID == raceID)
            {
                startLine = obj;

            }

        }
       // startLine.SetRotationToCheckpoint(checkpointSpawns.Last());
        //Debug.Log(startLine);
        return startLine;

    }
    
}
