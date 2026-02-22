using UnityEngine;

public class TutorialPromptManager : MonoBehaviour
{
    [SerializeField] private int promptIndex;
    private UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasController.ShowTutorialPrompt();
            canvasController.cachedTutPromptIndex = promptIndex;
        }
        
    }
}
