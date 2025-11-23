using System.Collections.Generic;
using UnityEngine;

 public enum EAccessoryItems { BottleCap, Monocle, Feather, Anklet, Bread, Rose, Reciept }

public class AccessoryBase : MonoBehaviour
{

    public Transform accessoryTransform;
    [SerializeField] protected string accessoryName;
    [SerializeField] protected string accessoryDescription;
    [SerializeField] protected Vector3 accessoryOffset;
    public EAccessoryItems itemState;
    [SerializeField] protected MeshFilter itemMesh;

    [SerializeField] protected Dictionary<GameObject, bool> accessoryList = new();
    [SerializeField] private GameObject currentItem;

    [SerializeField] protected List<Mesh> meshList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemMesh = GetComponent<MeshFilter>();
        SetItemState();
    }

    // Update is called once per frame
    void Update()
    {

    }


    protected void SetItemState()
    {
        switch (itemState)
        {
            
            case EAccessoryItems.BottleCap:
                SetTransform();
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Bottle Cap Hat";
                accessoryDescription = "Bottle Cap Hat";
                break;
            case EAccessoryItems.Monocle:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Monocle";
                accessoryDescription = "Monocle";
                break;
            case EAccessoryItems.Feather:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Feather";
                accessoryDescription = "Feather";
                break;
            case EAccessoryItems.Anklet:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Anklet";
                accessoryDescription = "Anklet";
                break;
            case EAccessoryItems.Bread:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Bread Slice Necklace";
                accessoryDescription = "Bread";
                break;
            case EAccessoryItems.Rose:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Rosebud Hat";
                accessoryDescription = "Rose";
                break;
            case EAccessoryItems.Reciept:
                accessoryOffset = new Vector3(0, 0, 0);
                itemMesh.mesh = meshList[0];
                accessoryName = "Reciept Scarf";
                accessoryDescription = "Reciept";
                break;
        }
    }

    private void SetTransform()
    {
        transform.localPosition = accessoryTransform.position;
    }



}
