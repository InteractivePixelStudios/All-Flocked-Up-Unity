using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_QuestLocationNotif : MonoBehaviour
{

    [SerializeField] GameObject locationNotifCanvas;
    [SerializeField] TextMeshProUGUI locationText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowQuestLocationNotif();
    }


    public async void ShowQuestLocationNotif()
    {
        locationNotifCanvas.SetActive(true);
        await Task.Delay(2000);
        Destroy(locationNotifCanvas);
    }



}
