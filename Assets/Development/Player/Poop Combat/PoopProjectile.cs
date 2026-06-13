using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PoopProjectile : MonoBehaviour
{
   //JK notes from unreal:
    //Use projectile movement to handle poop shooting
    //Get pigeon ref/then target/then poop fire point (spawn point)
    //Calculate Distance travelled with halved player velocity
    //Draw aerial reticle here? TBD
    //Calculate time to reach ground then spawn poop decal at that point

    private Rigidbody rb;
    private PoopFunction source;
    private PoopType poopType;

    [SerializeField] private float speed = 15f; //temporary, this should be half of the player speed

    private float lifeTimer;

    [SerializeField] private PoopSplatDecal decalPrefab;
    [SerializeField] private ParticleSystem splashParticle;
    [SerializeField] EventReference splatSFX;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetPoopType(PoopType type)
    {
        poopType = type;
    }

    public void SetPoopFuction(PoopFunction fuction)
    {
        source = fuction;
    }

    public void Launch(Vector3 target, PoopType type, PoopFunction functionSource, Vector3 playerVelocity)
    {


        Vector3 direction = (target - transform.position).normalized;
        Debug.Log(target);

        float launchSpeed = playerVelocity.magnitude * 0.5f; // Launch speed is half the player's speed

        if (launchSpeed <= 0.1f)
        {
            launchSpeed = speed; // Fallback to default speed if player is stationary
        }

        //rb.linearVelocity = direction * launchSpeed;

    }

    private void SpawnPoopDecal(Vector3 position, Vector3 hit)
    {
        PoopSplatDecal spawned;
        if (hit.y >= 1)
        {

            spawned = Instantiate(decalPrefab, position, Quaternion.Euler(90, 0, 0));
        }
        else if (hit.x >= 1)
        {

            spawned = Instantiate(decalPrefab, position, Quaternion.Euler(0, 90, 0));
        }
        else if (hit.z <= 0)
        {
            spawned = Instantiate(decalPrefab, position, Quaternion.Euler(0, 0, 0));
        }
        else return;
    }


    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
        var hit = collision.GetContact(0).normal;
        SpawnPoopDecal(transform.position,hit);
        source?.HandleHitEffects(poopType, collision.contacts[0].point); // Trigger hit effects
        AudioWizard.Instance.PlayOneshotSound(splatSFX, transform.position);
        var poopable = obj.GetComponentInParent<PoopableObject>();
        if (poopable != null)
        {
            poopable.OnPoopHit(poopType);
            Debug.Log(poopable);
        }
        if (collision.gameObject.CompareTag("Cat"))
        {
            obj.GetComponent<EnemyBaseComponent>().TakeDamage(10);
            Destroy(gameObject);
            Debug.Log("EnemyHit");
        }

        if (collision.gameObject.CompareTag("Dog"))
        {
            obj.GetComponent<EnemyBaseComponent>().TakeDamage(10);
            Destroy(gameObject);
            Debug.Log("EnemyHit");
        }

        if (collision.gameObject.CompareTag("Human"))
        {
            obj.GetComponentInParent<EnemyPatrol>().TakeDamage(10);
            Destroy(gameObject);
            Debug.Log("EnemyHit");
        }

        if (collision.gameObject.CompareTag("NPC"))
        {
            obj.GetComponent<NPCBase>().HitReact();
            Destroy(gameObject);
            Debug.Log("NPCHit");
        }
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            var vehicle = collision.gameObject.GetComponent<VehicleScript>();
            vehicle.TriggerCollisions();
            Destroy(gameObject);
            Debug.Log("CarHit");
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }


}
