using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private float stamina;
    [SerializeField] private float baseStamina;
    [SerializeField] private int level;
    [SerializeField] private int currentLevel;
    [SerializeField] private int trinkets;
    [SerializeField] private float stealth;
    [SerializeField] private int poop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerRef = FindFirstObjectByType<PlayerGroundMovement>().gameObject;

    }

    public void ToggleMaxStamina(bool enable)
    {
        var stamina = playerRef.GetComponent<StaminaSystem>();
        baseStamina =stamina.GetCurrentStamina();
        if (enable)
        {
            stamina.RegenStamina();
        }
        else return;
    }

    public void GivePlayerTrinket(int trinket)
    {
        trinkets = trinket;
        playerRef.GetComponent<PlayerWingventory>().playerTrinketQuantity += trinkets;
    }

    public void ToggleMaxStealth()
    {

    }

    public void GivePlayerLevel()
    {
        var comp = playerRef.GetComponent<EXPSystem>();
        currentLevel= comp.PLAYERLEVEL;
        


    }

    public void GivePlayerMaxLevel()
    {

    }

    public void ToggleMaxPoop()
    {

    }

}
