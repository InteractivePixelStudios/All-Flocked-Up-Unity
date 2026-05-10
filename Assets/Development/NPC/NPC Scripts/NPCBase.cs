using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class NPCBase: MonoBehaviour, I_NPCInterface
{

    public Transform targetLocation;
    [SerializeField] private NavMeshAgent navAgentComponent;
    public bool isMoving=false;
    private UI_CanvasController canvasController;
    [SerializeField] private DialogueBase dialogue;
    [SerializeField] private List<string> dialogueStartLineID = new();
    [SerializeField] private string retriggerDialogueLineID;
    int index;
    [SerializeField] private GameObject homeLocation;
    [SerializeField] private QuestGiver questGiverComp;
    public bool dialogueFirst;
    private IconToggle questIcon;
    private Transform npcTransform;
    //on load
    public void Awake()
    {
        Debug.Log("Loading");
    }
    //on start
    public void Start()
    {
        questGiverComp = GetComponent<QuestGiver>();
        navAgentComponent = GetComponent<NavMeshAgent>();
        dialogue = FindAnyObjectByType<DialogueBase>();
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        Debug.Log("NPC LOADED");
        homeLocation = FindAnyObjectByType<LargeNest>().gameObject;
        questIcon = GetComponent<IconToggle>();
    }

    public void Update()
    {
        if (targetLocation!=null && isMoving)
        {
            npcTransform = transform;
            MoveToLocation();
        }
        if(questGiverComp.hasQuest == false || questGiverComp == null)
        {
            npcTransform = transform;
            questIcon.enabled = false;
            targetLocation = homeLocation.transform;
            //isMoving = true;
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
        dialogue.SetNPCRef(this);
        if (dialogue.isRetrigger)
        {
            dialogue.PrintDialogue(retriggerDialogueLineID);
        }
        else
        {
            dialogue.PrintDialogue(dialogueStartLineID[index]);
            index++;
            if (index <= dialogueStartLineID.Count - 1)
            {
                dialogue.isRetrigger = true;
                index = 0;
            }
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
    }

    public void HitReact()
    {

    }

    public void ContinueDialogue()
    {
        index++;
        dialogue.PrintDialogue(dialogueStartLineID[index]);
    }

 


}
