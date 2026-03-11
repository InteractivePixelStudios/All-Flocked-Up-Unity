using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_SkinSelector : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private PlayerSkinSelector playerComp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = FindFirstObjectByType<PlayerInput>();
        input.SwitchCurrentActionMap("UI");
        playerComp = FindFirstObjectByType<PlayerSkinSelector>();
        playerComp.gameObject.GetComponent<PlayerGroundMovement>().enabled = false;
        previousButton.onClick.AddListener(PreviousSkin);
        nextButton.onClick.AddListener(NextSkin);
        confirmButton.onClick.AddListener(SetSkinToPlayer);
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Confined;
        //EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
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
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        playerComp.ConfirmSelection();
        playerComp.DestroyBackdrop();
        input.SwitchCurrentActionMap("Player");
        playerComp.gameObject.GetComponent<PlayerGroundMovement>().enabled = true;
        Destroy(this.gameObject);
    }
}
