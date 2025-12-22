using TMPro;
using UnityEngine;

public class UI_InteriorPrompt : MonoBehaviour
{
    public string prompt;
    [SerializeField] private TextMeshProUGUI promptText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        promptText.SetText(prompt);
    }


}
