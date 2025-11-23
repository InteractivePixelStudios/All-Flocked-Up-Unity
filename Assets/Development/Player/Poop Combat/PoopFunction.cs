using System.Collections.Generic;
using UnityEngine;

public class PoopFunction : MonoBehaviour
{
    //Use cooldown to manage poop shooting frequency
    //Trigger splat anims/vfx on hit
    //Trigger audio on shoot and hit

    [Header("Poop Settings")]
    [SerializeField] private PoopType currentPoopType;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PoopProjectile projectilePrefab;
    [SerializeField] private int poolSize = 10; //adjust as needed

    private Queue<PoopProjectile> projectilePool = new Queue<PoopProjectile>();

    private void Awake()
    {
       for (int i = 0; i < poolSize; i++)
        {
            PoopProjectile proj = Instantiate(projectilePrefab, transform);
            proj.gameObject.SetActive(false); // Deactivate initially
            projectilePool.Enqueue(proj);
        }
    }

    //Update to accept pigeon velocity - JK Oct23
    public void FirePoop(Vector3 target, Vector3 playerVelocity)
    {
        if (projectilePool.Count > 0)
        {
            var projectile = projectilePool.Dequeue();
            projectile.transform.position = spawnPoint.position;
            projectile.gameObject.SetActive(true);
            projectile.Launch(target, currentPoopType, this, playerVelocity);
        }
    }

    public void HandleHitEffects(PoopType type, Vector3 position)
    {
        if (type.splatVFX) Instantiate(type.splatVFX, position, Quaternion.identity);
        if (type.splatSFX) Debug.Log("Play poop splat sound here"); // Delegate to AudioManager
    }

    public void ReturnProjectileToPool(PoopProjectile poop)
    {
        poop.gameObject.SetActive(false); // Deactivate the projectile
        projectilePool.Enqueue(poop); // Return to pool
    }



}
