using UnityEngine;

public class Q_Collect : MonoBehaviour,IQuestMechanic
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
        questLog = FindAnyObjectByType<QuestLog>();
    }

    public void OnDestroy()
    {
        if(questLog != null)
        {
            questLog.UpdateQuestObjective(objectiveID, 1);

        }


    }

    public string GetObjectiveID() => objectiveID;
}
