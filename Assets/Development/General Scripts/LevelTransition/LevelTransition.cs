using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [Tooltip("MUST BE THE EXACT SPELLING OF THE SCENE NAME! :)")]
    [SerializeField] private string nextScene;
    [SerializeField] private UI_CanvasController canvasController;

    private void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void ShowTransitionPrompt()
    {
        canvasController.transitionObj = this;
        canvasController.cachedLevelName = nextScene;
        canvasController.OpenLevelTransition();
    }

    public void ChangeToNextScene()
    {
        canvasController.CloseLevelTransition();
        SceneManager.LoadScene(nextScene);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowTransitionPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasController.CloseLevelTransition();
        }
    }
}
