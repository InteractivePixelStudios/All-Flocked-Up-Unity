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

    [SerializeField] protected GameObject[] arrowPointers;
    protected int arrowIndex;
    [SerializeField] private Image controlBindImage;
    [SerializeField] protected Sprite[] controlBindSprites;
    protected int controlBindIndex = 1;
    [SerializeField] private Image keyBindImage;
    [SerializeField] protected Sprite[] keyboardSprites;
    protected int keyboardBindIndex = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePrompt();
        SetArrowPointers(arrowIndex);
    }

    public void UpdateBindSprites(int keyBindIndex, int controllerBindIndex)
    {
        keyBindImage.sprite = keyboardSprites[keyBindIndex];
        controlBindImage.sprite = controlBindSprites[controllerBindIndex];
    }

    public void IncrementBindIndex()
    {
        keyboardBindIndex++;
        controlBindIndex++;
    }

    public int GetNumberArrowPointer()
    {
        return arrowPointers.Length;
    }

    public void SetArrowIndex(int index)
    {
        arrowIndex = index;
    }

    public void HideArrowPointers()
    {
        foreach(var arrow in arrowPointers)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    public void SetArrowPointers(int index)
    {
        if (index ==0)
        {
            arrowPointers[0].SetActive(true);
        }
        else if (index == 1)
        {
            arrowPointers[0].SetActive(false);
            arrowPointers[1].SetActive(true);
        }
        else if(index == 2)
        {
            arrowPointers[1].SetActive(false);
            arrowPointers[2].SetActive(true);
        }
        else if(index == 3)
        {
            arrowPointers[2].SetActive(false);
            arrowPointers[3].SetActive(true);
        }
        else if (index == 4)
        {
            arrowPointers[3].SetActive(false);
            arrowPointers[4].SetActive(true);
        }
        else if(index == 5)
        {
            arrowPointers[4].SetActive(false);
            arrowPointers[5].SetActive(true);
        }
        else
        {
            HideArrowPointers();
        }

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
