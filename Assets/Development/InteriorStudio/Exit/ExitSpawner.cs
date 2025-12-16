using UnityEngine;

public class ExitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private UI_InteriorPrompt promptPrefab;
    [SerializeField] private UI_InteriorPrompt currentPrompt;
    [SerializeField] private BoxCollider boxCollider;
    public Vector3 startLocation;
    [SerializeField] private bool isPromptShown;
    [SerializeField] private bool isTeleporting;
    [SerializeField] private bool isReady;
    public string locationName;

    private void Start()
    {

    }

    private void Update()
    {
        if (isPromptShown && isReady)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isTeleporting)
                {
                    GoToStartLocation();
                }
                else return;
            }
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            if (player != null)
            {
                ShowPrompt();
                isReady = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            isReady = false;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if(currentPrompt == null)
        {
            currentPrompt = Instantiate(promptPrefab);
            currentPrompt.prompt = "Travel to " + locationName;
            isPromptShown = true;
        }

    }

    private void HidePrompt()
    {
        if(currentPrompt != null)
        {
            Destroy(currentPrompt);
            isPromptShown = false;
        }
    }

    private void GoToStartLocation()
    {
        isTeleporting = true;
        player.transform.position = startLocation;
        HidePrompt();
        isReady = false;
        isTeleporting = false;
    }
}
