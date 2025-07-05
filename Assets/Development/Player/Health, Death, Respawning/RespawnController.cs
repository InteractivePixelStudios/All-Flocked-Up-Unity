using TMPro;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [Header("Respawn Components")]
    [SerializeField] private TextMeshProUGUI respawnText; // UI text to display respawn information
    [SerializeField] private GameObject player; // Reference to the player GameObject

    [Header("Respawn Nest Configuration")]
    [SerializeField] private GameObject[] respawnNests; // Array of respawn nests
    [SerializeField] private int currentNestIndex = 0; // Index of the current respawn nest

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Find the player GameObject by tag if not assigned
        }
        respawnNests = GameObject.FindGameObjectsWithTag("Nest");
        NextNest(); // Initialize to the first nest
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            NextNest();
        }
        else if (Input.GetAxis("Horizontal") < -0.5f)
        {
            PreviousNest();
        }
    }

    public void NextNest()
    {
        currentNestIndex++;
        if (currentNestIndex >= respawnNests.Length)
        {
            currentNestIndex = 0; // Wrap around to the first nest
        }
        Camera.main.GetComponent<CameraController>().respawnTarget = respawnNests[currentNestIndex].transform; // Update camera target to the new nest
        respawnText.text = respawnNests[currentNestIndex].GetComponent<NestBrian>().nestName; // Update UI text with the current nest name
    }

    public void PreviousNest()
    {
        currentNestIndex--;
        if (currentNestIndex < 0)
        {
            currentNestIndex = respawnNests.Length - 1; // Wrap around to the last nest
        }
        Camera.main.GetComponent<CameraController>().respawnTarget = respawnNests[currentNestIndex].transform;
        respawnText.text = respawnNests[currentNestIndex].GetComponent<NestBrian>().nestName; // Update UI text with the current nest name
    }
    public void RespawnPlayer()
    {
        if (respawnNests.Length == 0)
        {
            Debug.LogWarning("No respawn nests available!");
            return; // No nests to respawn to
        }
        player.transform.position = respawnNests[currentNestIndex].transform.position; // Move player to the current nest position
        player.transform.rotation = respawnNests[currentNestIndex].transform.rotation; // Match the rotation of the nest
        player.GetComponent<PlayerHealth>().currentHealth = player.GetComponent<PlayerHealth>().maxHealth; // Reset player's health
        Camera.main.GetComponent<CameraController>().SwitchToPlayer(); // Update camera target to the new nest
        Destroy(gameObject); // Destroy the GameObject holding this script
    }
}
