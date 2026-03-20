using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private List<string> tipsList = new();
    [SerializeField] private List<Sprite> birdList = new();
    [SerializeField] private Image birdImage;
    [SerializeField] private TextMeshProUGUI tipText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetRandomTip();
        DestroyCanvas();
    }

    void GetRandomTip()
    {
        int random = Random.Range(0,tipsList.Count);
        tipText.SetText(tipsList[random]);
    }

    void GetBirdImage()
    {
        int random= Random.Range(0,birdList.Count);
        birdImage.sprite = birdList[random];
    }

    async void DestroyCanvas()
    {

        var menu = FindAnyObjectByType<UI_MainMenu>();
        if(menu != null)
        {

            menu.SetSelectedObject(menu.startButton.gameObject);
            Debug.Log("MenuButtonSet");
        }
        await Task.Delay(3000);
        Destroy(this.gameObject);
    }
}
