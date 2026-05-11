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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //InitCanvas();
    }

    public void SetCanvasReference(UI_CanvasController canvas)
    {
        canvasController = canvas;
    }

    public void SetTrashInstance(TrashCanInteraction trashCan)
    {
        trashCanInstance = trashCan;
    }

    //sets text and button listeners
    //public void InitCanvas()
    //{
    //    trashText = GetComponent<TextMeshProUGUI>();
    //    reward1Button.onClick.AddListener(GiveRewardOne);
    //    reward2Button.onClick.AddListener(GiveRewardTwo);
    //}
    ////call to destroy canvas
    //public void GiveRewardOne()
    //{

    //    //trashCanInstance.ShowPlayer();
    //    canvasController.CloseTrashPrompt();
    //}

    //public void GiveRewardTwo()
    //{

    //    //trashCanInstance.ShowPlayer();
    //    canvasController.CloseTrashPrompt();
    //}

    
}
