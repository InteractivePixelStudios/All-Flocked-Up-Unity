using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HideSeekCountdown : MonoBehaviour
{
    [SerializeField] Image overlay;
    UI_CanvasController canvasController;
    [SerializeField] Image startIcon;
    [SerializeField] TextMeshProUGUI countText;

    float timer = 5f;
    float fadeTime = 2f;
    bool countStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        ToggleStartImage();

    }

    // Update is called once per frame
    void Update()
    {
        if (countStarted)
        {


            timer -= Time.deltaTime;
            UpdateCountText(timer);
            if (timer > 5f)
            {
                if (fadeTime > 0f)
                {
                    fadeTime -= Time.deltaTime;
                    FadeIn();
                }
            }
            else if (timer <= 0)
            {
                FadeOut();
                if (countText.gameObject.activeSelf) { countText.gameObject.SetActive(false); }
            }
            if (timer <= -2)
            {
                DestroyCanvas();
            }
        }
    }

    void FadeIn()
    {
        var color = overlay.color;
        var startA = color.a;
        var maxA = 1;
        var finalA = Mathf.Lerp(startA, maxA, Time.deltaTime);
        color.a = finalA;
        overlay.color = color;
    }

    void FadeOut()
    {
        var color = overlay.color;
        var startA = color.a;
        var maxA = 0;
        var finalA = Mathf.Lerp(startA, maxA, Time.deltaTime);
        color.a = finalA;
        overlay.color = color;
    }

    async void ToggleStartImage()
    {
        startIcon.gameObject.SetActive(true);
        await Task.Delay(1500);
        startIcon.gameObject.SetActive(false);
        StartCount();

    }

    void UpdateCountText(float time)
    {
        if(time > 5 && time < 0) { countText.SetText(" "); }
        if (countText.text != time.ToString())
        {
            countText.SetText(time.ToString("F0"));
        }
        else return;
    }

    async void StartCount()
    {
        countStarted = true;
        await Task.Delay(2000);
        FadeIn();
    }

    void DestroyCanvas()
    {
        canvasController.currentHASCountdown = null;
        Destroy(this.gameObject);
    }


}
