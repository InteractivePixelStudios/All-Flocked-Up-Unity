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

    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerAccessoryComponent accessoryComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        accessoryComponent = FindFirstObjectByType<PlayerAccessoryComponent>();
        closeButton.onClick.AddListener(CloseAccessoryPanel);
        bottleCapButton.onClick.AddListener(EquipBottleCap);
        monocleButton.onClick.AddListener(EquipMonocle);
        featherButton.onClick.AddListener(EquipFeather);
        ankletButton.onClick.AddListener(EquipAnklet);
        breadButton.onClick.AddListener(EquipBread);
        roseButton.onClick.AddListener(EquipRose);
        recieptButton.onClick.AddListener(EquipReciept);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CloseAccessoryPanel()
    {
        Destroy(this.gameObject);
    }

    private void EquipBottleCap()
    {
        accessoryComponent.list = AccessoryList.BottleCap;
        accessoryComponent.GetAndEquipAccessory();
    }

    private void EquipMonocle()
    {
        accessoryComponent.list = AccessoryList.Monocle;
        accessoryComponent.GetAndEquipAccessory();
    }

    private void EquipFeather()
    {
        accessoryComponent.list = AccessoryList.Feather;
        accessoryComponent.GetAndEquipAccessory();
    }

    private void EquipAnklet()
    {
        accessoryComponent.list = AccessoryList.Anklet;
        accessoryComponent.GetAndEquipAccessory();
    }

    private void EquipBread()
    {
        accessoryComponent.list = AccessoryList.Bread;
        accessoryComponent.GetAndEquipAccessory();
    }

    private void EquipRose()
    {
        accessoryComponent.list = AccessoryList.Rose;
        accessoryComponent.GetAndEquipAccessory();
    }

    private void EquipReciept()
    {
        accessoryComponent.list = AccessoryList.Reciept;
        accessoryComponent.GetAndEquipAccessory();
    }
}
