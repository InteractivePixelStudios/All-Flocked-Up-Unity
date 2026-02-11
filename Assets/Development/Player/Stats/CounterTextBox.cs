using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CounterTextBox : MonoBehaviour
{
    public string statName;
    public int statNumber;
    [SerializeField]private TextMeshProUGUI nameText;
    [SerializeField]private TextMeshProUGUI numberText;

    private void Start()
    {

        UpdateText();
    }

    void UpdateText()
    {
        nameText.SetText(statName);
        numberText.SetText(statNumber.ToString());
    }
}
