using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHoldObject : MonoBehaviour
{
    [SerializeField] private GameObject grabPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private Vector3 grabOffset;
    [SerializeField] private bool isHoldingObject = false;
    [SerializeField] private PlayerPeckComponent peckComp;

    InputAction grabAction;
    PlayerInput playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        peckComp = GetComponent<PlayerPeckComponent>();
        playerInput = GetComponentInParent<PlayerInput>();

        grabAction = playerInput.actions.FindAction("Grab");

        if(grabAction != null )
        {
            grabAction.performed += CallGrab;
            grabAction.started += CallHold;
            grabAction.canceled += CallRelease;

        }
    }

    void GrabObject(InputAction.CallbackContext ctx)
    {
        if (!isHoldingObject)
        {
            peckComp.Peck();
            TryGrabObject();

        }
        else if (isHoldingObject)
        {

            ReleaseGrabbedObject();
        }
        else 
        {
            HoldGrabbedObject(grabbedObject, grabOffset);

        }
        
    }

    private void CallGrab(InputAction.CallbackContext ctx)
    {
        if (!isHoldingObject)
        {
            peckComp.Peck();
            TryGrabObject();

        }
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

    private void CallHold(InputAction.CallbackContext ctx)
    {
        HoldGrabbedObject(grabbedObject, grabOffset);
    }

    private void HoldGrabbedObject(GameObject Object, Vector3 offset)
    {
        if(grabbedObject != null)
        {
            Object.transform.localPosition = Vector3.zero;
            Object.transform.rotation = grabPoint.transform.rotation;
            Object.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void CallRelease(InputAction.CallbackContext ctx)
    {
        if (isHoldingObject)
        {

            ReleaseGrabbedObject();
        }
    }

    private void ReleaseGrabbedObject()
    {
        isHoldingObject = false;
        grabbedObject.transform.SetParent(null, true);
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabbedObject.GetComponent<BoxCollider>().enabled = true;
        grabbedObject = null;

    }
}
