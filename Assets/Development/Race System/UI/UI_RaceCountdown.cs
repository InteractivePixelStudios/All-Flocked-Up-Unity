using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UI_RaceCountdown : MonoBehaviour
{
    private float timer;
    [SerializeField] private Image numberImage;
    [SerializeField] private RaceBase raceBase;
    [SerializeField] private List<Sprite> imageMaterialList;

    private void Start()
    {
        if(numberImage == null)
        {
            numberImage = GetComponentInChildren<Image>();
        }
        if(raceBase == null)
        {
            raceBase = FindFirstObjectByType<RaceBase>();
        }


    }
    // Update is called once per frame
    void Update()
    {
        GetTimer();
        SetImage();
       
    }

    private void GetTimer()
    {
        timer = raceBase.countdown;
    }

    private void SetImage()
    {
        if ( timer >= 5)
        {
            numberImage.sprite = imageMaterialList[5];
        }
        else if ( timer >= 4)
        {
            numberImage.sprite = imageMaterialList[4];
        }
        else if( timer >= 3)
        {
            numberImage.sprite = imageMaterialList[3];
        }
        else if (timer >= 2)
        {
            numberImage.sprite = imageMaterialList[2];
        }
        else if(timer >= 1)
        {
            numberImage.sprite = imageMaterialList[1];
        }
        else
        {
            numberImage.sprite = imageMaterialList[0];
            Task.Delay(1000);
            Destroy(this.gameObject);
        }
    }
}
