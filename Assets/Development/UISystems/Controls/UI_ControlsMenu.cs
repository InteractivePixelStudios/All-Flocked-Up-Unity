using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

public class UI_ControlsMenu : UI_PauseMenu
{
    [Header("Controls")]
    [SerializeField] private Button closeControlsButton;
    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private TextMeshProUGUI meshSensText;
    [SerializeField] private Slider controllerSensSlider;
    [SerializeField] private TextMeshProUGUI controlSensText;
    [SerializeField] private GameObject remapParent;
    [SerializeField] private RectTransform remapBox;
    [SerializeField] private Button confirmRemapButton;
    [SerializeField] private Button cancelRemapButton;
    [SerializeField] private TMP_Dropdown controlImageDropdown;
    [SerializeField] private Image controlImage;
    [SerializeField] private Sprite keyboardImage;
    [SerializeField] private Sprite gamepadImage;
    [SerializeField] private Button rebindButton;
    [Header("Keybinds")]
    [SerializeField] private Transform keybindContainer;
    [SerializeField] private GameObject keybindBoxPrefab;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject rebindNotifPrefab;
    [SerializeField] private GameObject currentNotif;
    [SerializeField] private bool isRebinding = false;
    [SerializeField] private string currentAction = " ";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeControlsButton.onClick.AddListener(OnControlsOpen);
        confirmRemapButton.onClick.AddListener(ConfirmRemap);
        cancelRemapButton.onClick.AddListener(CancelRemap);
        rebindButton.onClick.AddListener(OpenRemap);
        InitControlImageDD();
        InitKeybindBox();
        InitMouseSensSlider();
        InitControllerSensSlider();
    }

    private void Update()
    {
        DetectKeypress();        
    }


    protected void InitMouseSensSlider()
    {

    }

    protected void OnMouseSensChanged()
    {

    }

    protected void ApplyMouseSens()
    {

    }

    protected void InitControllerSensSlider()
    {

    }

    protected void OnControllerSensChanged()
    {

    }

    protected void ApplyControllerSens()
    {

    }

    protected new void OnControlsOpen()
    {
        base.OnControlsOpen();
    }

    protected void ConfirmRemap()
    {
        remapParent.SetActive(false);
    }

    protected void CancelRemap()
    {
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
        int index = PlayerPrefs.GetInt("ControlImage");
        controlImageDropdown.value = index;
        controlImageDropdown.RefreshShownValue();

        SetControlImage(index);
        PlayerPrefs.SetInt("ControlImage",index);
        PlayerPrefs.Save();

        controlImageDropdown.onValueChanged.AddListener(OnControlImageChanged);
    }

    protected void OnControlImageChanged(int index)
    {
        switch (index)
        {
            case 0:SetControlImage(0); break;
            case 1:SetControlImage(1); break;
        }
    }

    protected void SetControlImage(int index)
    {
        if (index == 0)
        {
            controlImage.sprite = keyboardImage;
        }
        else { controlImage.sprite = gamepadImage; }
    }

    protected Dictionary<string,string> GetAllKeybinds(InputActionAsset inputActions)
    {
       var keybinds = new Dictionary<string,string>();
        foreach(var map in inputActions.actionMaps) 
        {
            foreach(var action in map)
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
        currentAction = action;
        ShowRebindNotif(action);
        isRebinding = true;
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

    protected void DetectKeypress()
    {
        if (!isRebinding) return;
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            foreach (var key in Keyboard.current.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    string keyName = key.displayName;
                    ApplyKeybind(keyName);
                    break;
                }
            }
        }
    }

    protected void ApplyKeybind(string keyName)
    {
        UpdateNotif(keyName);
        //PlayerPrefs.SetString(currentAction + " Key", keyName);
        //PlayerPrefs.Save();
        isRebinding = false;
    }
}
