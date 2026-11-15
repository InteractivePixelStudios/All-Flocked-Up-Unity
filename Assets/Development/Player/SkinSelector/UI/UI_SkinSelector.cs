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
    UI_NestMenu nestMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nestMenu = GetComponentInParent<UI_NestMenu>();
        input = FindAnyObjectByType<PlayerInput>();
        playerComp = input.GetComponent<PlayerSkinSelector>();
        previousButton.onClick.AddListener(PreviousSkin);
        nextButton.onClick.AddListener(NextSkin);
        confirmButton.onClick.AddListener(SetSkinToPlayer);
        EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
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
        nestMenu.CloseSkinSelector();
    }
}
