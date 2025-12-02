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
            wornObject = wearable;
            wearable.transform.SetParent(attachPoint.transform, false);
            wornObject.transform.position = attachPoint.transform.localPosition + offset;
            wornObject.transform.rotation = attachPoint.transform.localRotation* rotationOffset;
            wornObject.GetComponent<Rigidbody>().useGravity = false;
            wornObject.GetComponent<BoxCollider>().enabled = false;
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
            wornObject.transform.SetParent(null, true);
            wornObject.GetComponent<Rigidbody>().useGravity = true;
            wornObject.GetComponent<BoxCollider>().enabled = true;
            wornObject = null;
            RemoveStealth();
        }
    }

    protected void GiveStealth()
    {
        playerRef.stealthModifier = 2;
        playerRef.radiusModifier = 2f;
        playerRef.ToggleStealthOn();
    }

    protected void RemoveStealth()
    {
        playerRef.stealthModifier = 0;
        playerRef.radiusModifier = 0f;
        playerRef.ToggleStealthOff();
    }
}
