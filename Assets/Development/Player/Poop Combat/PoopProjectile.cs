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


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 target, PoopType type, PoopFunction functionSource, Vector3 playerVelocity)
    {
        poopType = type;
        source = functionSource;

        Vector3 direction = (target - transform.position).normalized;

        float launchSpeed = playerVelocity.magnitude * 0.5f; // Launch speed is half the player's speed

        if (launchSpeed <= 0.1f)
        {
            launchSpeed = speed; // Fallback to default speed if player is stationary
        }

        rb.linearVelocity = direction * launchSpeed;

    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //source?.HandleHitEffects(poopType, collision.contacts[0].point); // Trigger hit effects

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //poopable.OnPoopHit(poopType);
            Debug.Log("EnemyHit");
        }
        if (collision.gameObject.CompareTag("NPC"))
        {
            Debug.Log("NPCHit");
        }
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            var vehicle = collision.gameObject.GetComponent<VehicleScript>();
            //Add Honk
            Debug.Log("CarHit");
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }


}
