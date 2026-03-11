using System.Collections.Generic;
using UnityEngine;

 public enum EAccessoryItems { BottleCap, Monocle, Feather, Anklet, Bread, Rose, Reciept }

public class AccessoryBase : MonoBehaviour
{

    public Transform accessoryTransform;
    [SerializeField] protected string accessoryName;
    [SerializeField] protected string accessoryDescription;
    [SerializeField] protected Vector3 accessoryOffset;
    public EAccessoryItems itemState = EAccessoryItems.BottleCap;
    public bool isEquip;
    [SerializeField] protected MeshFilter itemMesh;
    MeshRenderer itemMaterial;

    [SerializeField] protected Dictionary<GameObject, bool> accessoryList = new();
    [SerializeField] private GameObject currentItem;

    [SerializeField] protected List<Mesh> meshList;
    [SerializeField] protected List<Material> materialList;
    public int poopStatBonus;
    public int staminaStatBonus;
    public int healthStatBonus;
    [SerializeField] private PlayerStealthSystem stealthComponent;
    [SerializeField] private int stealthBonus;
    [SerializeField] private float stealthRadiusBonus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemMesh = GetComponent<MeshFilter>();
        itemMaterial = GetComponent<MeshRenderer>();
        stealthComponent = FindFirstObjectByType<PlayerStealthSystem>();
        SetItemState();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetName()
    {
        return accessoryName;
    }

    public string GetDescription()
    {
        return accessoryDescription;
    }


    protected void SetItemState()
    {
        switch (itemState)
        {
            
            case EAccessoryItems.BottleCap:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                itemMesh.transform.rotation = Quaternion.Euler(-90, 0, 0);
                itemMesh.transform.localScale = new Vector3(0.31f, 0.31f, 0.31f);
                itemMaterial.material = materialList[0];
                accessoryName = "Bottle Cap Hat";
                accessoryDescription = "Bottle Cap Hat";
                break;
            case EAccessoryItems.Monocle:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[1];
                itemMesh.transform.rotation = Quaternion.Euler(-95, 0, 0);
                itemMesh.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                itemMaterial.material = materialList[1];
                accessoryName = "Monocle";
                accessoryDescription = "Monocle";
                break;
            case EAccessoryItems.Feather:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[2];
                itemMesh.transform.rotation = Quaternion.Euler(-97, 0, 0);
                itemMesh.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
                itemMaterial.material = materialList[2];
                accessoryName = "Feather";
                accessoryDescription = "Feather";
                break;
            case EAccessoryItems.Anklet:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[3];
                itemMesh.transform.rotation = Quaternion.Euler(-85, 0, 0);
                itemMesh.transform.localScale = new Vector3(0.48f, 0.48f, 0.48f);
                itemMaterial.material = materialList[3];
                accessoryName = "Anklet";
                accessoryDescription = "Anklet";
                break;
            case EAccessoryItems.Bread:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[4];
                itemMesh.transform.rotation = Quaternion.Euler(-95, 0, 0);
                itemMesh.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                itemMaterial.material = materialList[4];
                accessoryName = "Bread Slice Necklace";
                accessoryDescription = "Bread";
                break;
            case EAccessoryItems.Rose:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[5];
                itemMesh.transform.rotation = Quaternion.Euler(-96, 0, 0);
                itemMesh.transform.localScale = new Vector3(0.51f, 0.51f, 0.51f);
                itemMaterial.material = materialList[5];
                accessoryName = "Rosebud Hat";
                accessoryDescription = "Rose";
                break;
            case EAccessoryItems.Reciept:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[6];
                itemMesh.transform.rotation = Quaternion.Euler(-90, 0, 0);
                itemMesh.transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
                itemMaterial.material = materialList[6];
                accessoryName = "Reciept Scarf";
                accessoryDescription = "Reciept";
                break;
        }
    }

    private void SetTransform()
    {
        transform.localPosition = accessoryTransform.position;
    }

    private void GiveStealthBonus()
    {
        stealthComponent.stealthModifier = 2;
        stealthComponent.radiusModifier = 2f;
        stealthComponent.ToggleStealthOn();
    }

    private void RemoveStealthBonus()
    {
        stealthComponent.stealthModifier = 0;
        stealthComponent.radiusModifier = 0f;
        stealthComponent.ToggleStealthOff();
    }



}
