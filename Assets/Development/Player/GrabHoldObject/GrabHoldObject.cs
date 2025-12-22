using System.Drawing;
using UnityEngine;

public class GrabHoldObject : MonoBehaviour
{
    [SerializeField] private GameObject grabPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private Vector3 grabOffset;
    [SerializeField] private bool isHoldingObject = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isHoldingObject)
        {
            TryGrabObject();

        }
        else if (Input.GetKeyDown(KeyCode.G) && isHoldingObject)
        {

            ReleaseGrabbedObject();
        }
        else if (isHoldingObject)
        {
            HoldGrabbedObject(grabbedObject, grabOffset);

        }
        else return;
    }

    private void TryGrabObject()
    {

        RaycastHit hit;
        if(Physics.Raycast(grabPoint.transform.position, transform.forward, out hit, grabDistance, grabLayer))
        {
            grabbedObject = hit.collider.gameObject;
            PickUpObject(grabbedObject);

        }
    }

    private void PickUpObject(GameObject Object)
    {
        Vector3 offset = new();
        try
        {
            offset = Object.GetComponent<Interactable>().offset;

        }
        catch
        {
            offset = new Vector3(0, 0, 0);
        }
        finally
        {

            Object.transform.position = grabPoint.transform.localPosition+ offset;
            Object.transform.rotation = grabPoint.transform.localRotation;
            grabOffset = offset;
            Object.GetComponent<Rigidbody>().useGravity = false;
            Object.transform.SetParent(grabPoint.transform, false);
            grabbedObject.GetComponent<BoxCollider>().enabled = false;
            isHoldingObject = true;
            HoldGrabbedObject(Object, offset);
        }

    }

    private void HoldGrabbedObject(GameObject Object, Vector3 offset)
    {
        if(grabbedObject != null)
        {
            Object.transform.localPosition = Vector3.zero;
            Object.transform.rotation = grabPoint.transform.rotation;
        }
    }

    private void ReleaseGrabbedObject()
    {
        isHoldingObject = false;
        grabbedObject.transform.SetParent(null, true);
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<BoxCollider>().enabled = true;
        grabbedObject = null;

    }
}
