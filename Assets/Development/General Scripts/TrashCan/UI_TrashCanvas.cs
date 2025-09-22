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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(trashText ==null) InitCanvas();
    }

    //sets text and button listeners
    public void InitCanvas()
    {
        trashText = GetComponent<TextMeshProUGUI>();
        reward1Button.onClick.AddListener(trashCanInstance.GiveRewardOne);
        reward2Button.onClick.AddListener(trashCanInstance.GiveRewardTwo);
    }
    //call to destroy canvas
    public void DestroyCanvas()
    {
        Destroy(gameObject);
    }

    
}
