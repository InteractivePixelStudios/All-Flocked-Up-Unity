using UnityEngine;

public class Q_ItemToLocation : MonoBehaviour, IQuestMechanic
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

    void ItemAtLocation()
    {
        questLog.UpdateQuestObjective(objectiveID, 1);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("QuestItemToLocation"))
        {
            ItemAtLocation();
            Destroy(collision.gameObject);
        }
    }

    public string GetObjectiveID() => objectiveID;
}
