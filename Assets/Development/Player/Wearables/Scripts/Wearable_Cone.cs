using UnityEngine;

public class Wearable_Cone : Wearable_Base
{
    [SerializeField] private GameObject wornObject;
    [SerializeField] private Vector3 objectOffset;
    [SerializeField] private Quaternion rotationOffset;

    public override void SetOffset()
    {
        wornObject = this.gameObject;
        var comp = GetComponent<WearableObject>();
        objectOffset = comp.offset;
        rotationOffset = comp.rotOffset;
        objectRotOffset = comp.rotOffset;
    }
}
