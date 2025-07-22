using UnityEngine;

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
    [SerializeField] private float maxLifetime = 5f; //also adjust with testing

    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 target, PoopType type, PoopFunction functionSource)
    {
        poopType = type;
        source = functionSource;
        Vector3 direction = (target = transform.position).normalized; // Calculate direction to target
        rb.linearVelocity = direction * speed; //remember to update logic to be half player speed
        lifeTimer = maxLifetime; // Reset lifetime timer
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        source?.HandleHitEffects(poopType, collision.contacts[0].point); // Trigger hit effects

        if (collision.gameObject.TryGetComponent<IPoopable>(out var poopable))
        {
            poopable.OnPoopHit(poopType);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero; // Stop the projectile
        gameObject.SetActive(false); // Deactivate the projectile
        source?.ReturnProjectileToPool(this);
    }
}
