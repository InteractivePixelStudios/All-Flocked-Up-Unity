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
    [SerializeField] private Image birdImage;
    [SerializeField] private TextMeshProUGUI tipText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetRandomTip();
        DestroyCanvas();
        var player = FindAnyObjectByType<PlayerHealth>();
        var ground = player.GetComponent<PlayerGroundMovement>();
        var flight = player.GetComponent<PlayerFlightMovement>();
        ground.enabled = true;
        flight.enabled = true;
    }

    void GetRandomTip()
    {
        int random = Random.Range(0,tipsList.Count);
        tipText.SetText(tipsList[random]);
    }

    async void DestroyCanvas()
    {
        await Task.Delay(3000);
        Destroy(this.gameObject);
    }
}
