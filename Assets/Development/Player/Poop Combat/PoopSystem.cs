using UnityEngine;
using System.Collections;

public class PoopSystem : MonoBehaviour
{
    [Header ("Poop Settings")]
    [SerializeField] private int maxPoop = 5;
    [SerializeField] private float poopCooldown = 2.0f;

    private int currentPoop;
    private float cooldownTimer = 0f;

    public bool CanPoop => cooldownTimer <= 0f && currentPoop > 0;

    private void Awake()
    {
        currentPoop = maxPoop;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool TryPoop()
    {
        if (!CanPoop) return false;

        currentPoop--;
        cooldownTimer = poopCooldown;
        return true;
    }

    //Logic to restore poop, ensure does not go past max poop count
    private void RestorePoop(int amount) => currentPoop = Mathf.Min(currentPoop + amount, maxPoop);
    //logic to increase the maximum poop count
    private void IncreaseMaxPoop(int amount) => maxPoop += amount;

    private void AddAccessoryBonus()
    {
        //Primary or secondary bonus increases max poop count
    }

    


}
