using UnityEngine;

public class Perchable_HideSpot : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private PerchableObject_Tree treeRef;
    [SerializeField] private bool readyToHide;

    void Start()
    {
        treeRef = GetComponentInParent<PerchableObject_Tree>();
    }

    void Update()
    {
        treeRef.isHiding = readyToHide;
        if(readyToHide && playerRef.GetComponent<PlayerInteraction>().ReturnInteractPerformed())
        {
            treeRef.StartPerch();
            playerRef.GetComponentInChildren<IconToggle>().HideIcon();
        }
    }
    void OnCollisionEnter(Collision hideColliders)
    {
        if (hideColliders.gameObject.CompareTag("Player"))
        {
            playerRef = hideColliders.gameObject;
            readyToHide = true;
            playerRef.GetComponentInChildren<IconToggle>().ShowIcon();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (readyToHide)
        {
            readyToHide = false;
            playerRef = null;
        }
        else return;
    }

}
