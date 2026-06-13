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
        questLog = FindAnyObjectByType<QuestLog>();
    }

    void ItemAtLocation()
    {
        questLog.UpdateQuestObjective(objectiveID, 1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("QuestItemToLocation"))
        {
            if(other.GetComponent<Q_Item>().objectiveID == objectiveID)
            {
                ItemAtLocation();
                Destroy(other.gameObject);
            }
        }
    }

    public string GetObjectiveID() => objectiveID;
}
