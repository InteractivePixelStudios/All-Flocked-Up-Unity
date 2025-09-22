using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask npcLayer;
    public LayerMask questLayer;
    public LayerMask dialogueLayer;
    public LayerMask trashLayer;
    public LayerMask raceLayer;
    public QuestLog questLog; // assign in Inspector
    public UI_CanvasController canvasController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // test input. Change later
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * interactionRange, Color.red);
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, npcLayer))
            {
                var questNPC = hit.collider.GetComponentInParent<IQuestInteraction>();
                if (questNPC != null)
                {
                    canvasController.ShowQuestGiver(hit.collider.GetComponentInParent<QuestGiver>());
                    Debug.Log(hit.ToString() + "QuestGiver");
                }
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, questLayer))
            {
                var questInteractable = hit.collider.GetComponentInParent<Q_InteractComponent>();
                if (questInteractable != null)
                {
                    questInteractable.InteractWithObjective();
                    Debug.Log(hit.ToString() + "InteractionObject");
                }
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, dialogueLayer))
            {
                var dialogueInteractable = hit.collider.GetComponentInParent<NPCBase>();
                if (dialogueInteractable != null)
                {
                    canvasController.OpenDialogue();
                    dialogueInteractable.InteractWithNPCDialogue();
                    Debug.Log(hit.ToString() + "Dialogue");
                }
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, trashLayer))
            {
                var trashInteractable = hit.collider.GetComponentInParent<TrashCanInteraction>();
                if (trashInteractable != null)
                {
                    canvasController.OpenDialogue();
                    trashInteractable.InteractWithTrashCan();
                    Debug.Log(hit.ToString() + "TrashCan");
                }
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, raceLayer))
            {
                var raceGiver = hit.collider.GetComponentInParent<RaceGiver>();
                if (raceGiver != null)
                {
                    raceGiver.InteractWithRaceGiver();
                    Debug.Log(hit.ToString() + "RaceGiver");
                }
            }
        }
        //TAB for quest log...will change this later to new input system
       else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (canvasController.activeLogInstance == null)
            {
                Debug.Log("QuestLog Opened?");
                canvasController.ShowQuestLog();
            }
            else canvasController.DestroyQuestLog();
        }

        
        RaycastHit lookHit;
        if (Physics.Raycast(transform.position, transform.forward, out lookHit, 6f, npcLayer))
        {
            var questNPC = lookHit.collider.GetComponentInParent<IQuestInteraction>();
            questNPC?.LookAtNPC();
        }
    }
}
