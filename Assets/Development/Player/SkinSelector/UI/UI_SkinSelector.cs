using UnityEngine;
using UnityEngine.UI;

public class UI_SkinSelector : MonoBehaviour
{

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private PlayerSkinSelector playerComp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerComp = FindFirstObjectByType<PlayerSkinSelector>();
        previousButton.onClick.AddListener(PreviousSkin);
        nextButton.onClick.AddListener(NextSkin);
        confirmButton.onClick.AddListener(SetSkinToPlayer);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    void PreviousSkin()
    {
        playerComp.PrevSkin();
    }

    void NextSkin()
    {
        playerComp.NextSkin();
    }

    void SetSkinToPlayer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerComp.ConfirmSelection();
        playerComp.DestroyBackdrop();
        Destroy(this.gameObject);
    }
}
