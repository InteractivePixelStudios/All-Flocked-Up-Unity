using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialPrompt : MonoBehaviour
{
    [SerializeField] private GameObject confirmWindow;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button skipConfirmButton;
    [SerializeField] private Button skipCancelButton;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private List<string> prompts = new();
    public int promptIndex;
    public UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        confirmButton.onClick.AddListener(CloseWindow);
        skipButton.onClick.AddListener(SkipTutorial);
        skipConfirmButton.onClick.AddListener(ConfirmSkip);
        skipCancelButton.onClick.AddListener(CloseConfirmWindow);
        UpdatePrompt();
    }

    public void UpdatePrompt()
    {
        promptText.SetText(prompts[promptIndex]);
    }


    void CloseWindow()
    {
        canvasController.DestroyPrompt();
    }

    void SkipTutorial()
    {
        ShowConfirmWindow();
    }

    void ShowConfirmWindow()
    {
        confirmWindow.SetActive(true);
    }

    void ConfirmSkip()
    {
        SceneManager.LoadScene("KensingtonMarket");
    }

    void CloseConfirmWindow()
    {
        confirmWindow.SetActive(false);
    }
}
