using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wearable_Base : MonoBehaviour
{
    public bool isGrabbed;
    public bool isRemoving;
     private Vector3 objectOffset;
    [SerializeField] protected Quaternion objectRotOffset;
    [SerializeField] private Quaternion objectRotation;
     private GameObject wornObject;
    [SerializeField] private LayerMask wearableLayer;
    public GameObject attachPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private float forwadForce;
    [SerializeField] private float verticalForce;

    [SerializeField] private GameObject playerRef;
    PlayerInput input;
    InputAction removeAction;


    private void Start()
    {
        playerRef = FindAnyObjectByType<PlayerStealthSystem>().gameObject;
    }

    void Update()
    {
        if (wornObject != null)
        {
            UpdatePosition();
        }

    }

    public void LookForObject(RaycastHit hit)
    {
        Debug.Log("Called");
            wornObject = hit.collider.gameObject;
            if(wornObject != null )
            {
                SetOffset();
                GrabObject(hit.collider.gameObject,objectOffset,objectRotOffset);
            }
        
    }

    public virtual void SetOffset()
    {
        Debug.Log("OffsetSet");
        var comp = wornObject.GetComponent<WearableObject>();
        objectOffset = comp.offset;
    }

    protected void GrabObject(GameObject wearable, Vector3 offset,Quaternion rotationOffset)
    {
        if(wearable != null)
        {
            isGrabbed = true;
            wearable.transform.position = attachPoint.transform.position + offset;
            wearable.transform.rotation = attachPoint.transform.rotation* rotationOffset;
            wearable.transform.SetParent(attachPoint.transform, false);
            wearable.GetComponent<Rigidbody>().useGravity = false;
            wearable.GetComponent<BoxCollider>().enabled = false;
            wornObject = wearable;
            Debug.Log("Grabbed");
            GiveStealth();
            input = playerRef.GetComponent<PlayerInput>();
            removeAction = input.currentActionMap.FindAction("Interact");
            if(removeAction != null)
            {
                removeAction.performed += RemoveObject;
            }

        }
    }

    protected void UpdatePosition()
    {
        if(wornObject != null)
        {
            wornObject.transform.localPosition = Vector3.zero;
            wornObject.transform.localRotation = Quaternion.identity * objectRotOffset;
        }
    }


    public void RemoveObject(InputAction.CallbackContext ctx)
    {
        if(wornObject != null)
        {
            isGrabbed = false;
            wornObject.transform.position += new Vector3(0, 1, 0);
            wornObject.transform.SetParent(null, true);
            var rb = wornObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            wornObject.GetComponent<BoxCollider>().enabled = true;
            rb.linearVelocity = new Vector3(forwadForce,verticalForce, 0).normalized;
            wornObject = null;
            RemoveStealth();
            Debug.Log("RemovingObject");
            removeAction.performed -= RemoveObject;
        }
    }

    protected void GiveStealth()
    {
        playerRef.GetComponent<PlayerStealthSystem>().ToggleStealthOn();
        playerRef.GetComponent<PlayerFlightMovement>().enabled = false;
    }

    protected void RemoveStealth()
    {
        playerRef.GetComponent<PlayerStealthSystem>().ToggleStealthOff();
        playerRef.GetComponent<PlayerFlightMovement>().enabled = true;
    }
}
