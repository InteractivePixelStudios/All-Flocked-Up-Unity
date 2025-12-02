using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;


    [Header("Death Settings")]
    [SerializeField] private bool isDead = false;
    [SerializeField] private float deathDelay = 2f; // Delay before respawning or performing death actions

    [Header("Respawn Settings")]
    [SerializeField] private Canvas playerCanvas;
    [SerializeField] private Canvas respawnCanvasPrefab;
    [SerializeField] private Canvas respawnCanvasInstance;

    [Header("Components")]
    public Rigidbody rb;

    void Start()
    {
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth; // Initialize current health to max health if not set
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>(); // Get the Rigidbody component if not assigned
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(10); // For testing, press R to take 10 damage
            Debug.Log("Current Health: " + currentHealth);
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
        {
            return; // Cannot heal if the player is dead
        }
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health does not exceed max health
        }
    }

    public void TakeDamage(int Damage)
    {
        if (currentHealth <= 0)
        {
            return; // Player is already dead, no damage can be taken
        }
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(DelayBeforeDie(deathDelay)); // Delay before dying to allow for any last actions
        }
    }
    private System.Collections.IEnumerator DelayBeforeDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    private void Die()
    {
        isDead = true;
        // Add any additional death logic here, such as playing a death animation or sound
        Debug.Log("Player has died.");

        // Switch camera to top-down view
        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null)
        {
            camController.SwitchToRespawnCam();
        }

        // Spawn respawnCanvasPrefab as a child of playerCanvas and assign it to respawnCanvasInstance
        if (respawnCanvasPrefab != null && respawnCanvasInstance == null)
        {
            respawnCanvasInstance = Instantiate(respawnCanvasPrefab, playerCanvas.transform);
        }
    }
}
