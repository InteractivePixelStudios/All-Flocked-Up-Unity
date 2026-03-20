using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class UI_DialogueCanvas : MonoBehaviour
{
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private Image dialogueImage2;
    [SerializeField] private DialogueBase dialogueBase;
    [SerializeField] private ScrollRect responseBox;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private int textSpeed = 1;
    public LocalizedString[] responses;
    [SerializeField] private string responseReturnID;
    [SerializeField] private bool hasButtons = false;
    UI_CanvasController canvasController;
    PlayerInput playerInput;
    InputAction skipAction;

    bool skipDialogue;
    public Action SkipLine { get; private set; }
    private void Awake()
    {
        dialogueCanvas = GetComponent<Canvas>();
        dialogueBase = FindAnyObjectByType<DialogueBase>();
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        //dialogueImage = GetComponent<Image>();
    }
    void Start()
    {
        textSpeed=dialogueBase.textSpeed;
        skipAction = playerInput.currentActionMap.FindAction("Click");
        if (skipAction != null)
        {
            skipAction.performed += SkipDialogueLine;
        }
    }
    void Update()
    {
        
    }

    private void SkipDialogueLine(InputAction.CallbackContext ctx)
    {
        dialogueBase.SetSkipLine(true);
    }



    public void UpdateDialogueUI(string name,string dialogue, Sprite image)
    {
        dialogueText.SetText(dialogue);
        nameText.SetText(name);
        if (name == "Peep")
        {
            dialogueImage.gameObject.SetActive(true);
            dialogueImage2.gameObject.SetActive(false);
            dialogueImage.sprite = image;
            
        }
        else 
        {
            dialogueImage.gameObject.SetActive(false);
            dialogueImage2.gameObject.SetActive(true);
            dialogueImage2.sprite = image; 
        }
    }

    public void ClearDialogueCanvas()
    {
        
        dialogueText.SetText("");
        nameText.SetText("");
        DestroyDialogue();
       
    }

    public void DestroyDialogue()
    {
        canvasController.CloseDialogue();
    }


    public void ProgressDialogueCanvas()
    {
        dialogueBase.ProgressDialogue();
    }

    public async void TypeText()
    {
        foreach (var item in dialogueText.ToString().AsSpan().ToArray())
        {
            Debug.Log(item);
            await Task.Delay(500);
        }

    }

    public void GetResponseOptions()
    {
        DestroyCurrentOptionButtons();
        responses = new LocalizedString[dialogueBase.currentResponseOptions.Length];
        dialogueBase.currentResponseOptions.CopyTo(responses.AsSpan());
        Debug.Log("UISpawnResponseButtons");
        float startY = 0f;
        float offset = .2f;
        int index = 0;
        if (responses.Length>3) offset = 0.15f;

        foreach (var item in responses)
        {
            string[] capturedOptions = dialogueBase.currentDialogueLineData.branchID.Split('|');
            string selectedOption = capturedOptions[index];
            CreateResponseButton(item,selectedOption,startY,offset);
            startY -= offset;
            index++;
        }
        hasButtons = true;
        Cursor.visible = true ;
    }

    private void ResponseClicked(string option)
    {
        Debug.Log("responseClicked");
        dialogueBase.responseReturnID = responseReturnID = option;
        Cursor.visible = false;
        DestroyCurrentOptionButtons(); Debug.Log("destroycalled");
        dialogueBase.ProgressDialogue();
    }

    private void DestroyCurrentOptionButtons()
    {
        foreach (RectTransform child in responseBox.transform)
        {
            Destroy(child.gameObject);
        }
        hasButtons = false;
    }

    private Button CreateResponseButton(LocalizedString text, string branchOption, float startY,float offset)
    {
        Button response = Instantiate(buttonPrefab, responseBox.transform);
        RectTransform buttonTransform = response.GetComponent<RectTransform>();

        SetButtonTransform(buttonTransform,startY,offset);
        SetButtonText(response, text);

        response.onClick.AddListener(() => ResponseClicked(branchOption));
        EventSystem.current.SetSelectedGameObject(response.gameObject);
        return response;
    }

    private void SetButtonTransform(RectTransform transform, float startY,float offset)
    {
        transform.anchorMin = new Vector2(0.5f, 1);
        transform.anchorMax = new Vector2(0.5f, 1);
        transform.pivot = new Vector2(0.5f, 1);
        transform.anchoredPosition = new Vector2(0, startY);

    }

    private void SetButtonText(Button button, LocalizedString text)
    {
        var labelText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (labelText != null)
        {
            text.GetLocalizedStringAsync().Completed += handle =>
            {
                labelText.SetText(handle.Result);
            };
        }
    }

}
