using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;


public class UI_DialogueCanvas : MonoBehaviour
{
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private DialogueBase dialogueBase;
    [SerializeField] private ScrollRect responseBox;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private int textSpeed = 1;
    public string[] responses;
    [SerializeField] private string responseReturnID;
    [SerializeField] private bool hasButtons = false;

    bool skipDialogue;
    public Action SkipLine { get; private set; }
    private void Awake()
    {
        dialogueCanvas = GetComponent<Canvas>();
        dialogueBase = FindFirstObjectByType<DialogueBase>();
        //dialogueImage = GetComponent<Image>();
    }
    void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
        textSpeed=dialogueBase.textSpeed;
    }
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            if (responses != null)
            {
                // ProgressDialogueCanvas();
            }
        }
    }

    public void UpdateDialogueUI(string name,string dialogue, Image image)
    {
        dialogueText.SetText(dialogue);
        nameText.SetText(name);
        dialogueImage.material = image.material;
    }

    public void ClearDialogueCanvas()
    {
        
        dialogueText.SetText("");
        nameText.SetText("");
        DestroyDialogue();
       
    }

    public void DestroyDialogue()
    {
        this.gameObject.SetActive(false);
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
        responses = new string[dialogueBase.currentResponseOptions.Length];
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        hasButtons = true;  
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

    private Button CreateResponseButton(string text, string branchOption, float startY,float offset)
    {
        Button response = Instantiate(buttonPrefab, responseBox.transform);
        RectTransform buttonTransform = response.GetComponent<RectTransform>();

        SetButtonTransform(buttonTransform,startY,offset);
        SetButtonText(response, text);

        response.onClick.AddListener(() => ResponseClicked(branchOption));
        return response;
    }

    private void SetButtonTransform(RectTransform transform, float startY,float offset)
    {
        transform.anchorMin = new Vector2(0.5f, 1);
        transform.anchorMax = new Vector2(0.5f, 1);
        transform.pivot = new Vector2(0.5f, 1);
        transform.anchoredPosition = new Vector2(0, startY);

    }

    private void SetButtonText(Button button, string text)
    {
        var labelText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (labelText != null) labelText.SetText(text);
    }

}
