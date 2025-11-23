using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class UI_BugReporter : MonoBehaviour
{
    [SerializeField] private TMP_InputField bugTextBox;
    [SerializeField] private Slider ratingSlider;
    [SerializeField] private TextMeshProUGUI sliderValueText;
    private int ratingValue;
    [SerializeField] private TMP_InputField commentTextBox;
    [SerializeField] private Button submitButton;
    private GameObject bugReportManager;
    private BugReportManager reportManagerRef;

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitBugReport);
        bugReportManager = new GameObject();
        reportManagerRef = bugReportManager.AddComponent<BugReportManager>();
        InitSlider();
    }

    private void SubmitBugReport()
    {
        string bugText = bugTextBox.text;
        string commentText = commentTextBox.text;
        int rating = ratingValue;
        reportManagerRef.SubmitBug(bugText, "PC", rating, commentText);
        Destroy(bugReportManager);
        Destroy(this.gameObject);
    }

    private void InitSlider()
    {
        ratingSlider.onValueChanged.AddListener(OnSliderChanged);
        OnSliderChanged(ratingSlider.value);
    }

    private void OnSliderChanged(float value)
    {
        sliderValueText.SetText(value.ToString());
        ratingValue = Mathf.RoundToInt(value);
    }
}
