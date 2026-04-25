using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemButton : MonoBehaviour
{
    [SerializeField] private PlayerWingventory wingventory;
    private WingventoryCanvas wingUI;
    public Image itemImage;
    [SerializeField] private Button itemButton;
    public int itemCount;
    public TextMeshProUGUI itemQuantityText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    public string itemRef;
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

    public void SetWingUIRef(WingventoryCanvas UI)
    {
        wingUI = UI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWingRef(PlayerWingventory wing)
    {
        wingventory = wing;
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
        itemCount--;
        wingUI.RemoveItemFromInv(this);
        HideOptions();
    }

    private void DropItem()
    {
        wingventory.DropItem(itemRef);
        itemCount--;
        HideOptions();
        if (itemCount <= 0)
        {
            wingUI.RemoveItemFromInv(this);
            Destroy(this.gameObject);
        }

    }
}
