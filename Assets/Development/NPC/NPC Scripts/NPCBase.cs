using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class NPCBase: MonoBehaviour, I_NPCInterface
{

    public Transform targetLocation;
    [SerializeField] private NavMeshAgent navAgentComponent;
    public bool isMoving=false;
    bool destinationSet;
    [SerializeField] private DialogueBase dialogue;
    [SerializeField] private List<string> dialogueStartLineID = new();
    [SerializeField] private string retriggerDialogueLineID;
    [SerializeField]int index;
    [SerializeField] private GameObject homeLocation;
    [SerializeField] private GameObject warpLocation;
    [SerializeField]bool readyToWarp;
   [SerializeField] bool atWarpLoc = false;
    [SerializeField] private QuestGiver questGiverComp;
    public bool dialogueFirst;
    private IconToggle questIcon;
    [SerializeField] bool isWaiting = true;

    public void SetReadyToWarp(bool value)
    {
        Debug.Log("readytowarp Set");
        readyToWarp = value;
    }

    public void SetIsWaiting(bool value)
    {
        isWaiting = value;
    }

    public int GetDialogueLineCount()
    {
        return dialogueStartLineID.Count;
    }
    //on load
    public void Awake()
    {

    }
    //on start
    public void Start()
    {
        navAgentComponent = GetComponent<NavMeshAgent>();
        dialogue = FindAnyObjectByType<DialogueBase>();
        if(homeLocation == null)
        {
            homeLocation = FindAnyObjectByType<LargeNest>().gameObject;
        }
        questIcon = GetComponent<IconToggle>();
        TryGetComponent<QuestGiver>(out questGiverComp);
    }

    public void Update()
    {
        if (targetLocation!=null && isMoving)
        {
            MoveToLocation();
        }
        if (warpLocation != null && !atWarpLoc && readyToWarp) 
        {
            isMoving = false;
            navAgentComponent.Warp(warpLocation.transform.position);
            atWarpLoc = true; 
        }
        if (dialogueFirst == false&& questGiverComp == null && !isWaiting &&!isMoving) //no quest giver... no dialogue...not waiting...for Racegiver
        {
            questIcon.enabled = false;
            targetLocation = homeLocation.transform;
            isMoving = true;
            return;
        }
        if (questGiverComp != null && !isWaiting &&!isMoving)//  questgiver... not waiting ....for questGiver
        {
            if(!questGiverComp.hasQuest)
            {
                questIcon.enabled = false;
                targetLocation = homeLocation.transform;
                isMoving = true;
                return;
            }

        }

    }  
    
    public void LoadData(NPCBase npc)
    {
        dialogueStartLineID = npc.dialogueStartLineID;
        dialogueFirst = npc.dialogueFirst;
        retriggerDialogueLineID = npc.retriggerDialogueLineID;
        index = npc.index;
        isMoving = npc.isMoving;
        transform.position = npc.transform.position;
        transform.rotation = npc.transform.rotation;
    }

    //use this to add "Look at" effects like a prompt or something
    public void LookAtNPC()
    {

    }

    //called from PlayerInteraction... opens and prints dialogue
    public void InteractWithNPCDialogue()
    {
        if (dialogueStartLineID.Count == 0)
            return;
        dialogue.SetNPCRef(this);
        if (dialogue.isRetrigger)
        {
            dialogue.PrintDialogue(retriggerDialogueLineID);
        }
        else
        {
            Debug.Log(index);
            dialogue.isRetrigger = false;
            dialogue.PrintDialogue(dialogueStartLineID[index]);
            index++;
            Debug.Log(index);
            //if (index >= dialogueStartLineID.Count)
            //{
            //    dialogue.isRetrigger = true;
            //   index = 0;
            //}
        }


    }

    //sets the NPC move-to location
    public void SetMoveToLocation(Transform location)
    {
        targetLocation = location;
    }

    //call this to run like wind
    public void MoveToLocation()
    {
        if (!destinationSet)
        {
            navAgentComponent.SetDestination(targetLocation.position);
            navAgentComponent.updateRotation = true;
            destinationSet = true;
            return;
        }
        if(!navAgentComponent.pathPending && navAgentComponent.remainingDistance <= navAgentComponent.stoppingDistance)
        {
            navAgentComponent.isStopped = true;
            destinationSet = false;
            isMoving = false;
            return;
        }
    }

    public void HitReact()
    {

    }



}
