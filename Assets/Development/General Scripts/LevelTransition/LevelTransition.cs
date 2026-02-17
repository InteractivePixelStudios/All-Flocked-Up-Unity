using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private GameObject promptPrefab;
    [Tooltip("MUST BE THE EXACT SPELLING OF THE SCENE NAME! :)")]
    [SerializeField] private string nextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void ShowTransitionPrompt()
    {
        Instantiate(promptPrefab);
    }

    public void ChangeToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowTransitionPrompt();
        }
    }
}
