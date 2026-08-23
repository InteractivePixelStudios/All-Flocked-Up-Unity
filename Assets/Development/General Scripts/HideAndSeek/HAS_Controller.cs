using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HAS_Controller : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] float countdownTimer;
    [SerializeField] bool countdownComplete = false;
    [SerializeField] private HAS_Info currentInfo;

    [SerializeField] private float hideSpotCount;
    private List<GameObject> hideSpots = new();

    [SerializeField] private float npcCount;
    [SerializeField] private HAS_NPC npcPrefab;
    private List<HAS_NPC> hideNPCs = new();
    [SerializeField] private GameObject spawnLocation;

    [SerializeField] private float gameTimer;
    [SerializeField] bool isStarted;
    [SerializeField] bool timerComplete;
    [SerializeField] private int foundCount;
    bool gameComplete;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
    }

    private void Update()
    {
        if ( isStarted && !countdownComplete)
        {
            countdownTimer -= Time.deltaTime;
            if(countdownTimer <= 0) { countdownComplete = true; }
        }
        if (countdownComplete && !timerComplete)
        {
            gameTimer -=Time.deltaTime;
            if(foundCount == npcCount && !gameComplete) { timerComplete = false; CheckOutcome(timerComplete, foundCount); }
            else if(gameTimer <= 0 &&!gameComplete ) { timerComplete = true; CheckOutcome(timerComplete, foundCount); }
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
        countdownTimer = 5f;
        isStarted = true;
    }

    public void StartHideAndSeek()
    {
        StartCountdown();
        SpawnHideNPC();
        UI_HudController.Instance.ShowHASIcon();
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
        if (!gameComplete)
        {
            if (found == npcCount && !timeComplete)
            {
                HASCompleted();
                gameComplete = true;
            }
            else if (timeComplete && found != npcCount)
            {
                HASFailed();
                gameComplete = true;
            }
            else return;
        }
        else return;
    }

    void HASCompleted()
    {
        if (canvasController != null)
        {
            canvasController.SpawnHASComplete();
            ResetGame();
        }
        
    }

    void HASFailed()
    {
        if(canvasController!=null)
        {
            canvasController.SpawnHASComplete();
            ResetGame();
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
        UI_HudController.Instance.UpdateHASText(foundCount);
    }

    public GameObject GetSpawnLocation()
    {
        return spawnLocation;
    }

    private void ResetGame()
    {
        DestroyNPCs();
        gameComplete = false;
        timerComplete = false;
        countdownComplete = false;
        UI_HudController.Instance.HideHASIcon();
    }

    void DestroyNPCs()
    {
        foreach(var npc in hideNPCs)
        {
            Destroy(npc.gameObject);
        }
    }


}
