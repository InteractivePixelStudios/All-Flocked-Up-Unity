using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_LevelTransition : MonoBehaviour
{
    public UI_CanvasController canvasController;
    public LevelTransition transitionObj;
    [SerializeField] private TextMeshProUGUI text;
    public string sceneName;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.SetText(sceneName);
        confirmButton.onClick.AddListener(ConfirmTravel);
        cancelButton.onClick.AddListener(CancelTravel);
    }

    void ConfirmTravel()
    {
        transitionObj.ChangeToNextScene();
    }

    void CancelTravel()
    {
        canvasController.CloseLevelTransition();
    }


}
