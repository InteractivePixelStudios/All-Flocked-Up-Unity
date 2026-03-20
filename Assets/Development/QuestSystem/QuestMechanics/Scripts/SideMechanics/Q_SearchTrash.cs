using UnityEngine;
public class Q_SearchTrash : MonoBehaviour, IQuestMechanic
{

    public string objectiveID;
    public UI_CanvasController canvasController;
    public QuestLog questLog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        questLog = FindAnyObjectByType<QuestLog>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SearchTrash()
    {
        questLog.UpdateQuestObjective(objectiveID, 1);
    }


    public void GetQuestLog()
    {
        questLog = FindAnyObjectByType<QuestLog>();
    }

    public string GetObjectiveID() => objectiveID;


}



