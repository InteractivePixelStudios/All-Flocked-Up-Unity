using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStealthSystem : MonoBehaviour
{
    [SerializeField] private int baseStealth;
    [SerializeField] private int maxStealth;
    [SerializeField] private int currentStealth;
    public int stealthModifier;
    public const int stealthModifierBase = 0;
    [SerializeField] private SphereCollider sphereRadius;
    public float radiusModifier;
    public const float radiusModifierBase = 8f;
    [SerializeField] private bool isStealthToggled;
    [SerializeField] private bool isTimedBonus;
    [SerializeField] private float currentTimer;
    public float maxTimer;
    public const float maxTimerBase = 30f;
    [SerializeField] InputAction crouchAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStealth = baseStealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimedBonus)
        {
            currentTimer-= Time.deltaTime;
            if(currentTimer < 0)
            {
                TimedStealthBonus(2,maxTimer);
                TimedRadiusBonus(2f,maxTimer);
            }
            else { isStealthToggled = false; }
        }
        {
            
        }
        if (!isStealthToggled)
        {
            GiveStealthAttribute(radiusModifier,stealthModifier);
        }
        else if (isStealthToggled) { RemoveStealthAttribute(radiusModifier, stealthModifier); }
        else return;
    }

    private void SetStealth(int modifier)
    {
        maxStealth = baseStealth + modifier;
        if (currentStealth!=maxStealth)
        {
            currentStealth = maxStealth;
        }
    }

    private void IncreaseModifier(int modifier)
    {
        stealthModifier += modifier;
        SetStealth(modifier);
    }

    private void DecreaseModifier(int modifier)
    {
        stealthModifier+= modifier;
        SetStealth(modifier);
    }

    private void GainStealth(int modifier)
    {
        maxStealth = baseStealth + modifier;
        currentStealth = maxStealth;
    }

    private void LoseStealth(int modifier)
    {
        maxStealth = baseStealth + modifier;
        currentStealth = maxStealth;
    }

    public void ToggleStealthOn()
    {
        isStealthToggled = true;
    }

    public void ToggleStealthOff()
    {
        isStealthToggled = false;
    }

    private void IncreaseRadius(float modifier)
    {
        if (sphereRadius.radius <= 10)
        {
            sphereRadius.radius += modifier;
        }
        else return;
    }

    private void DecreaseRadius(float modifier)
    {
        if (sphereRadius.radius >= 10)
        {
            sphereRadius.radius -= modifier;
        }
    }

    private void GiveStealthAttribute(float radModifier,int stealthMod)
    {
        IncreaseModifier(stealthMod);
        DecreaseRadius(radModifier);
        GainStealth(stealthMod);
    }

    private void RemoveStealthAttribute(float radModifier,int stealthMod)
    {
        DecreaseModifier(stealthMod);
        IncreaseRadius(radModifier);
        LoseStealth(stealthMod);
    }

    public void TimedStealthBonus(int bonus, float time)
    {
        currentTimer = maxTimer;
        isStealthToggled = true;
    }

    public void TimedRadiusBonus(float radiusBonus, float time)
    {
        currentTimer = maxTimer;
        isStealthToggled = true;
    }
}
