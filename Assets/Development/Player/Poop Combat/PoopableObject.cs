using UnityEngine;

public class PoopableObject : MonoBehaviour, IPoopable
{
    //Define damage(stun duration) on hit for poopable objects
    //We used an enum in unreal to define different types of poopable objects (garbage, NPC, car, bike)
    //Going to switch to interface for now, but we can change back if not needed

    [SerializeField] private PoopType objectType;

    public void OnPoopHit(PoopType type)
    {
        Debug.Log($"{objectType} was pooped on with {type.poopName}");
        //Trigger stun logic here
    }

}
