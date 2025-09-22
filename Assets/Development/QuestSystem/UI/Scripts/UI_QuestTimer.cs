using UnityEngine;
using TMPro;


public class UI_QuestTimer : MonoBehaviour
{
    public GameObject timerCanvas;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private QuestLog questLog;
    [SerializeField]private float canvasTime;
    public bool isTimerActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerCanvas = this.gameObject;
        //timerText = GetComponent<TextMeshProUGUI>();
        questLog = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestLog>();
        isTimerActive = true;
        

    }

    // Update is called once per frame
    void Update()
    {
        canvasTime = questLog.currentTime;
        if (isTimerActive)
        {
            UpdateTimer(canvasTime);
        }
    }

    public void UpdateTimer(float timer)
    {
        if (timer != 0)
        {
            int intTime = (int)timer;
            timerText.SetText(intTime.ToString());
        }
       else {
            isTimerActive = false;
            
        }
    }

    public void DestroyTimer()
    {
        Destroy(gameObject);
    }

}
