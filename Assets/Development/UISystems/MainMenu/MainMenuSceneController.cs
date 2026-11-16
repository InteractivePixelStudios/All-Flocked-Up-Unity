using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuSceneController : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private PoopSystem playerPoop;

    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerPoop = FindAnyObjectByType<PoopSystem>();
        canvasController.OpenLanguageSelect();
    }

    void Update()
    {
        //if (playerPoop.TryPoop()) playerPoop.GainPoop(1);
    }
}
