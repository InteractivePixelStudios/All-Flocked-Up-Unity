using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;
using Unity.Mathematics;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class DialogueBase : MonoBehaviour
{
    [SerializeField] private int currentDialogueIndex=0;
    [SerializeField] private int startDialogueIndex=0;
    [SerializeField] private string currentDialogueLineID;

    [SerializeField] private string currentDialogueName;
    [SerializeField] private string currentDialogueText;
    [SerializeField] private Image currentDialogueImage;
    [SerializeField] private string currentContinueStatus;
    public string currentBranchID;
    public string[] currentResponseOptions;
    public string responseReturnID;
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private bool typerComplete {  get; set; }

    [SerializeField] private string DIALOGUEFILENAME = "DialogueSpreadsheet.csv";
    [SerializeField]private List<DialogueLineData> dialogueList = new List<DialogueLineData>();
    public DialogueLineData currentDialogueLineData;


    [SerializeField]private string retriggerDialogueLineID;
    public bool isRetrigger;

   [SerializeField] private int currentTextSpeed;
    public int textSpeed=>currentTextSpeed=500;// this speed is in ms


    void Start()
    {
        LoadDialogueSheet();

    }
    //loads the CSV and adds each line as a string into importedLines, trims each line into lineData and sets the currentDialogueLine based on currentDialogueIndex
    public void LoadDialogueSheet()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "DialogueSpreadsheet.csv");
        if (!File.Exists(filePath)) { Debug.Log("FileNotFound"); return; }

        string[] importedLines = File.ReadAllLines(filePath);
        Debug.Log(importedLines.Length);

        for (int i = currentDialogueIndex; i < importedLines.Length; i++)
        {
            string line = importedLines[i].Trim();

            if (string.IsNullOrEmpty(line)) continue;

            string[] lineData = line.Split(',');

            DialogueLineData dialogueLine = new DialogueLineData
            {
                dialogueID = lineData[0],
                dialogueSpeaker = lineData[1],
                dialogueText = lineData[2],
                dialogueImage = lineData[3],
                dialogueContinue = lineData[4],
                nextID = lineData[5],
                resposeOptions = lineData[6].Split('|'),
                branchID = lineData[7]



            };
            dialogueList.Add(dialogueLine);
            currentDialogueLineData = dialogueLine;
            currentDialogueLineID = dialogueLine.dialogueID;
            currentContinueStatus = dialogueLine.dialogueContinue;
            currentDialogueName = dialogueLine.dialogueSpeaker;
            currentDialogueText = dialogueLine.dialogueText;
            retriggerDialogueLineID = dialogueLine.nextID;
            currentResponseOptions = dialogueLine.resposeOptions;
            currentBranchID = dialogueLine.branchID;

            
        }
        if (dialogueList.Count > 0)
        {
            currentDialogueLineData = dialogueList[0];
            currentDialogueLineID = currentDialogueLineData.dialogueID;
            currentDialogueName = currentDialogueLineData.dialogueSpeaker;
            currentDialogueText = currentDialogueLineData.dialogueText;
            currentContinueStatus=currentDialogueLineData.dialogueContinue;
            currentResponseOptions = currentDialogueLineData.resposeOptions;
            currentBranchID=currentDialogueLineData.branchID;

        }
        TypeText(textSpeed);
        SendResponseOptions();
    }
    //returns the desired dialogueLineData based on the given string ID
    public DialogueLineData GetDialogueLineByID(string id)
    {
        return dialogueList.Find(line => line.dialogueID == id);
    }
    //Sets the currentDialogue variables based on string ID
    public void SetCurrentDialogue( string id)
    {
        DialogueLineData line = GetDialogueLineByID(id);
        if (line == null)
        {
            Debug.LogWarning("Dialogue ID not found: " + id);
            return;
        }

        currentDialogueLineData = line;
        currentDialogueLineID = line.dialogueID;
        currentDialogueName = line.dialogueSpeaker;
        currentDialogueText = line.dialogueText;
        currentContinueStatus = line.dialogueContinue;
        currentResponseOptions = line.resposeOptions;
    }
    
    //Sets the current dialogue and calls TypeText
    public void PrintDialogue(string dialogueLineID)
    {
        typerComplete = false;
        SetCurrentDialogue(dialogueLineID);

        if (canvasController != null && canvasController.dialogueCanvas != null)
        {
            TypeText(textSpeed);

        }

       // if (typerComplete) canvasController.dialogueCanvas.GetResponseOptions();
        
        currentDialogueIndex++;
    }

    //checks if the currentBranchID string contains the returned response ID or if the currentDialogueLine != returned response ID.
    //if current continue status != "BREAK" it progresses to NextID... otherwise prints the responseID... all other paths clear the dialogue
    public void ProgressDialogue()
    {
        
        if (currentDialogueLineData != null && !string.IsNullOrEmpty(currentDialogueLineData.nextID))
        {
            //Added for branching dialogue but will only check if NOT first option
            if (currentBranchID.Contains(responseReturnID) || currentDialogueLineID!=responseReturnID) { SetCurrentDialogue(responseReturnID); PrintDialogue(responseReturnID);Debug.Log("ResponseTriggered"); }
            else if (currentContinueStatus != "BREAK")
            {
                Debug.Log("Next ID Triggered");
                PrintDialogue(currentDialogueLineData.nextID);
            }
            if(currentContinueStatus == "BREAK")ClearDialogue(); 
        }
        else
        {
            ClearDialogue(); 
        }
        typerComplete = false;
    }

    //calls the function from the dialogue canvas
    public void ClearDialogue()
    {
        canvasController.dialogueCanvas.ClearDialogueCanvas();
        isRetrigger = true;
    }

    public bool SkipDialogue(Action SkipLine)
    {

        return true;
    }

    //sets the text speed to type (in ms)
    public int SetTextSpeed(int speed)
    {
        currentTextSpeed = speed;
        return currentTextSpeed;
    }

    //turns the currentDialogueText string into an array and loops foreach letter and updates the canvas, then delays based on textSpeed sent in
    //waits 2s after text done to show buttons
    public async void TypeText(int speed)
    {
        string temp = "";
        foreach (var item in currentDialogueText.AsSpan().ToArray())
        {
            
            Debug.Log(temp);
            temp += item; 
            canvasController.dialogueCanvas.UpdateDialogueUI(
            currentDialogueName,
            temp,
            currentDialogueImage);
            //Add narrative sounds / function call here
            await Task.Delay(speed);

        }
        
        await Task.Delay(2000);
        ShowResponseButtons(true);
    }
    //sends the current response options to the dialogue canvas
    public void SendResponseOptions()
    {

        canvasController.SendResponseOptions(currentResponseOptions);
    }

    //calls function in dialogue canvas to show/spawn response buttons
    private void ShowResponseButtons(bool ready)
    {
        typerComplete = ready;
        if (typerComplete)
        {
            canvasController.dialogueCanvas.GetResponseOptions();
            
        }
        
    }

}
