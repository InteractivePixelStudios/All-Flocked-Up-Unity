using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HASComplete : MonoBehaviour
{
    UI_CanvasController canvasController;
    private string currentState;
    private int currentReward;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Button confirmButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        confirmButton.onClick.AddListener(ConfirmClose);
    }

    public void SetState(string state, int reward)
    {
        currentState = state;
        currentReward = reward;
        UpdateText(currentState, currentReward);
    }

    void UpdateText(string state, float reward)
    {
        if(state == "Complete")
        {
            stateText.SetText("Complete");
            rewardText.SetText(currentReward.ToString());

        }else if (state == "Failed")
        {
            stateText.SetText("Failed");
            rewardText.SetText(" ");
        }
    }

    void ConfirmClose()
    {
        canvasController.CloseHASComplete();
    }
}
