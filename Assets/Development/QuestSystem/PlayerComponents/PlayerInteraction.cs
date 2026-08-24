using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 1.5f;
    public LayerMask npcLayer;
    public LayerMask questLayer;
    public LayerMask dialogueLayer;
    public LayerMask trashLayer;
    public LayerMask raceLayer;
    public LayerMask nestLayer;
    public LayerMask shopLayer;
    public LayerMask wearableLayer;
    public LayerMask perchLayer;
    public LayerMask rideLayer;
    public LayerMask hideSeekLayer;
    public QuestLog questLog; // assign in Inspector
    
    //lazy init pattern here, helps fix some issues
    private UI_CanvasController _canvasController;
    public UI_CanvasController canvasController
    {
        get
        {
            if (!_canvasController) 
            _canvasController = FindAnyObjectByType<UI_CanvasController>();
            return _canvasController;
        }
    
    }
    public bool gamePaused;
    [SerializeField] private GameObject attachPoint;
    private bool isWingventoryOpen;
    public PlayerPerchSystem perchComp;
    public I_Perchable currentPerchPoint;

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
      //  if (playerInput.currentActionMap == playerInput.actions.FindActionMap("UI") &&!uiOn) { uiOn = true; InitInputs(); Debug.Log("REINIT"); }
       // else return;
    }

    public bool ReturnInteractPerformed()
    {
        return interactAction.inProgress;
    }
    void InitInputs()
    {
        interactAction = InputSystem.actions.FindAction("Player/Interact");
        questLogAction = InputSystem.actions.FindAction("Player/QuestLog");
        mapAction = InputSystem.actions.FindAction("Player/Map");
        inventoryAction = InputSystem.actions.FindAction("Player/Inventory");
        pauseAction = InputSystem.actions.FindAction("Player/Pause");
        debugAction = InputSystem.actions.FindAction("Player/Debug");
        reportAction = InputSystem.actions.FindAction("Player/Report");

        if (interactAction != null && questLogAction != null && mapAction != null && inventoryAction != null && pauseAction != null)
        {
            //use started / cancelled for grab/hold
            interactAction.started+= Interact;
            questLogAction.performed += OpenQuestLog;
            mapAction.performed += OpenMap;
            inventoryAction.performed += OpenInventory;
            pauseAction.performed += OpenPause;
           // debugAction.performed += OpenDebug;
           // reportAction.performed += OpenReport;
        }
        
    }
    public bool GetIsWingventoryOpen()
    {
        return isWingventoryOpen;
    }
    
    public void SetIsWingventoryOpen(bool value) => isWingventoryOpen = value;


    //public void OpenReport(InputAction.CallbackContext ctx)
    //{
    //    if (canvasController.activeBugReporter == null)
    //    {
    //        canvasController.OpenBugReporter();
    //    }
    //    else
    //    {
    //        canvasController.CloseBugReporter();
    //        uiOn = false;
    //    }
    //}

    //public void OpenDebug(InputAction.CallbackContext ctx)
    //{
    //    if (canvasController.activeBugReporter == null)
    //    {
    //        canvasController.OpenDebugMenu();
    //    }
    //    else
    //    {
    //        canvasController.CloseDebugMenu();
    //        uiOn = false;
    //    }
    //}



    public void Interact(InputAction.CallbackContext ctx)
    {

            RaycastHit hit;
            Debug.DrawRay(transform.position + (transform.up / 4), transform.forward * interactionRange, Color.red);
            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, npcLayer))
            {
            var questNPC = hit.collider.GetComponentInParent<IQuestInteraction>();
            var questGiver =  hit.collider.GetComponentInParent<QuestGiver>();
                if (questNPC != null)
                {
                var NPC = hit.collider.gameObject.GetComponent<NPCBase>();
                if (NPC.dialogueFirst == true)
                {
                    canvasController.OpenDialogue();
                    NPC.InteractWithNPCDialogue();
                    NPC.dialogueFirst = false;
                }else if(NPC.dialogueFirst == false && questGiver.quests.Count>0 && !questLog.HasQuest(questGiver.quests[0]))
                {
                    //questGiver.hasQuest = true;
                    NPC.dialogueFirst = true;
                    canvasController.ShowQuestGiver(questGiver);
                }
                  
                }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, questLayer))
            {
                var questInteractable = hit.collider.GetComponentInParent<Q_InteractComponent>();
                if (questInteractable != null)
                {
                    questInteractable.InteractWithObjective();
                }
            }

            //if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, dialogueLayer))
            //{
            //    var dialogueInteractable = hit.collider.GetComponentInParent<NPCBase>();
            //    if (dialogueInteractable != null)
            //    {
            //        canvasController.OpenDialogue();
            //        dialogueInteractable.InteractWithNPCDialogue();
            //    }
            //}

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

                var nestComp = nestObj.GetComponent<Q_InteractComponent>();
            if(nestComp != null)
            {
                nestComp.InteractWithObjective();

            }
            }

            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, shopLayer))
            {
                var shopObj = hit.collider.GetComponentInParent<ShopLocation>();
                var box = hit.collider as BoxCollider ?? hit.collider.GetComponentInParent<BoxCollider>();
                shopObj?.InteractWithShop(box);
            }

        if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward , out hit, interactionRange, rideLayer))
        {
            var rideObj = hit.collider.GetComponent<Rider_Base>();
            Debug.Log(rideObj);
            rideObj?.StartRiding();
        }


        if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, hideSeekLayer))
        {
            var hideSeekCon = hit.collider.GetComponent<HAS_Giver>();
            if (hideSeekCon != null) { hideSeekCon?.GiveInfo(); }
            else
            {
                var hideSeekObj = hit.collider.GetComponent<HAS_NPC>();
                if (hideSeekObj != null)
                {
                    Debug.Log(hideSeekObj);
                    hideSeekObj?.CallFound();
                }
            }

        }

        if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, wearableLayer))
            {
                var wearableObj = hit.collider.gameObject;
                var comp = wearableObj.GetComponent<Wearable_Base>();
                if (!comp.isGrabbed)
                {
                    comp.attachPoint = attachPoint;
                    comp.LookForObject(hit);

                }
                else Debug.Log("skipped"); return;
            }

            RaycastHit lookHit;
            if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out  lookHit, interactionRange, npcLayer))
            {
                var questNPC = lookHit.collider.GetComponentInParent<IQuestInteraction>();
                questNPC?.LookAtNPC();
            }


        if (Physics.Raycast(transform.position + (transform.up / 4), transform.forward, out hit, interactionRange, perchLayer))
        {

            currentPerchPoint = hit.collider.GetComponentInParent<I_Perchable>();
            currentPerchPoint.SetPlayerRef(this.gameObject);
            Debug.Log(currentPerchPoint);
            perchComp.isReady = true;
            switch (currentPerchPoint)
            {
                case PerchableObject_Tree:

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
        else { perchComp.isReady = false; }
    }

        void OpenQuestLog(InputAction.CallbackContext ctx)
        {
        if (UI_HudController.Instance!= null)
        {
            if (UI_HudController.Instance.GetIsTDOpen() == false)
            {
                canvasController.ShowToDoPanel();
            }
            else { canvasController.HideToDoPanel(); uiOn = false; }
        }
        
    }

        void OpenMap(InputAction.CallbackContext ctx)
        {
            if (canvasController.activeMapCanvas == null && !canvasController.uiOpen)
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
            if (canvasController.activeWingventory == null && !canvasController.uiOpen)
            {
                canvasController.OpenWingventory();
                isWingventoryOpen = true;
                uiOn = true;
        }
            
            //this else never ever fires it's impossible with current action map setup
            //moving the below logic somewhere else to kill two birds with one stone -
            //having the bool flipped by all three inventory closing pathways: button, action and camera
           
            /*else
            {
                canvasController.CloseWingventory();
                isWingventoryOpen = false;  //MOVED TO WINGVENTORYCANVAS LINES 97-99
                uiOn = false;
            }*/
        }
        

        void OpenPause(InputAction.CallbackContext ctx)
        {
            if (!gamePaused && canvasController.activePauseMenu == null && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
            {
                canvasController.PauseGame();
            }
            else canvasController.ResumeGame(); uiOn = false;
        }

        //OnLevelWasLoaded refreshes scene-bound refs on scene change — deprecated but functional
        private void OnLevelWasLoaded(int level)
        {
         //   canvasController = FindAnyObjectByType<UI_CanvasController>();
            questLog = FindAnyObjectByType<QuestLog>();
            Debug.Log("OLWL fired, level=" + level);
        }
}

