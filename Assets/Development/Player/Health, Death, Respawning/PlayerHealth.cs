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
    private RagdollController ragdoll;
    [SerializeField]UI_CanvasController canvasController;

    void Start()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        ragdoll = GetComponent<RagdollController>();
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>(); 
        }
    }

    void Update()
    {


    }

    public void Heal(int amount)
    {
        if (isDead)
        {
            return; 
        }
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int Damage)
    {
        if (currentHealth <= 0)
        {
            return; 
        }
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(DelayBeforeDie(deathDelay));
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
        ragdoll.ToggleRagdollOn();
        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null)
        {
            camController.SwitchToRespawnCam();
        }

        canvasController.OpenRespawn();
    }
}
