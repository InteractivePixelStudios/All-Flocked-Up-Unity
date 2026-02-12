using UnityEngine;

public class MainMenuSceneController : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private PoopSystem playerPoop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        playerPoop = FindFirstObjectByType<PoopSystem>();
        canvasController.OpenLanguageSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPoop.TryPoop())
            playerPoop.GainPoop(1);
    }
}
