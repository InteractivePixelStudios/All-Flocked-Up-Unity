using UnityEngine;

public class Perchable_HideSpot : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private PerchableObject_Tree treeRef;
    [SerializeField] private bool readyToHide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        treeRef = GetComponentInParent<PerchableObject_Tree>();
    }

    // Update is called once per frame
    void Update()
    {
        treeRef.isHiding = readyToHide;
        if(readyToHide && Input.GetKeyUp(KeyCode.E))
        {
            treeRef.StartPerch();
        }
    }
    void OnCollisionEnter(Collision hideColliders)
    {
        if (hideColliders.gameObject.CompareTag("Player"))
        {
            playerRef = hideColliders.gameObject;
            readyToHide = true;
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
