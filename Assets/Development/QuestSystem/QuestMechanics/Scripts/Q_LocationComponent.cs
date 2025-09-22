using System.Threading.Tasks;
using UnityEngine;

public class Q_LocationComponent : MonoBehaviour, IQuestMechanic
{
    public string objectiveID;
    public UI_CanvasController canvasController;
    public QuestLog questLog;

    private bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetQuestLog();

        //change this later or add spawning to quests...this will only check against hasQuest bool...any quest will trigger
        //and you would be able to hit collider while wrong quest is active
        //if (!questLog.hasQuest)
        //{
        //    this.gameObject.SetActive(false);
        //}
        //else this.gameObject.SetActive(true);
        
        
    }

    // Update is called once per frame

    public void GetQuestLog()
    {
        questLog = FindFirstObjectByType<QuestLog>();
    }

    public string GetObjectiveID() => objectiveID;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) { return; }
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            //questRuntimeInstance.UpdateObjective(objectiveID, 1);
            questLog.UpdateQuestObjective(objectiveID, 1);
            canvasController.ShowQuestLocationNotif();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = false;
            Destroy(this);
        }
    }
}
