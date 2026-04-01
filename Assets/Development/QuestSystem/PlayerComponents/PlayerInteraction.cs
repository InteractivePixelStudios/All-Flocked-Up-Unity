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
    public LayerMask perchLayer;
    public QuestLog questLog; // assign in Inspector
    public UI_CanvasController canvasController;
    public bool gamePaused;
    [SerializeField] private GameObject attachPoint;
    private bool isWingventoryOpen;
    public PlayerPerchSystem perchComp;
    public I_Perchable currentPerchPoint;
    bool perchInteracted;

    private PlayerInput playerInput;
    private InputAction interactAction;
    private InputAction questLogAction;
    private InputAction mapAction;
    private InputAction inventoryAction;
    private InputAction pauseAction;
    private InputAction debugAction;
    private InputAction reportAction;
    bool uiOn;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        InitInputs();
    }
    private void Update()
    {
        if (playerInput.currentActionMap == playerInput.actions.FindActionMap("UI") &&!uiOn) { uiOn = true; InitInputs(); Debug.Log("REINIT"); }
        else return;
    }

    public bool ReturnInteractPerformed()
    {
        return interactAction.inProgress;
    }
    void InitInputs()
    {
        interactAction = playerInput.actions.FindAction("Interact");
        questLogAction = playerInput.actions.FindAction("QuestLog");
        mapAction = playerInput.actions.FindAction("Map");
        inventoryAction = playerInput.actions.FindAction("Inventory");
        pauseAction = playerInput.actions.FindAction("Pause");
        debugAction = playerInput.actions.FindAction("Debug");
        reportAction = playerInput.actions.FindAction("Report");

        if (interactAction != null && questLogAction != null && mapAction != null && inventoryAction != null && pauseAction != null)
        {
            //use started / cancelled for grab/hold
            interactAction.started+= Interact;
            questLogAction.performed += OpenQuestLog;
            mapAction.performed += OpenMap;
            inventoryAction.performed += OpenInventory;
            pauseAction.performed += OpenPause;
           // debugAction.performed += OpenDebug;
            reportAction.performed += OpenReport;
        }
        
    }
    public bool GetIsWingventoryOpen()
    {
        return isWingventoryOpen;
    }

    public void OpenReport(InputAction.CallbackContext ctx)
    {
        if (canvasController.activeBugReporter == null)
        {
            canvasController.OpenBugReporter();
        }
        else
        {
            canvasController.CloseBugReporter();
            uiOn = false;
        }
    }

    public void OpenDebug(InputAction.CallbackContext ctx)
    {
        if (canvasController.activeBugReporter == null)
        {
            canvasController.OpenDebugMenu();
        }
        else
        {
            canvasController.CloseDebugMenu();
            uiOn = false;
        }
    }



    public void Interact(InputAction.CallbackContext ctx)
    {

            RaycastHit hit;
            Debug.DrawRay(transform.position + (transform.up / 4), transform.forward * interactionRange, Color.red);
            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, npcLayer))
            {
            Debug.Log("Pressed");
            var questNPC = hit.collider.GetComponentInParent<IQuestInteraction>();
                if (questNPC != null)
                {
                Debug.Log("FoundNPC");
                var NPC = hit.collider.gameObject.GetComponent<NPCBase>();
                if (NPC.dialogueFirst == true)
                {
                    canvasController.OpenDialogue();
                    NPC.InteractWithNPCDialogue();
                    Debug.Log("DialogueFirst");
                    NPC.dialogueFirst = false;
                }else if(NPC.dialogueFirst == false)
                {
                    canvasController.ShowQuestGiver(hit.collider.GetComponentInParent<QuestGiver>());
                }
                  
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, questLayer))
            {
                var questInteractable = hit.collider.GetComponentInParent<Q_InteractComponent>();
                if (questInteractable != null)
                {
                    questInteractable.InteractWithObjective();
                Debug.Log("InteractWithQuest");
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, dialogueLayer))
            {
                var dialogueInteractable = hit.collider.GetComponentInParent<NPCBase>();
                if (dialogueInteractable != null)
                {
                    canvasController.OpenDialogue();
                    dialogueInteractable.InteractWithNPCDialogue();
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, trashLayer))
            {
                var trashInteractable = hit.collider.GetComponentInParent<TrashCanInteraction>();
                if (trashInteractable != null)
                {
                    trashInteractable.InteractWithTrashCan();
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, raceLayer))
            {
                var raceGiver = hit.collider.GetComponent<RaceGiver>();
                if (raceGiver != null)
                {
                    raceGiver.InteractWithRaceGiver();
                }
            }


            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, nestLayer))
            {
                var nestObj = hit.collider.GetComponentInParent<NestBase>();
                nestObj?.InteractWithNest();
            Debug.Log("InteractWithNest");
                var nestComp = nestObj.GetComponent<Q_InteractComponent>();
            if(nestComp != null)
            {
                nestComp.InteractWithObjective();
                Debug.Log("InteractWithQuest");
            }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, shopLayer))
            {
                var shopObj = hit.collider.GetComponentInParent<ShopLocation>();
                var box = hit.collider as BoxCollider ?? hit.collider.GetComponentInParent<BoxCollider>();
                shopObj?.InteractWithShop(box);
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, wearableLayer))
            {
                var wearableObj = hit.collider.gameObject;
                var comp = wearableObj.GetComponent<Wearable_Base>();
                if (!comp.isGrabbed)
                {
                    comp.attachPoint = attachPoint;
                    comp.LookForObject(hit);
                    Debug.Log("Attached");
                }
                else Debug.Log("skipped"); return;
            }

            RaycastHit lookHit;
            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out lookHit, interactionRange, npcLayer))
            {
                var questNPC = lookHit.collider.GetComponentInParent<IQuestInteraction>();
                questNPC?.LookAtNPC();
            }


        if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, perchLayer))
        {
            Debug.Log("PerchSeen");
            currentPerchPoint = hit.collider.GetComponentInParent<I_Perchable>();
            Debug.Log(currentPerchPoint);
            perchComp.isReady = true;
            perchInteracted = true;
            switch (currentPerchPoint)
            {
                case PerchableObject_Tree:
                    Debug.Log("Ima Tree");
                    var tree = currentPerchPoint as PerchableObject_Tree;
                    tree.isPerching = true;
                    currentPerchPoint.StartPerch();
                    var check = hit.collider.CompareTag("HideSpot");
                    if (check)
                    {
                        tree.isHiding = true;
                    }
                    break;
                case PerchableObject_Bush:
                    perchComp.Perch(currentPerchPoint);
                    break;
                case PerchableObject_General:
                    perchComp.Perch(currentPerchPoint);
                    break;
            }

        }
        else { perchComp.isReady = false; perchInteracted = false; }
    }

        void OpenQuestLog(InputAction.CallbackContext ctx)
        {
            if (canvasController.activeLogInstance == null)
            {
                canvasController.ShowQuestLog();
            }
            else canvasController.DestroyQuestLog(); uiOn = false;
    }

        void OpenMap(InputAction.CallbackContext ctx)
        {
            if (canvasController.activeMapCanvas == null)
            {
                canvasController.OpenMainMap();
            }
            else
            {
                canvasController.CloseMainMap(); uiOn = false;
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
                uiOn = false;
            }
    }

        void OpenPause(InputAction.CallbackContext ctx)
        {
            if (!gamePaused && canvasController.activePauseMenu == null)
            {
                canvasController.PauseGame();
            }
            else canvasController.ResumeGame(); uiOn = false;
    }

    private void OnLevelWasLoaded(int level)
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
    }
}

