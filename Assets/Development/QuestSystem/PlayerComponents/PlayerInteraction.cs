using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


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
    private bool isWingventoryOpen;

    private PlayerInput playerInput;
    private InputAction interactAction;
    private InputAction questLogAction;
    private InputAction mapAction;
    private InputAction inventoryAction;
    private InputAction pauseAction;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        questLogAction = playerInput.actions.FindAction("QuestLog");
        mapAction = playerInput.actions.FindAction("Map");
        inventoryAction = playerInput.actions.FindAction("Inventory");
        pauseAction = playerInput.actions.FindAction("Pause");

        if (interactAction != null && questLogAction != null && mapAction != null && inventoryAction != null && pauseAction != null)
        {
            //use started / cancelled for grab/hold
            interactAction.performed += Interact;
            questLogAction.performed += OpenQuestLog;
            mapAction.performed += OpenMap;
            inventoryAction.performed += OpenInventory;
            pauseAction.performed += OpenPause;
        }
    }
    public bool GetIsWingventoryOpen()
    {
        return isWingventoryOpen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (canvasController.activeBugReporter == null)
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
            if (canvasController.activeBugReporter == null)
            {
                canvasController.OpenDebugMenu();
            }
            else
            {
                canvasController.CloseDebugMenu();
            }
        }

    }

    public void Interact(InputAction.CallbackContext ctx)
    {

            RaycastHit hit;
            Debug.DrawRay(transform.position + (transform.up / 2), transform.forward * interactionRange, Color.red);
            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, npcLayer))
            {
                var questNPC = hit.collider.GetComponentInParent<IQuestInteraction>();
                if (questNPC != null)
                {
                    canvasController.ShowQuestGiver(hit.collider.GetComponentInParent<QuestGiver>());
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, questLayer))
            {
                var questInteractable = hit.collider.GetComponentInParent<Q_InteractComponent>();
                if (questInteractable != null)
                {
                    questInteractable.InteractWithObjective();
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, dialogueLayer))
            {
                var dialogueInteractable = hit.collider.GetComponentInParent<NPCBase>();
                if (dialogueInteractable != null)
                {
                    canvasController.OpenDialogue();
                    dialogueInteractable.InteractWithNPCDialogue();
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, trashLayer))
            {
                var trashInteractable = hit.collider.GetComponentInParent<TrashCanInteraction>();
                if (trashInteractable != null)
                {
                    trashInteractable.InteractWithTrashCan();
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, raceLayer))
            {
                var raceGiver = hit.collider.GetComponent<RaceGiver>();
                if (raceGiver != null)
                {
                    raceGiver.InteractWithRaceGiver();
                }
            }


            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, nestLayer))
            {
                var nestObj = hit.collider.GetComponentInParent<NestBase>();
                nestObj?.InteractWithNest();
            }

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, shopLayer))
            {
                var shopObj = hit.collider.GetComponentInParent<ShopLocation>();
                var box = hit.collider as BoxCollider ?? hit.collider.GetComponentInParent<BoxCollider>();
                shopObj?.InteractWithShop(box);
            }

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out hit, interactionRange, wearableLayer))
            {
                var wearableObj = hit.collider.gameObject;
                var comp = wearableObj.GetComponent<Wearable_Base>();
                if (!comp.isGrabbed)
                {
                    comp.LookForObject();
                    comp.attachPoint = attachPoint;
                    Debug.Log("Attached");
                }
                else if (comp.isGrabbed) { comp.RemoveObject(); Debug.Log("remove"); }
                else Debug.Log("skipped"); return;
            }

            RaycastHit lookHit;
            if (Physics.Raycast(transform.position + (transform.up / 2), transform.forward, out lookHit, interactionRange, npcLayer))
            {
                var questNPC = lookHit.collider.GetComponentInParent<IQuestInteraction>();
                questNPC?.LookAtNPC();
            }
        }

        void OpenQuestLog(InputAction.CallbackContext ctx)
        {
            if (canvasController.activeLogInstance == null)
            {
                canvasController.ShowQuestLog();
            }
            else canvasController.DestroyQuestLog();
        }

        void OpenMap(InputAction.CallbackContext ctx)
        {
            if (canvasController.activeMapCanvas == null)
            {
                canvasController.OpenMainMap();
            }
            else
            {
                canvasController.CloseMainMap();
            }
        }

        void OpenInventory(InputAction.CallbackContext ctx)
        {
            if (canvasController.activeWingventory == null)
            {
                canvasController.OpenWingventory();
                isWingventoryOpen = true;
            }
            else
            {
                canvasController.CloseWingventory();
                isWingventoryOpen = false;
            }
        }

        void OpenPause(InputAction.CallbackContext ctx)
        {
            if (!gamePaused && canvasController.activePauseMenu == null)
            {
                canvasController.PauseGame();
            }
            else canvasController.ResumeGame();
        }
    }

