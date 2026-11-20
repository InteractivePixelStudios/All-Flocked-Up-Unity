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
    [SerializeField] private NestBase homeNest;

    [Header("Buttons")]
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
        homeNest = FindAnyObjectByType<NestBase>();
        respawnButton.onClick.AddListener(RespawnPlayer);
        respawnText.SetText("HOME");
    }

    public void RespawnPlayer()
    {
        if (homeNest!=null)
        {
            ragdoll.ToggleRagdollOff();
            player.transform.position = startLoc; 
            player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth; 
            Camera.main.GetComponent<CameraController>().SwitchToPlayer(); 
            Destroy(gameObject);
            return; // No nests to respawn to
        }
        ragdoll.ToggleRagdollOff();
        player.transform.position = homeNest.transform.position; 
        player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth; 
        Camera.main.GetComponent<CameraController>().SwitchToPlayer();
        canvasController.CloseRespawn();
        Destroy(gameObject); 
    }
}
