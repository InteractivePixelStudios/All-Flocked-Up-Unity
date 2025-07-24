using System.Collections.Generic;

[System.Serializable]
public class QuestRuntimeInstance
{
    public QuestDetails questData;
    public int currentStageIndex = 0;
    public Dictionary<string, int> objectiveProgress = new();

    public bool IsComplete => currentStageIndex >= questData.stages.Length;

    public void StartQuest()
    {
        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            objectiveProgress[obj.objectiveID] = 0;
        }
    }

    public ObjectiveDetails[] GetCurrentObjectives()
    {
        if (IsComplete) return new ObjectiveDetails[0];
        return questData.stages[currentStageIndex].objectivesToComplete;
    }

    public void UpdateObjective(string objectiveID, int amount)
    {
        if (!objectiveProgress.ContainsKey(objectiveID)) return;

        objectiveProgress[objectiveID] += amount;
        if (CheckStageComplete())
            AdvanceStage();
    }

    public bool CheckStageComplete()
    {
        var objectives = GetCurrentObjectives();
        foreach (var obj in objectives)
        {
            if (obj.isOptional) continue;
            if (!objectiveProgress.ContainsKey(obj.objectiveID)) return false;
            if (objectiveProgress[obj.objectiveID] < obj.quantityToComplete)
                return false;
        }
        return true;
    }

    public void AdvanceStage()
    {
        currentStageIndex++;
        if (!IsComplete)
        {
            StartQuest();
        }
    }
}
