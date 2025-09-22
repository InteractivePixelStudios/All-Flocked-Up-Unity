using UnityEngine;
using System.Threading.Tasks;

public class Q_KillComponent : MonoBehaviour, IQuestMechanic
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

    public void KillComplete()
    {
        questLog.UpdateQuestObjective(objectiveID, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            KillComplete();
            Debug.Log("collision test");
        }
    }

    public string GetObjectiveID() => objectiveID;
}
