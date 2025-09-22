using UnityEngine;

public class Q_InteractComponent : MonoBehaviour, IQuestMechanic
{
    public string objectiveID;
    public QuestLog questLog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetQuestLog();
    }



    public void GetQuestLog()
    {
        questLog = FindFirstObjectByType<QuestLog>();
    }

    public void InteractWithObjective()
    {
        questLog.UpdateQuestObjective(objectiveID, 1);
    }

    public string GetObjectiveID() => objectiveID;

    //Link to PlayerInteraction script.
}
