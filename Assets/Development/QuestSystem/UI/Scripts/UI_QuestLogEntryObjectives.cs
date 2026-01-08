using TMPro;
using UnityEngine;

public class UI_QuestLogEntryObjectives : MonoBehaviour
{
    public string description;
    public int quantity;
    [SerializeField]TextMeshProUGUI descText;
    [SerializeField]TextMeshProUGUI quantText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        descText.SetText(description);
        quantText.SetText(quantity.ToString());
    }

}
