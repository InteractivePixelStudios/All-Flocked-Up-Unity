using System.Drawing;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHoldObject : MonoBehaviour
{
    [SerializeField] private GameObject grabPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private LayerMask consumeLayer;
    [SerializeField] private LayerMask collectLayer;
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

        grabAction = playerInput.actions.FindAction("Interact");

        if(grabAction != null )
        {

                grabAction.started += OnGrabPressed;


            
            //grabAction.started += CallHold;

        }
    }

    private void Update()
    {
        if( isHoldingObject )
        {
            HoldGrabbedObject(grabbedObject, grabOffset);
        }
    }

    void OnGrabPressed(InputAction.CallbackContext ctx)
    {
        if (!isHoldingObject)
        {
            peckComp.Peck();
            TryGrabObject();
        }
        else
        {
            ReleaseGrabbedObject();
        }
    }




    private void TryGrabObject()
    {

        RaycastHit hit;
        if(Physics.Raycast(grabPoint.transform.position, transform.forward + (Vector3.down*2), out hit, grabDistance, grabLayer))
        {
            grabbedObject = hit.collider.gameObject;
            PickUpObject(grabbedObject);

        }


    }

    private void PickUpObject(GameObject obj)
    {
        Vector3 offset = new();
        try
        {
            offset = obj.GetComponent<Interactable>().offset;

        }
        catch
        {
            offset = new Vector3(0, 0, 0);
        }
        finally
        {

            obj.transform.position = grabPoint.transform.localPosition+ offset;
            obj.transform.rotation = grabPoint.transform.localRotation;
            grabOffset = offset;
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.transform.SetParent(grabPoint.transform, false);
            obj.GetComponent<BoxCollider>().enabled = false;
            isHoldingObject = true;
            obj.GetComponent<Interactable>().ToggleVFXOn();
            HoldGrabbedObject(obj, offset);
        }

    }


    private void HoldGrabbedObject(GameObject obj, Vector3 offset)
    {
        if(grabbedObject != null)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = grabPoint.transform.rotation;

        }
    }



    private void ReleaseGrabbedObject()
    {
        isHoldingObject = false;
        grabbedObject.transform.SetParent(null, true);
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<BoxCollider>().enabled = true;
        grabbedObject.GetComponent<Interactable>().ToggleVFXOff();
        grabbedObject = null;

    }
}
