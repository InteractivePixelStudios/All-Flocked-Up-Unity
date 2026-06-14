using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HAS_Controller : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    float countdownTimer;
    bool countdownComplete = false;
    [SerializeField] private HAS_Info currentInfo;

    [SerializeField] private float hideSpotCount;
    private List<GameObject> hideSpots = new();

    [SerializeField] private float npcCount;
    [SerializeField] private HAS_NPC npcPrefab;
    private List<HAS_NPC> hideNPCs = new();
    [SerializeField] private GameObject spawnLocation;

    private float gameTimer;
    bool isStarted;
    bool timerComplete;
    [SerializeField] private int foundCount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
    }

    private void Update()
    {
        if (!countdownComplete)
        {
            countdownTimer -= Time.deltaTime;
        }
        if (countdownComplete && isStarted)
        {
            gameTimer -=Time.deltaTime;
            if(gameTimer <= 0 || foundCount == npcCount) { timerComplete = true; CheckOutcome(timerComplete, foundCount); }
        }
    }

    public void InitGameInfo(HAS_Info info)
    {
        currentInfo =  info;
        if(currentInfo != null)
        {
            bool loaded = false;
            if (!loaded)
            {
                hideSpotCount = currentInfo.GetHideCount();
                hideSpots = currentInfo.GetHideSpotList();
                npcCount = currentInfo.GetNPCCount();
                spawnLocation = currentInfo.GetSpawnLocation();
                gameTimer = currentInfo.GetTimer();
                loaded = true;
                if (loaded) { StartHideAndSeek(); }
            }
        }
    }

    void StartCountdown()
    {
        canvasController.SpawnHASCountdown();
    }

    public void StartHideAndSeek()
    {
        StartCountdown();
        SpawnHideNPC();
    }

    void SpawnHideNPC()
    {
        for (int i = 0; i < hideSpotCount; i++)
        {
            var npc = Instantiate(npcPrefab, spawnLocation.transform.position,spawnLocation.transform.rotation);
            hideNPCs.Add(npc);
            npc.SetMoveToLocation(hideSpots[i].transform);
            npc.SetNewHomeLoc(spawnLocation);
            npc.isMoving = true;
        }
    }

    void CheckOutcome(bool timeComplete, int found)
    {
        if (found == npcCount && !timeComplete)
        {
            HASCompleted();
        }
        else if (timeComplete && found != npcCount)
        {
            HASFailed();
        }
        else return;
    }

    void HASCompleted()
    {
        if (canvasController != null)
        {
            canvasController.SpawnHASComplete();
        }
        
    }

    void HASFailed()
    {
        if(canvasController!=null)
        {
            canvasController.SpawnHASComplete();
        }
    }

    public bool CheckForNPC(HAS_NPC npc)
    {
        if (hideNPCs.Contains(npc))
        {
            SetNPCFound(npc);
            return true;
        }
        else return false;
    }

    private void SetNPCFound(HAS_NPC npc)
    {
        hideNPCs.Remove(npc);
        foundCount++;
    }

    public GameObject GetSpawnLocation()
    {
        return spawnLocation;
    }


}
