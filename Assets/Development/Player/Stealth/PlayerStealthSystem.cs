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
    [SerializeField] private bool isActivated;
    [SerializeField] private bool isTimedBonus;
    [SerializeField] private float currentTimer;
    public float maxTimer;
    public const float maxTimerBase = 30f;
    [SerializeField] InputAction crouchAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentStealth = baseStealth;
    }

    public int GetStealth()
    {
        return currentStealth;
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
        if (isStealthToggled && !isActivated)
        {
            isActivated = true;
            GiveStealthAttribute(radiusModifier,stealthModifier);
            Debug.Log("Toggled");
        }
        else if (!isStealthToggled && isActivated) { isActivated = false; RemoveStealthAttribute(radiusModifier, stealthModifier); Debug.Log("ToggleOFF"); }
        else return;
    }

    private void SetStealth(int modifier)
    {
        maxStealth = baseStealth + modifier;
        if (currentStealth != maxStealth)
        {
            currentStealth = maxStealth;
        }
        else return;
    }

    private void IncreaseModifier(int modifier)
    {
        Debug.Log("IncModCalled");
        if (stealthModifier != currentStealth)
        {
            Debug.Log(stealthModifier + "+" + modifier + "+" + currentStealth );
            stealthModifier += modifier;
            SetStealth(stealthModifier);
        }
        else return;

    }

    private void DecreaseModifier(int modifier)
    {
        Debug.Log("DecModCalled");
        if (stealthModifier == modifier)
        {
            Debug.Log(stealthModifier + "+" + modifier + "+" + currentStealth);
            stealthModifier -= modifier;
            SetStealth(stealthModifier);
        }
        else return;

    }

    private void GainStealth(int modifier)
    {
        maxStealth = baseStealth + modifier;
        currentStealth = maxStealth;
    }

    private void LoseStealth(int modifier)
    {
        maxStealth -= modifier;
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
        if (isActivated)
        {
            GainStealth(stealthMod);
            //IncreaseModifier(stealthMod);
            IncreaseRadius(radModifier);
            Debug.Log("GiveCalled");
        }


    }

    private void RemoveStealthAttribute(float radModifier,int stealthMod)
    {
        if (!isActivated)
        {
            LoseStealth(stealthMod);
            //DecreaseModifier(stealthMod);
            DecreaseRadius(radModifier);
            Debug.Log("RemoveCalled");
        }

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
