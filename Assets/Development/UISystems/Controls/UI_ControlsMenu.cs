using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class UI_ControlsMenu : UI_PauseMenu
{
    [SerializeField] private PlayerInput playerInput;
    [Header("Controls")]
    [SerializeField] private Button closeControlsButton;
    [SerializeField] private Slider mouseSensSlider;
    private float mouseSens;
    [SerializeField] private TextMeshProUGUI meshSensText;
    [SerializeField] private Slider controllerSensSlider;
    private float controllerSens;
    [SerializeField] private TextMeshProUGUI controlSensText;
    [SerializeField] private GameObject remapParent;
    [SerializeField] private RectTransform remapBox;
    [SerializeField] private Button confirmRemapButton;
    [SerializeField] private Button cancelRemapButton;
    [SerializeField] private TMP_Dropdown controlImageDropdown;
    [SerializeField] private Image controlImage;
    [SerializeField] private GameObject keyboardImage;
    [SerializeField] private GameObject gamepadImage;
    [SerializeField] private Button rebindButton;
    [Header("Keybinds")]
    [SerializeField] private Transform keybindContainer;
    [SerializeField] private GameObject keybindBoxPrefab;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject rebindNotifPrefab;
    [SerializeField] private GameObject currentNotif;
    [SerializeField] private bool isRebinding = false;
    [SerializeField] private string currentAction = " ";
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private InputAction currentInputAction;

    void Start()
    {
        closeControlsButton.onClick.AddListener(OnControlsOpen);
        confirmRemapButton.onClick.AddListener(ConfirmRemap);
        cancelRemapButton.onClick.AddListener(CancelRemap);
        rebindButton.onClick.AddListener(OpenRemap);
        InitControlImageDD();
        LoadBindings();
        InitKeybindBox();
        InitMouseSensSlider();
        InitControllerSensSlider();        
    }

    protected void LoadBindings()
    {
        string json = PlayerPrefs.GetString("InputBindings", "");
        if (!string.IsNullOrEmpty(json))
            inputActions.LoadBindingOverridesFromJson(json);
    }

    public void SetFirstControlsButton()
    {
        EventSystem.current.SetSelectedGameObject(mouseSensSlider.gameObject);
    }

    protected void InitMouseSensSlider()
    {
        mouseSensSlider.onValueChanged.RemoveAllListeners();
        mouseSens = PlayerPrefs.GetFloat("MouseSens", 1f);
        mouseSensSlider.SetValueWithoutNotify(mouseSens);
        meshSensText.SetText(mouseSens.ToString("F2"));
        mouseSensSlider.onValueChanged.AddListener((_) => OnMouseSensChanged());
        if(CameraController.Instance != null)
        {
            CameraController.Instance.SetLookSens(mouseSens);
        }
    }

    protected void OnMouseSensChanged()
    {
        mouseSens = mouseSensSlider.value;
        meshSensText.SetText(mouseSens.ToString("F2"));
        ApplyMouseSens();
    }

    protected void ApplyMouseSens()
    {
        CameraController.Instance.SetLookSens(mouseSens);
        PlayerPrefs.SetFloat("MouseSens", mouseSens);
        PlayerPrefs.Save();
    }

    protected void InitControllerSensSlider()
    {
        controllerSensSlider.onValueChanged.RemoveAllListeners();
        controllerSens = PlayerPrefs.GetFloat("ControllerSens", 1f);
        controllerSensSlider.SetValueWithoutNotify(controllerSens);
        controlSensText.SetText(controllerSens.ToString("F2"));
        controllerSensSlider.onValueChanged.AddListener((_) => OnControllerSensChanged());
        if (CameraController.Instance != null)
        {
            CameraController.Instance.SetLookSens(controllerSens);
        }
    }

    protected void OnControllerSensChanged()
    {
        controllerSens = controllerSensSlider.value;
        controlSensText.SetText(controllerSens.ToString("F2"));
        ApplyControllerSens();
    }

    protected void ApplyControllerSens()
    {
        CameraController.Instance.SetLookSens(controllerSens);
        PlayerPrefs.SetFloat("ControllerSens", controllerSens);
        PlayerPrefs.Save();
    }

    protected new void OnControlsOpen()
    {
        base.OnControlsOpen();
    }

    protected void ConfirmRemap()
    {
        if (isRebinding) CancelRebind();
        remapParent.SetActive(false);
    }

    protected void CancelRemap()
    {
        CancelRebind();
        remapParent.SetActive(false);
    }

    protected void OpenRemap()
    {
        remapParent.SetActive(true);
    }

    protected void InitControlImageDD()
    {
        controlImageDropdown.ClearOptions();
        var options = new List<string>
        {
            "Keyboard/Mouse",
            "Gamepad"
        };
        controlImageDropdown.AddOptions(options);
        int index = Mathf.Clamp(PlayerPrefs.GetInt("ControlImage",0),0,1);
        controlImageDropdown.SetValueWithoutNotify(index);
        controlImageDropdown.RefreshShownValue();
        SetControlImage(index);
        PlayerPrefs.SetInt("ControlImage",index);
        PlayerPrefs.Save();
        controlImageDropdown.onValueChanged.AddListener(OnControlImageChanged);
    }

    protected void OnControlImageChanged(int index)
    {
        SetControlImage(index);
        PlayerPrefs.SetInt("ControlImage", index);
        PlayerPrefs.Save();      
    }

    protected void SetControlImage(int index)
    {
        if (index == 0)
        {
            keyboardImage.SetActive(true);
            gamepadImage.SetActive(false);
        }
        else
        {
            keyboardImage.SetActive(false);
            gamepadImage.SetActive(true);
        }
    }

    protected Dictionary<string,string> GetAllKeybinds(InputActionAsset inputActions)
    {
       var keybinds = new Dictionary<string,string>();

            foreach(var action in inputActions.FindActionMap("Player"))
            {
                foreach(var binding in action.bindings)
                {

                    if (!binding.isComposite && !string.IsNullOrEmpty(binding.path))
                    {
                        string display = binding.ToDisplayString();
                        keybinds[action.name] = display;
                        break;
                    }
                }
            }    
        return keybinds;
    }

    protected void InitKeybindBox()
    {
        var keybinds = GetAllKeybinds(inputActions);
        foreach (Transform child in keybindContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(var bind in keybinds)
        {
            var box = Instantiate(keybindBoxPrefab, keybindContainer);
            var button = box.GetComponent<UI_RemapButton>();
            button.actionText.SetText(bind.Key);
            button.keyText.SetText(bind.Value);
            button.controlsRef = this;
        }
    }

    public void CheckForRebindPressed(string action)
    {
        if (isRebinding) return;
        currentInputAction = inputActions.FindAction("Player/" + action);
        if (currentInputAction == null) return;
        currentAction = action;
        ShowRebindNotif(action);
        isRebinding = true;
        int bindingIndex = -1;
        for (int i = 0; i < currentInputAction.bindings.Count; i++)
        {
            var binding = currentInputAction.bindings[i];
            if (!binding.isComposite && !binding.isPartOfComposite && binding.path.StartsWith("<Keyboard>"))
            {
                bindingIndex = i;
                break;
            }
        }

        if (bindingIndex < 0)
        {
            CancelRebind();
            return;
        }

        rebindOperation = currentInputAction.PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                string display = currentInputAction.GetBindingDisplayString(bindingIndex);
                UpdateNotif(display);
                SaveBindings();
                FinishRebind();
            })
            .OnCancel(operation =>
            {
                FinishRebind();

                if (currentNotif != null) Destroy(currentNotif);
                currentNotif = null;
            })
            .Start();
    }

    protected void SaveBindings()
    {
        string json = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("InputBindings", json);
        PlayerPrefs.Save();
    }

    protected void FinishRebind()
    {
        var operation = rebindOperation;
        rebindOperation = null;
        operation?.Dispose();
        isRebinding = false;
        InitKeybindBox();
    }

    protected void CancelRebind()
    {
        if (rebindOperation != null)
        {
            rebindOperation.Cancel();
            return;
        }
        isRebinding = false;
        if (currentNotif != null) Destroy(currentNotif);
        currentNotif = null;
    }

    protected void ShowRebindNotif(string action)
    {
        currentNotif = Instantiate(rebindNotifPrefab,remapParent.transform);
        var rebind = currentNotif.GetComponent<UI_RebindNotif>();
        rebind.actionText.SetText(action);
        rebind.newKeyText.SetText("Press any key...");
    }

    protected void UpdateNotif(string key)
    {
        if(currentNotif == null) return;
        var rebind = currentNotif.GetComponent<UI_RebindNotif>();
        rebind.newKeyText.SetText(key);
        Task.Delay(2000);
        Destroy(currentNotif.gameObject);
    }
}
