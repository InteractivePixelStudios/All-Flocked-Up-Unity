using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TrashCanvas : MonoBehaviour
{
    [SerializeField] private GameObject trashCanvas;
    [SerializeField] private TextMeshProUGUI trashText;
    [SerializeField] private Button reward1Button;
    [SerializeField] private Button reward2Button;
    [SerializeField] private TrashCanInteraction trashCanInstance;
    [SerializeField]private UI_CanvasController canvasController;
    public void SetCanvasReference(UI_CanvasController canvas)
    {
        canvasController = canvas;
    }

    public void SetTrashInstance(TrashCanInteraction trashCan)
    {
        trashCanInstance = trashCan;
    }
}
