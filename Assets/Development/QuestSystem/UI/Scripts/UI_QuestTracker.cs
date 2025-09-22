using UnityEngine;

public class UI_QuestTracker : MonoBehaviour
{
    [SerializeField] GameObject questTrackerCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questTrackerCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TrackCurrentQuest()
    {
        questTrackerCanvas.SetActive(true);
    }

    public void RemoveTracker()
    {
        Destroy(questTrackerCanvas);
    }
}
