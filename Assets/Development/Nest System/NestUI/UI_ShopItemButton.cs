using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopItemButton : MonoBehaviour
{
    public int cost;
    private TextMeshProUGUI costText;
    private Image itemImage;
    public Sprite itemSprite;
    public Button itemButton  => GetComponent<Button>();

    private void Start()
    {
        costText.SetText(cost.ToString());
        itemImage.sprite = itemSprite;
    }
}
