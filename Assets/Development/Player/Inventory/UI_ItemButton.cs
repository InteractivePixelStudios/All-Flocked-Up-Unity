using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemButton : MonoBehaviour
{
    [SerializeField] private PlayerWingventory wingventory;
    public Image itemImage;
    [SerializeField] private Button itemButton;
    public TextMeshProUGUI itemQuantityText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    public GameObject itemRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemButton = GetComponent<Button>();
        itemButton.onClick.AddListener(ShowOptions);
        itemImage = GetComponentInChildren<Image>();
        itemQuantityText = GetComponentInChildren<TextMeshProUGUI>();
        useButton.onClick.AddListener(UseConsumeItem);
        dropButton.onClick.AddListener(DropItem);
        HideOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowOptions()
    {
        useButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(true);

    }

    private void HideOptions()
    {
        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);

    }

    private void UseConsumeItem()
    {
        wingventory.UseConsumeItem(itemRef);
        HideOptions();
    }

    private void DropItem()
    {
        wingventory.DropItem(itemRef);
        HideOptions() ;
    }
}
