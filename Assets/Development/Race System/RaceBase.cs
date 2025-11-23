using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class RaceBase : MonoBehaviour
{
    [SerializeField] private GameObject playerRef => GetPlayer();
    [SerializeField] private UI_CanvasController canvasController;
    public RaceData raceData => GetRaceData(currentRaceGiver.raceData);
    [SerializeField] private RaceCheckpoint checkpointPrefab;
    public RaceGiver currentRaceGiver;
    public List<RaceCheckpoint> activeCheckpoints = new();
    public List<Transform> checkpointTransforms = new();
    public int checkpointIndex = 1;
    public RaceCheckpoint lastCheckpoint;
    public List<RaceData> completedRaces = new();
    public bool raceStarted;
    private bool raceFailed;
    private bool raceEnded;

    [SerializeField] private float raceTimer;
    [SerializeField] private float currentTime;
    [SerializeField] private bool timerStarted;
    [SerializeField] private bool countdownStarted;
    [SerializeField] private bool countdownComplete;
    [SerializeField] private StartingLine currentRaceStartingLine => raceData.GetStartLine();
    public StartingLine raceStartLine => currentRaceStartingLine;
    [SerializeField] private List<CPURacer> currentRacerList = new();
    [SerializeField] private CPURacer racerPrefab;
    [SerializeField] private List<GameObject> completedRacer = new();
    public float playerFinishTime;

    public float countdown = 5;
    public float recordTime;

    private RaceWallSpawner wallSpawner;

    private void Awake()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        wallSpawner = GetComponent<RaceWallSpawner>();
    }

    private void Start()
    {
        if (activeCheckpoints.Count > 0)
            lastCheckpoint = activeCheckpoints[activeCheckpoints.Count - 1];
    }
    private void Update()
    {
        if (!raceEnded && completedRacer.Count == currentRacerList.Count + 1)
        {
            raceEnded = true;
            RaceCompleted();
        }
        if (countdownStarted && countdown >= 0) { StartRaceCountdown(); return; }
        else if (countdown <= 0 && !timerStarted) { SetRacerMovement(); StartRaceTimer(raceData.raceTime); }
        else if (raceStarted && timerStarted && currentTime >= 0) { UpdateRaceTimer(); }
        else { return; }
    }

    private void StartRaceCountdown()
    {
        if (countdownStarted)
        {
            countdown -= Time.deltaTime;
            return;
        }
        else if (countdown <= 0)
        {
            countdownComplete = true;
        }
    }

    private float StartRaceTimer(float raceTime)
    {
        StartPlayerMove();
        currentTime = raceTime;
        timerStarted = true;
        return currentTime;
    }

    private void UpdateRaceTimer()
    {
        if (timerStarted)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !raceFailed) { raceFailed = true; RaceFailed(); }
        }
    }

    private GameObject GetPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return player;
    }

    private RaceData GetRaceData(RaceData data)
    {
        var temp = data;
        return temp;
    }

    private void GetCheckpointLocationAndClear()
    {
        raceData.GetCheckPoints();
        //raceData.checkpointSpawns = activeCheckpoints;
        foreach (var checkpoint in raceData.checkpointSpawns)
        {
            //checkpointTransforms.Add(checkpoint.transform);
            activeCheckpoints.Add(checkpoint);
        }
        activeCheckpoints = activeCheckpoints.OrderBy(cpoint => cpoint.checkpointNumber).ToList();
        SpawnCheckpoints();
    }

    public void InteractWithRaceGiver()
    {
        GetRaceData(currentRaceGiver.raceData);
        StartRace();
        countdownStarted = true;
    }

    public void StartRace()
    {
        StopPlayerMove();
        raceStarted = true;
        GetCheckpointLocationAndClear();
        Debug.Log("Race Started");
        canvasController.CloseRaceGiver();
        Debug.Log("canvasClosed");
        checkpointIndex = 1;
        SetStartLine();
        SpawnCPURacers();
    }

    private void SetRacerMovement()
    {
        countdownComplete = true;
        foreach (var racer in currentRacerList)
        {
            racer.StartMoving();
        }
    }

    private void SpawnCheckpoints()
    {
        foreach (var transform in checkpointTransforms)
        {
            var checkpoint = Instantiate(checkpointPrefab, transform.position, transform.rotation);
            activeCheckpoints.Add(checkpoint);
        }
        lastCheckpoint = activeCheckpoints[activeCheckpoints.Count - 1];
    }

    public void UpdateCheckpoints(int hitPoint)
    {
        Debug.Log($"HitPoint: {hitPoint}, Expected: {checkpointIndex}");

        for (int i = 0; i < activeCheckpoints.Count; i++)
        {
            if (hitPoint != checkpointIndex) { Debug.Log("Wrong or Last Checkpoint Missed"); return; }
            checkpointIndex++;
            // Debug.Log("Checkpoint Hit");
            //Debug.Log(activeCheckpoints.Count);
            //if (activeCheckpoints.Count == 1)
            //{
            //    // RaceCompleted();
            //}


            //activeCheckpoints.RemoveAt(0);
        }
    }

    private void RaceCompleted()
    {
        raceStarted = false;
        GetRaceResults();
        DestroyCheckpoints();
        raceStarted = false;
        DestroyRacers();
        canvasController.OpenRaceRewards();
    }

    private void RaceFailed()
    {
        StopPlayerMove();
        raceStarted = false;
        canvasController.OpenRaceFail();
    }


    private void DestroyCheckpoints()
    {
        Debug.Log("Destroyed?");
        foreach (var checkpoint in activeCheckpoints)
        {
            Destroy(checkpoint.gameObject);
        }
        activeCheckpoints.Clear();
    }

    public void GiveRewards()
    {
        var reward = raceData.raceRewards;
        FindFirstObjectByType<EXPSystem>().IncrementXP(reward);
    }
    private void SetStartLine()
    {
        Debug.Log(currentRaceStartingLine.name);
    }

    private void MovePlayerToStartLine()
    {
        playerRef.transform.position = raceStartLine.transform.position;
        playerRef.transform.rotation = raceStartLine.transform.rotation;
    }

    private void SetStartingRacerLocation()
    {
        //this will need to be changed depending on where the start line is located
        Vector3 offset = new Vector3(0, 0, 2);
        Vector3 row2offset = new Vector3(2, 0, 0);
        var gap = new Vector3(0, 0, 2);
        for (int i = 0; i < currentRacerList.Count; i++)
        {
            if (i >= currentRacerList.Count / 2)
            {
                currentRacerList[i].transform.position = raceStartLine.transform.position + row2offset;
                currentRacerList[i].transform.rotation = raceStartLine.transform.rotation;
                row2offset += gap;
                currentRacerList[i].SetMoveToLocation(1);
            }
            else
            {
                currentRacerList[i].transform.position = raceStartLine.transform.position + offset;
                currentRacerList[i].transform.rotation = raceStartLine.transform.rotation;
                offset += gap;
                currentRacerList[i].SetMoveToLocation(1);
            }
        }
    }

    private void SpawnCPURacers()
    {
        var racers = raceData.numberOfCPURacers;
        for (int i = racers; i > 0; i--)
        {
            CPURacer racer = Instantiate(racerPrefab);
            currentRacerList.Add(racer);
            SetStartingRacerLocation();
        }
        MovePlayerToStartLine();
    }


    public void ResetRace()
    {
        countdown = 5;
        countdownComplete = false;
        timerStarted = false;
        DestroyRacers();
        raceFailed = false;
        Debug.Log("ResetCalled");
        StartRace();
    }

    private void DestroyRacers()
    {
        foreach (var racer in currentRacerList)
        {
            Destroy(racer.gameObject);
        }
        currentRacerList.Clear();
    }

    public void AddRacerToCompleted(GameObject racer, RaceCheckpoint checkpoint)
    {
        if (!completedRacer.Contains(racer))
        {
            if (checkpoint == lastCheckpoint)
            {
                completedRacer.Add(racer);
                if (racer.CompareTag("Race"))
                {
                    racer.GetComponent<CPURacer>().finishTime = currentTime;
                }
                else if (racer.CompareTag("Player"))
                {
                    playerFinishTime = currentTime;
                    StopPlayerMove();
                }
                Debug.Log($"{racer.name} finished the race!");
            }
        }
    }

    private void GetRaceResults()
    {
        foreach (var racer in completedRacer)
        {
            if (racer.CompareTag("Race"))
            {
                var time = racer.GetComponent<CPURacer>().finishTime;
                canvasController.CollectRaceStandings(racer, time);
                Debug.Log("AddedCPU");
            }
            else if (racer.CompareTag("Player"))
            {
                canvasController.CollectRaceStandings(racer, playerFinishTime);
                Debug.Log("AddedPlayer");
            }
        }
    }

    public void StopPlayerMove()
    {
        playerRef.GetComponent<PlayerGroundMovement>().enabled = false;
    }

    public void StartPlayerMove()
    {
        playerRef.GetComponent<PlayerGroundMovement>().enabled = true;
    }

    [ContextMenu("RespawnPlayer")]
    public void RespawnPlayerAtLastPoint()
    {
        playerRef.transform.position = activeCheckpoints[checkpointIndex-1].transform.position;
        playerRef.transform.rotation = activeCheckpoints[checkpointIndex-1].transform.rotation;
    }

}
