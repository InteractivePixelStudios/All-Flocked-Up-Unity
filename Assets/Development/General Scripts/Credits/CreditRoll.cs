using NUnit.Framework;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Data;
using UnityEngine.SceneManagement;

public class CreditRoll : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI roleText;
    [SerializeField] private Button skipButton;
    [SerializeField] private List<Sprite> splashImages;
    [SerializeField] private Dictionary<string, string> creditsNamesList;
    [SerializeField] private List<string> nameList = new();
    [SerializeField] private List<string> roleList = new();
    private int nameIndex=0;
    private int imageIndex = 0;
    [SerializeField] private Image currentImage;
    [SerializeField] private float timePerName;
    [SerializeField] private float currentTime;
    [SerializeField] private bool inReady;
    [SerializeField] private bool outReady;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameText.alpha = 0f;
        roleText.alpha = 0f;
        currentImage.gameObject.SetActive(false);
        skipButton.onClick.AddListener(SkipCredits);
    }

    // Update is called once per frame
    void Update()
    {
        RollTimer();
        if (inReady &&!outReady) EffectIn();
        if (outReady) EffectOut();

    }

    private void RollTimer()
    {
        if (currentTime <= 0)
        {
            if(nameIndex < nameList.Count)
            {
                CycleNames();
                currentTime = timePerName;
            }
            else
            {
                if (!currentImage.gameObject.activeInHierarchy) { currentImage.gameObject.SetActive(true); }
                CycleImage();
                currentTime = timePerName;
            }
        }
        currentTime -= Time.deltaTime;
    }



    private async void CycleNames()
    {
        inReady = true;
        outReady = false;
        Debug.Log("CycleCalled");
        nameText.SetText(nameList[nameIndex]);
        roleText.SetText(roleList[nameIndex]);
        await Task.Delay(5000);
        nameIndex++;

    }

    private async void CycleImage()
    {
        currentImage.sprite = splashImages[imageIndex];
        await Task.Delay(6000);
        imageIndex++;


    }

    private void EffectIn()
    {
        if (nameText.alpha < 1)
        {
            nameText.alpha += Time.deltaTime;
            roleText.alpha += Time.deltaTime;
        }
        else
        {
            inReady = false;
            outReady = true;
        }


    }

    private void EffectOut()
    {
        if (nameText.alpha >= 0)
        {
            nameText.alpha -= Time.deltaTime;
            roleText.alpha -= Time.deltaTime;
        }
        else
        {
            outReady = false;
        }
    }

    void SkipCredits()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
