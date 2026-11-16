using UnityEngine;

public class Q_IconRemover : MonoBehaviour, IQuestMechanic
{
    public string objectiveID;
    public QuestLog questLog;
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
