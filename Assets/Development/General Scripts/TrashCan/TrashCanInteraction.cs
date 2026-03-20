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
    private Q_SearchTrash questComp;

    [SerializeField] private int regenAmt;
    [SerializeField] private int poopRegen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        TryGetComponent<Q_SearchTrash>(out questComp);
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
            canvasController.ShowTrashPrompt(this);
            Debug.Log("interacted");
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

    public void ShowPlayer()
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
            FillPlayerPoop();
            //CloseUI();

            if (questComp != null)
            {
                questComp.SearchTrash();
            }
        }
    }

    public void GiveRewardTwo()
    {
        if (playerRef != null)
        {
            looted = true;
            ToggleParticles(looted);
            FillPlayerStats();
           // CloseUI();

            if (questComp != null)
            {
                questComp.SearchTrash();
            }
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
        playerRef.GetComponent<StaminaSystem>().RegenStamina() ;
    }

    private void FillPlayerPoop()
    {
        playerRef.GetComponent<PlayerHealth>().Heal(regenAmt);
        playerRef.GetComponent<PoopSystem>().GainPoop(poopRegen);
    }

    private void CloseUI()
    {
        if (this != null)
        {
            canvasController.CloseTrashPrompt();
        }
    }

    
}
