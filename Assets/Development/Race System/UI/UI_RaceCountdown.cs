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
    [SerializeField] private List<Material> imageMaterialList;

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
            numberImage.material = imageMaterialList[5];
        }
        else if ( timer >= 4)
        {
            numberImage.material = imageMaterialList[4];
        }
        else if( timer >= 3)
        {
            numberImage.material = imageMaterialList[3];
        }
        else if (timer >= 2)
        {
            numberImage.material = imageMaterialList[2];
        }
        else if(timer >= 1)
        {
            numberImage.material = imageMaterialList[1];
        }
        else
        {
            numberImage.material = imageMaterialList[0];
            Task.Delay(1000);
            Destroy(this.gameObject);
        }
    }
}
