using UnityEngine;

enum CanState {Full, InUse, Empty}

public class TrashCanInteraction : MonoBehaviour
{
    [SerializeField] private GameObject trashCanObject;
    [SerializeField] private bool inRange;
    [SerializeField] private GameObject playerRef;
    [SerializeField] private bool looted;
    [SerializeField] private ParticleSystem trashParticles;
    [SerializeField] private UI_CanvasController canvasController;

    [SerializeField] private int regenAmt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractWithTrashCan()
    {
        if (inRange)
        {
            HidePlayer();
            canvasController.ShowTrashPrompt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerRef = null;
        inRange = false;
    }

    private void HidePlayer()
    {
        if (playerRef != null)
        {
            playerRef.SetActive(false);
        }
    }

    private void ShowPlayer()
    {
        if (playerRef != null)
        {
            playerRef.SetActive(true);
        }
    }

    public void GiveRewardOne()
    {
        if (playerRef != null)
        {
            looted = true;
            ToggleParticles(looted);
            FillPlayerStats();
        }
    }

    public void GiveRewardTwo()
    {
        if (playerRef != null)
        {
            looted = true;
            ToggleParticles(looted);
            FillPlayerStats();
        }
    }

    private void ResetCan()
    {
        if (looted)
        {
            looted = false;
            ToggleParticles(looted);
        }
    }

    private void ToggleParticles(bool used)
    {
        if (used)
        {
            trashParticles.Stop();
        }
        else trashParticles.Play();
    }

    private void FillPlayerStats()
    {
        playerRef.GetComponent<PlayerHealth>().Heal(regenAmt);
    }

    
}
