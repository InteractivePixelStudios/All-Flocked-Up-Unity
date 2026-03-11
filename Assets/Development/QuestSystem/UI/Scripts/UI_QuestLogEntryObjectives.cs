using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class UI_QuestLogEntryObjectives : MonoBehaviour
{
    public LocalizedString description;
    public int quantity;
    [SerializeField]TextMeshProUGUI descText;
    [SerializeField]TextMeshProUGUI quantText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        description.StringChanged += value =>
        {
            descText.text = value;
        };
        quantText.SetText(quantity.ToString());
    }

}
