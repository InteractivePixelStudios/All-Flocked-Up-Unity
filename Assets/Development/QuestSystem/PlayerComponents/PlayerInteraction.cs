using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask npcLayer;
    public LayerMask questLayer;
    public LayerMask dialogueLayer;
    public LayerMask trashLayer;
    public LayerMask raceLayer;
    public LayerMask nestLayer;
    public LayerMask shopLayer;
    public LayerMask wearableLayer;
    public QuestLog questLog; // assign in Inspector
    public UI_CanvasController canvasController;
    public bool gamePaused;
    [SerializeField] private GameObject attachPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // test input. Change later
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position + transform.up, transform.forward * interactionRange, Color.red);
            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, npcLayer))
            {
                var questNPC = hit.collider.GetComponentInParent<IQuestInteraction>();
                if (questNPC != null)
                {
                    canvasController.ShowQuestGiver(hit.collider.GetComponentInParent<QuestGiver>());
                }
            }

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, questLayer))
            {
                var questInteractable = hit.collider.GetComponentInParent<Q_InteractComponent>();
                if (questInteractable != null)
                {
                    questInteractable.InteractWithObjective();
                }
            }

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, dialogueLayer))
            {
                var dialogueInteractable = hit.collider.GetComponentInParent<NPCBase>();
                if (dialogueInteractable != null)
                {
                    canvasController.OpenDialogue();
                    dialogueInteractable.InteractWithNPCDialogue();
                }
            }

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, trashLayer))
            {
                var trashInteractable = hit.collider.GetComponentInParent<TrashCanInteraction>();
                if (trashInteractable != null)
                {
                    trashInteractable.InteractWithTrashCan();
                }
            }

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, raceLayer))
            {
                var raceGiver = hit.collider.GetComponentInParent<RaceGiver>();
                if (raceGiver != null)
                {
                    raceGiver.InteractWithRaceGiver();
                }
            }


            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, nestLayer))
            {
                var nestObj = hit.collider.GetComponentInParent<NestBase>();
                nestObj?.InteractWithNest();
            }

            if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, interactionRange, shopLayer))
            {
                var shopObj = hit.collider.GetComponentInParent<ShopLocation>();
                var box = hit.collider as BoxCollider ?? hit.collider.GetComponentInParent<BoxCollider>();
                shopObj?.InteractWithShop(box);
            }

            if(Physics.Raycast(transform.position, transform.forward,out hit,interactionRange, wearableLayer))
            {
                var wearableObj = hit.collider.gameObject;
                var comp = wearableObj.GetComponent<Wearable_Base>();
                if (!comp.isGrabbed)
                {
                    comp.LookForObject();
                    comp.attachPoint = attachPoint;
                }
                else if (comp.isGrabbed) { comp.RemoveObject(); }
                else return;
            }
        }
        //TAB for quest log...will change this later to new input system
       else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (canvasController.activeLogInstance == null)
            {
                canvasController.ShowQuestLog();
            }
            else canvasController.DestroyQuestLog();
        }

        
        RaycastHit lookHit;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out lookHit, interactionRange, npcLayer))
        {
            var questNPC = lookHit.collider.GetComponentInParent<IQuestInteraction>();
            questNPC?.LookAtNPC();
        }

        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (canvasController.activeWingventory== null)
            {
                canvasController.OpenWingventory();
            }
            else canvasController.CloseWingventory();
        }

        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (!gamePaused && canvasController.activePauseMenu == null)
            {
                canvasController.PauseGame();
            }
            else canvasController.ResumeGame();
        }

        else if (Input.GetKeyDown(KeyCode.F10))
        {
            if(canvasController.activeBugReporter == null)
            {
                canvasController.OpenBugReporter();
            }
            else
            {
                canvasController.CloseBugReporter();
            }

        }

        else if (Input.GetKeyDown(KeyCode.F9))
        {
            if(canvasController.activeBugReporter == null)
            {
                canvasController.OpenDebugMenu();
            }
            else
            {
                canvasController.CloseDebugMenu();
            }
        }

        else if (Input.GetKeyDown(KeyCode.M))
        {
            if(canvasController.activeMapCanvas == null)
            {
                canvasController.OpenMainMap();
            }
            else
            {
                canvasController.CloseMainMap();
            }
        }


    }
}
