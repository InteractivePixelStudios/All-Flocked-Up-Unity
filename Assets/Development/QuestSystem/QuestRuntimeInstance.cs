
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestRuntimeInstance
{

    public QuestDetails questData; //Quest Details Struct
    public string questID;
    public int currentStageIndex = 0; //Stage index; Each Quest has 1+ stages with 1+ Objectives.
    public Dictionary<string, int> objectiveProgress = new(); //Dictionary stores the objectives from the current Stage Index

    public bool IsComplete => currentStageIndex >= questData.stages.Length;
    private GameObject player;
    public QuestLog questLog;
    private EXPSystem expComp;
    private PlayerWingventory invComp;
    public float currentTime;

    public bool isQuestFailed = false;
    public bool isRetrySelected = false;

    public List<GameObject> questMechanicsObjects = new List<GameObject>();
    [SerializeField] private PlayerNavArrow arrowPointer;
    public GameObject destination;
    private int cachedExp;
    private int cachedTrinkets;
    private string[] itemRewards;

    public void Start()
    {
        
    }

    public int GetCachedExp()
    {
        return cachedExp;
    }

    public int GetCachedTrinkets()
    {
        return cachedTrinkets;
    }

    public string[] GetItemRewards()
    {
        return itemRewards;
    }

    //Gets objectives and for each sets an objectiveID
    public void StartQuest()
    {
        questLog = GameObject.FindAnyObjectByType<QuestLog>();
        player = questLog.gameObject;
        arrowPointer = player.GetComponent<PlayerNavArrow>();
        expComp = player.GetComponent<EXPSystem>();
        invComp = player.GetComponent<PlayerWingventory>();

        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            objectiveProgress[obj.objectiveID] = 0;
        }

        // finds quest mechanics
        GetQuestObjects();
        GetQuestID(questData.questID);
        GetObjectiveDestination(objectives[0].objectiveID);
        arrowPointer.destination = destination;
        arrowPointer.EnablePointerArrow(destination);


    }

    //gets and sets QuestID to variable
    public string GetQuestID(string questid) => questID;

    //gets the quest mechanic gameobjects associated to that questID
    public void GetQuestObjects()
    {

        questMechanicsObjects.Clear();
        IQuestMechanic[] mechanics = Object.FindObjectsByType<MonoBehaviour>()
    .OfType<IQuestMechanic>()
    .ToArray();
        foreach (var mechanic in mechanics)
        {
            if (mechanic is MonoBehaviour monoBehaviour && objectiveProgress.Keys.Contains<string>(mechanic.GetObjectiveID()))
            {
                GameObject mechanicObject = monoBehaviour.gameObject;
                questMechanicsObjects.Add(mechanicObject);
              
            }
            else continue;
        }
    }



    //Checks if Objective Complete and sets & returns ObjectiveDetails array 0 (empty array).Returns current stage objectives to complete (next objective)
    public ObjectiveDetails[] GetCurrentObjectives()
    {
        if (IsComplete) return new ObjectiveDetails[0];
        return questData.stages[currentStageIndex].objectivesToComplete;
    }

    private void GetObjectiveDestination(string objectiveID)
    {
        var objective = objectiveProgress[objectiveID];
        if (objective.Equals(currentStageIndex))
        {
            foreach (var mechanic in questMechanicsObjects)
            {
                var comp = mechanic.GetComponent<IQuestMechanic>();
                if (objectiveProgress.Keys.Contains<string>(comp.GetObjectiveID()))
                {
                    destination = mechanic;
                } 
            }
        }
    }

    //Takes ObjectiveID and amount and increments. Checks if stage is complete and advances if true.
    public void UpdateObjective(string objectiveID, int amount)
    {
        if (!objectiveProgress.ContainsKey(objectiveID)) { return; }
        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            if (objectiveProgress[objectiveID] + amount > obj.quantityToComplete) { return; }
        }
            GetObjectiveDestination(objectiveID); 


        arrowPointer.SetDestination(destination);

        objectiveProgress[objectiveID] += amount;
        questLog.OnObjectiveUpdated(this, objectiveID, objectiveProgress[objectiveID]);


        Debug.Log("Objective Increments?");
        if (CheckStageComplete())
            AdvanceStage();
        Debug.Log("Stage Completed");
    }

    //checks if stages are completed and completed quest if true
    public bool CheckStageComplete()
    {
        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            if (obj.isOptional) continue;
            if (!objectiveProgress.ContainsKey(obj.objectiveID)) return false;
            if (objectiveProgress[obj.objectiveID] < obj.quantityToComplete)
                return false;
            //not sure if this triggers properly
            cachedExp += obj.bonusEXP;
            
        }

        return true;
    }

    //Advances Stage index. If not complete, Start quest.
    public void AdvanceStage()
    {
        
        cachedExp += questData.stages[currentStageIndex].expReward;
        cachedTrinkets += questData.stages[currentStageIndex].trinketReward;
        itemRewards.AddRange(questData.itemRewards);
        currentStageIndex++;
        GetQuestObjects();
        if (!IsComplete)
        {
            StartQuest();
        }
        if (currentStageIndex >= questData.stages.Length)
        {
            CompleteQuest();
        }
    }
    //calls the quest log function to remove quest
    public void CompleteQuest()
    {
        arrowPointer.DestroyArrow();
        expComp.IncrementXP(cachedExp);
        invComp.AddTrinketToInv(cachedTrinkets, 0);
        GiveItemReward();
        questLog.CheckForCompletedQuests();
    }

    //called if quest if failed and prompts player to cancel or retry
    public void QuestFailed()
    {
        questLog.OnQuestFailed(this);
        isQuestFailed = true;
        if (isRetrySelected)
        {
            objectiveProgress.First();
        }
        else if(isRetrySelected && isQuestFailed)
        {
            questLog.OnQuestFailed(this);
            
        }
   
        Debug.Log("Call Quest Failed");
    }

    public void GiveItemReward()
    {
        questLog.AddItemsToInventory(questData.itemRewards);
    }
}
