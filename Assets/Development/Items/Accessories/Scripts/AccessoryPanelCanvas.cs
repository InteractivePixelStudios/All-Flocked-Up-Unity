using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryPanelCanvas : MonoBehaviour
{
    [SerializeField] private Button bottleCapButton;
    [SerializeField] private Button monocleButton;
    [SerializeField] private Button featherButton;
    [SerializeField] private Button ankletButton;
    [SerializeField] private Button breadButton;
    [SerializeField] private Button roseButton;
    [SerializeField] private Button recieptButton;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;

    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerAccessoryComponent accessoryComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        accessoryComponent = FindAnyObjectByType<PlayerAccessoryComponent>();
        bottleCapButton.onClick.AddListener(EquipBottleCap);
        monocleButton.onClick.AddListener(EquipMonocle);
        featherButton.onClick.AddListener(EquipFeather);
        ankletButton.onClick.AddListener(EquipAnklet);
        breadButton.onClick.AddListener(EquipBread);
        roseButton.onClick.AddListener(EquipRose);
        recieptButton.onClick.AddListener(EquipReciept);
        HideText();
    }

    void HideText()
    {
        nameText.gameObject.SetActive(false);
        descText.gameObject.SetActive(false);
    }

    void UpdateAndShowText(string name, string desc)
    {
        nameText.gameObject.SetActive(true);
        descText.gameObject.SetActive(true);
        nameText.SetText(name);
        descText.SetText(desc);
        Debug.Log(name + " + " + desc);
    }

    private void CloseAccessoryPanel()
    {
        Destroy(this.gameObject);
    }

    private void EquipBottleCap()
    {
        accessoryComponent.list = AccessoryList.BottleCap;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.SendItemName(), accessoryComponent.SendItemDesc());
    }

    private void EquipMonocle()
    {
        accessoryComponent.list = AccessoryList.Monocle;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.currentItem.GetName(), accessoryComponent.currentItem.GetDescription());
    }

    private void EquipFeather()
    {
        accessoryComponent.list = AccessoryList.Feather;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.currentItem.GetName(), accessoryComponent.currentItem.GetDescription());
    }

    private void EquipAnklet()
    {
        accessoryComponent.list = AccessoryList.Anklet;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.currentItem.GetName(), accessoryComponent.currentItem.GetDescription());
    }

    private void EquipBread()
    {
        accessoryComponent.list = AccessoryList.Bread;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.currentItem.GetName(), accessoryComponent.currentItem.GetDescription());
    }

    private void EquipRose()
    {
        accessoryComponent.list = AccessoryList.Rose;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.currentItem.GetName(), accessoryComponent.currentItem.GetDescription());
    }

    private void EquipReciept()
    {
        accessoryComponent.list = AccessoryList.Reciept;
        accessoryComponent.GetAndEquipAccessory();
        UpdateAndShowText(accessoryComponent.currentItem.GetName(), accessoryComponent.currentItem.GetDescription());
    }
}
