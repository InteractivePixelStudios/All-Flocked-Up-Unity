using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespawnController : MonoBehaviour
{
    private static Vector3 startLoc;
    [Header("Respawn Components")]
    [SerializeField] private TextMeshProUGUI respawnText; 
    [SerializeField] private GameObject player; 
    [SerializeField] private RagdollController ragdoll;
    public UI_CanvasController canvasController;

    [Header("Respawn Nest Configuration")]
    [SerializeField] private NestBase[] respawnNests; 
    [SerializeField] private int currentNestIndex = 0; 

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button respawnButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ragdoll = GetComponent<RagdollController>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            startLoc = transform.position;
            ragdoll = player.GetComponent<RagdollController>();
        }
        respawnNests = FindObjectsByType<NestBase>();
        nextButton.onClick.AddListener(NextNest);
        prevButton.onClick.AddListener(PreviousNest);
        respawnButton.onClick.AddListener(RespawnPlayer);
        NextNest(); 
    }


    void Update()
    {

    }

    public void NextNest()
    {
        currentNestIndex++;
        if (currentNestIndex >= respawnNests.Length)
        {
            currentNestIndex = 0; 
        }
        Camera.main.GetComponent<CameraController>().respawnTarget = respawnNests[currentNestIndex].transform; 
        respawnText.text = respawnNests[currentNestIndex].GetComponent<NestBrian>().nestName; 
    }

    public void PreviousNest()
    {
        currentNestIndex--;
        if (currentNestIndex < 0)
        {
            currentNestIndex = respawnNests.Length - 1; 
        }
        Camera.main.GetComponent<CameraController>().respawnTarget = respawnNests[currentNestIndex].transform;
        respawnText.text = respawnNests[currentNestIndex].GetComponent<NestBrian>().nestName; 
    }
    public void RespawnPlayer()
    {
        if (respawnNests.Length == 0)
        {
            ragdoll.ToggleRagdollOff();
            player.transform.position = startLoc; 
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth; 
            Camera.main.GetComponent<CameraController>().SwitchToPlayer(); 
            Destroy(gameObject);
            return; // No nests to respawn to
        }
        ragdoll.ToggleRagdollOff();
        player.transform.position = respawnNests[currentNestIndex].transform.position; 
        player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth; 
        Camera.main.GetComponent<CameraController>().SwitchToPlayer();
        canvasController.CloseRespawn();
        Destroy(gameObject); 
    }
}
