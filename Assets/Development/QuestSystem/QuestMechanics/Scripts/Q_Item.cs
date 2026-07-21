using UnityEngine;

public class Q_Item : MonoBehaviour, IQuestMechanic
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
    public string GetObjectiveID() => objectiveID;
}
