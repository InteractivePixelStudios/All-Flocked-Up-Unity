using UnityEngine;

public class PoopManager : MonoBehaviour
{
    [Header ("Poop Settings")]
    [SerializeField] private int maxPoop = 5;
    [SerializeField] private int currentPoop = 0;
    [SerializeField] private float poopCooldown = 1.0f;


    private void LosePoop()
    {
        //Logic to handle "shooting" poop at poopable objects
    }

    private void RestorePoop()
    {
        //Logic to restore poop, ensure does not go past max poop count
    }

    private void AddAccessoryBonus()
    {
        //Primary or secondary bonus increases max poop count
    }

    private void IncreaseMaxPoop()
    {
        //logic to increase the maximum poop count
    }

    



}
