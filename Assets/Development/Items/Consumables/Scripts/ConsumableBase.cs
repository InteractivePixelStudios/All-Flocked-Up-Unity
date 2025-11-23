using System.Collections;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
enum E_ConsumableType {Health, Stamina,Poop,Custom }
enum E_ReactionType { Health, Stamina,Poop,Speed,Flight,FlySpeed,Custom }
public class ConsumableBase : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Mesh consumableMesh;
    [SerializeField] private GameObject playerRef;
    [Header("States")]
    [SerializeField] private E_ConsumableType consumableType;
    [SerializeField] private bool hasReaction;
    [SerializeField] private E_ReactionType reactionType;

    [SerializeField] private int positiveEffectValue;
    [SerializeField] private int positiveModifier;
    [SerializeField] private int negativeEffectValue;
    [SerializeField] private int negativeModifier;
    //[Header("Health")]
    //[SerializeField] private bool isHealth;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] StaminaSystem playerStamina;
    //[SerializeField] private int healthToRegen;
    //[SerializeField] private int healthToLose;
    //[Header("Stamina")]
    //[SerializeField] private bool isStamina;
    //[SerializeField]private float staminaToRegen;
    //[SerializeField] private float staminaToLose;
    //[Header("Poop")]
    //[SerializeField] private bool isPoop;
    //[SerializeField] private int poopToRegen;
    //[SerializeField] private int poopToLose;
    //[Header("Custom")]
    //[SerializeField] private bool isCustom;
    //[Header("ReactionModifiers")]
    //[SerializeField] private int healthModifier;
    //[SerializeField] private float staminaModifier;
    //[SerializeField] private int poopModifier;
    //[SerializeField] private float groundSpeedModifier;
    [SerializeField] private bool canFly;
    [SerializeField] private bool canPoop;
    //[SerializeField] private float flightSpeedModifier;
    [Header("Particles")]
    [SerializeField] private ParticleSystem consumableParticles;

    //gets comps
    private void Awake()
    {
        consumableMesh = GetComponent<MeshFilter>().sharedMesh;
        consumableParticles = GetComponent<ParticleSystem>();
    }

    //trigger checks if player and sets the playerRef, then calls effect and destroy
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            ConsumableEffect(consumableType);
            Destroy(gameObject);
        }
    }
    //switch case based on consumabletype enum... this is what triggers each effect when collected
    private void ConsumableEffect(E_ConsumableType consumableType)
    {
        switch (consumableType)
        {
            case E_ConsumableType.Health:
                //isHealth = true;
                playerHealth = playerRef.GetComponent<PlayerHealth>();
                if (playerHealth.currentHealth <= playerHealth.maxHealth) { playerRef.GetComponent<PlayerWingventory>().AddItemToInv(this.gameObject, 1); }
                HealEffect(positiveEffectValue, positiveModifier);
                CheckForReaction();
                break;
            case E_ConsumableType.Stamina:
                playerStamina = playerRef.GetComponent<StaminaSystem>();
                //Change me to add to INV
                //if (playerHealth.currentHealth <= playerHealth.maxHealth) { playerRef.GetComponent<PlayerWingventory>().AddItemToInv(this.gameObject, 1); }
                StaminaEffect(positiveEffectValue,positiveModifier);
                CheckForReaction();
                break;
            case E_ConsumableType.Poop:
                //isPoop = true;
                //Change me to add to INV
                //if (playerHealth.currentHealth <= playerHealth.maxHealth) { playerRef.GetComponent<PlayerWingventory>().AddItemToInv(this.gameObject, 1); }
                PoopEffect(positiveEffectValue,positiveModifier,canPoop);
                CheckForReaction();
                break;
            case E_ConsumableType.Custom:
                //isCustom = true;
                //Change me to add to INV
                //if (playerHealth.currentHealth <= playerHealth.maxHealth) { playerRef.GetComponent<PlayerWingventory>().AddItemToInv(this.gameObject, 1); }
                CustomEffect(positiveEffectValue,positiveModifier,canFly,canPoop);
                CheckForReaction();
                break;
        }
    }
    //switch case based on reaction effects... this is what neg effect triggers when collected
    private void ReactionEffect(E_ReactionType reactionType)
    {
        switch (reactionType)
        {
            
            case E_ReactionType.Health:
                HealEffect(negativeEffectValue, negativeModifier);
                break;
            case E_ReactionType.Stamina:
                StaminaEffect(negativeEffectValue, negativeModifier);
                break;
            case E_ReactionType.Poop:
                PoopEffect(negativeEffectValue, negativeModifier, canPoop);
                break;
            case E_ReactionType.Custom:
                CustomEffect(-negativeEffectValue, negativeModifier, canFly,canPoop);
                break;
            case E_ReactionType.Speed:
                SpeedEffect(negativeEffectValue);
                break;
            case E_ReactionType.Flight:
                FlightEffect(!canFly);
                break;
            case E_ReactionType.FlySpeed:
                FlySpeedEffect(negativeEffectValue);
                break;
           

        }
    }
    //check for if object has reaction
    private void CheckForReaction()
    {
        if (hasReaction)
        {
            ReactionEffect(reactionType);
        }
    }
    //heal function
    private void HealEffect(int health, int modifier)
    {
        playerHealth = playerRef.GetComponent<PlayerHealth>();
        var currenthealth = playerHealth.currentHealth;
        currenthealth += health;
        if (modifier > 0)
        {
            currenthealth *= modifier;
            currenthealth = playerHealth.currentHealth;
            return;
        }
        else
            currenthealth = playerHealth.currentHealth;
    }

    //stamina effect
    private void StaminaEffect(float stamina, float modifier)
    {
        if (modifier > 0)
        {

        }
        else
            Debug.Log("");
    }

    //poop effect
    private void PoopEffect(int poop,int poopMod, bool canPoop)
    {
        if (poopMod > 0)
        {

        }
        else
            Debug.Log("");
    }
    //custom effect
    private void CustomEffect(int health, int healthMod,  bool canFly, bool canPoop)
    {
        if (healthMod > 0)
        {

        }
        //if (stamMod > 0)
        //{

        //}
        //if (poopMod > 0)
        //{

        //}
        //if (groundSpeedMod > 0)
        //{

        //}
        //if (flySpeedMod > 0)
        //{

        //}
        if (!canFly)
        {

        }
        if (!canPoop)
        {

        }
        else
            Debug.Log("");
    }

    //speed effect
    private void SpeedEffect(float speed)
    {

    }
    //flight effect
    private void FlightEffect(bool canFly)
    {

    }
    //flyspeed effect
    private void FlySpeedEffect(float flySpeed)
    {

    }

    public void UseConsumable()
    {
        ConsumableEffect(consumableType);
        Destroy(gameObject);
    }
    
}
