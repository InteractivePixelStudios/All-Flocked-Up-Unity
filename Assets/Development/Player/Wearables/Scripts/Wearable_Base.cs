using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Wearable_Base : MonoBehaviour
{
    public bool isGrabbed;
    [SerializeField] private Vector3 objectOffset;
    [SerializeField] protected Quaternion objectRotOffset;
    [SerializeField] private Quaternion objectRotation;
    [SerializeField] private GameObject wornObject;
    [SerializeField] private LayerMask wearableLayer;
    public GameObject attachPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private float forwadForce;
    [SerializeField] private float verticalForce;

    [SerializeField] private PlayerStealthSystem playerRef;

    private void Start()
    {
        playerRef = FindFirstObjectByType<PlayerStealthSystem>();
    }

    void Update()
    {
        if (wornObject != null)
        {
            UpdatePosition();
        }
        if (Input.GetKeyDown(KeyCode.R) && isGrabbed)
        {
            RemoveObject();
        }
    }

    public void LookForObject()
    {
        Debug.Log("Called");
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, grabDistance, wearableLayer))
        {
            wornObject = hit.collider.gameObject;
            if(wornObject != null )
            {
                SetOffset();
                GrabObject(hit.collider.gameObject,objectOffset,objectRotOffset);
            }
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


    public void RemoveObject()
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
        }
    }

    protected void GiveStealth()
    {
        playerRef.ToggleStealthOn();
    }

    protected void RemoveStealth()
    {
        playerRef.ToggleStealthOff();
    }
}
