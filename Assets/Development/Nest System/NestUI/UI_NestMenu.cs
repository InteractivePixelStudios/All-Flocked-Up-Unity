using UnityEngine;
using UnityEngine.UI;

public class UI_NestMenu : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    public UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeButton.onClick.AddListener(CloseNestMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CloseNestMenu()
    {
        canvasController.CloseNestMenu();
        Destroy(this.gameObject);
    }


}
