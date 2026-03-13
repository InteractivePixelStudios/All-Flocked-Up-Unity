using UnityEngine;

public class Q_Garbage : MonoBehaviour, IQuestMechanic
{

    public string objectiveID;
    public UI_CanvasController canvasController;
    public QuestLog questLog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        questLog = FindFirstObjectByType<QuestLog>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GarbageInTrash()
    {
        questLog.UpdateQuestObjective(objectiveID, 1);
    }


    public void GetQuestLog()
    {
        questLog = FindFirstObjectByType<QuestLog>();
    }

    public string GetObjectiveID() => objectiveID;


}


