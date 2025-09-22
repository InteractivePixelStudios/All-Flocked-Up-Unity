using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestNotif : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI questNotifText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questNotifText = GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNotifText(string text)
    {
        questNotifText.text = text;
    }

    public async void ShowQuestNotif()
    {
        await Task.Delay(1500);
        Destroy(this.gameObject);
    }
}
