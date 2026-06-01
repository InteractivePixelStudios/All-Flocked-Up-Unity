using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class NPCBase: MonoBehaviour, I_NPCInterface
{

    public Transform targetLocation;
    [SerializeField] private NavMeshAgent navAgentComponent;
    public bool isMoving=false;
    [SerializeField] private DialogueBase dialogue;
    [SerializeField] private List<string> dialogueStartLineID = new();
    [SerializeField] private string retriggerDialogueLineID;
    [SerializeField]int index;
    [SerializeField] private GameObject homeLocation;
    [SerializeField] private GameObject warpLocation;
    bool atWarpLoc = false;
    [SerializeField] private QuestGiver questGiverComp;
    public bool dialogueFirst;
    private IconToggle questIcon;
    private Transform npcTransform;
    [SerializeField] bool isWaiting = true;

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
        Debug.Log("Loading");
    }
    //on start
    public void Start()
    {
        navAgentComponent = GetComponent<NavMeshAgent>();
        dialogue = FindAnyObjectByType<DialogueBase>();
                        npcTransform = transform;
        Debug.Log("NPC LOADED");
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
            npcTransform = transform;
            MoveToLocation();
        }
        if (warpLocation != null && !atWarpLoc && questGiverComp.readyToWarp) 
        { 
            transform.position = warpLocation.transform.position; 
            transform.rotation = warpLocation.transform.rotation; 
            isMoving = false;
            atWarpLoc = true; 
        }
        if (dialogueFirst == false&& questGiverComp == null && !isWaiting)
        {
            npcTransform = transform;
            questIcon.enabled = false;
            targetLocation = homeLocation.transform;
            isMoving = true;
            return;
        }
        if (questGiverComp != null && !isWaiting)
        {
            if(questGiverComp.hasQuest == false)
            {
                npcTransform = transform;
                questIcon.enabled = false;
                targetLocation = homeLocation.transform;
                isMoving = true;
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
        npcTransform = npc.npcTransform;
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
        navAgentComponent.SetDestination(targetLocation.position);
        navAgentComponent.updateRotation = true;
        if(transform.position == targetLocation.position)
        {
            isMoving = false;
            return;
        }
    }

    public void HitReact()
    {

    }



}
