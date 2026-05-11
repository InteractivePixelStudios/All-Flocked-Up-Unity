
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private NPCBase dialogueComp;
    UI_CanvasController canvasController;
    public float currentTime;

    public bool isQuestFailed = false;
    public bool isRetrySelected = false;
    private bool isPausedForDialogue;
    private bool dialogueComplete;

    public List<GameObject> questMechanicsObjects = new List<GameObject>();
    [SerializeField] private PlayerNavArrow arrowPointer;
    public GameObject destination;
    private int cachedExp;
    private int cachedTrinkets;
    private List<string> itemRewards = new();

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

    public List<string> GetItemRewards()
    {
        return itemRewards;
    }

    //Gets objectives and for each sets an objectiveID
    public async void StartQuest()
    {
        questLog = GameObject.FindAnyObjectByType<QuestLog>();
        player = questLog.gameObject;
        arrowPointer = player.GetComponent<PlayerNavArrow>();
        expComp = player.GetComponent<EXPSystem>();
        invComp = player.GetComponent<PlayerWingventory>();
        canvasController = GameObject.FindAnyObjectByType<UI_CanvasController>();

        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            objectiveProgress[obj.objectiveID] = 0;
        }

        // finds quest mechanics
        GetQuestObjects();
        SetQuestID(questData.questID);
        destination = GetObjectiveDestination(objectives[0].objectiveID);
         await Task.Delay(1000);
        dialogueComp = questLog.currentQuestGiver.GetComponent<NPCBase>();
        arrowPointer.EnablePointerArrow(destination);
        SetupStage();


    }

    //gets and sets QuestID to variable
    public void SetQuestID(string id)
    {
        questID = id;
    }

    //gets the quest mechanic gameobjects associated to that questID
    public void GetQuestObjects()
    {

        questMechanicsObjects.Clear();
        IQuestMechanic[] mechanics = Object
            .FindObjectsByType<MonoBehaviour>()
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

    private GameObject GetObjectiveDestination(string objectiveID)
    {
        foreach (var mechanic in questMechanicsObjects)
        {
            var comp = mechanic.GetComponent<IQuestMechanic>();

            if (comp != null && comp.GetObjectiveID() == objectiveID)
            {
                destination = mechanic;
                Debug.Log($"Destination set: {mechanic.name}");
                return destination;
            }
        }

        return null;
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
        objectiveProgress[objectiveID] += amount;
        questLog.OnObjectiveUpdated(this, objectiveID, objectiveProgress[objectiveID]);
        arrowPointer.SetDestination(destination);
        if (CheckStageComplete()) AdvanceStage();
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
        if (questData.stages[currentStageIndex].hasDialogueAfter && currentStageIndex < questData.stages.Length)
        {
            CallDialogue();
            return false;
        }
        else
        {
            return true;
        }

    }

    void SetupStage()
    {
        objectiveProgress.Clear();
        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            objectiveProgress[obj.objectiveID] = 0;
        }

        GetQuestObjects();

        destination = GetObjectiveDestination(objectives[0].objectiveID);
        arrowPointer.SetDestination(destination);
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
             SetupStage();
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

    private void CallDialogue()
    {
        canvasController.OpenDialogue();
        dialogueComp.InteractWithNPCDialogue();

    }
    public void GiveItemReward()
    {
        questLog.AddItemsToInventory(questData.itemRewards);
    }
}
