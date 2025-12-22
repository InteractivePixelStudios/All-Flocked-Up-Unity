using System.Collections;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Stats")]
    [SerializeField] float currentStamina;
    [SerializeField] float MaxStamina = 10;
    [SerializeField] float staminaRegenSpeed = .5f;

    bool regenStamina = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = MaxStamina;   
    }

    // Update is called once per frame
    void Update()
    {
        if (regenStamina)
        {
            currentStamina += staminaRegenSpeed * Time.deltaTime;
            if (currentStamina >= MaxStamina)
            {
                currentStamina = MaxStamina;
                regenStamina = false;
            }
        }
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina - amount >= 0)
        {
            CancelRegen();
            currentStamina -= amount;
            return true;
        }
        else
            return false;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void RegenStamina()
    {
        regenStamina = true;
    }

    public void CancelRegen()
    {
        regenStamina = false;
    }
}
