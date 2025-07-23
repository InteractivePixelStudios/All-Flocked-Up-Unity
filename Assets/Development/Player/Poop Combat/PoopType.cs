using UnityEngine;

[CreateAssetMenu(fileName = "PoopType", menuName = "Scriptable Objects/PoopType")]
public class PoopType : ScriptableObject
{
    public string poopName;
    public float stunDuration = 2f; //adjust as needed
    
    //Visual & Audio - feel free to change names as needed
    public GameObject poopVisual;
    public GameObject splatVFX;
    public AudioClip splatSFX;

}
