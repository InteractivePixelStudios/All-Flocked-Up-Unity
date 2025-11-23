using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DebugMenu : MonoBehaviour
{
    [SerializeField] private DebugManager debugManager;
    [SerializeField] private Toggle stamToggle;
    [SerializeField] private Button giveLevelButton;
    [SerializeField] private Button maxLevelButton;
    [SerializeField] private TMP_InputField trinketInput;
    [SerializeField] private Toggle stealthToggle;
    [SerializeField] private Toggle poopToggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        debugManager = new();
        giveLevelButton.onClick.AddListener(debugManager.GivePlayerLevel);
        maxLevelButton.onClick.AddListener(debugManager.GivePlayerMaxLevel);
    }

    private void OnStaminaToggled()
    {

    }
    
}
