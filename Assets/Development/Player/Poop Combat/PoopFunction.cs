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
    [SerializeField] float forwardVelocity = 10f;
    [SerializeField] private float verticalVelocity = 5f;



    //Update to accept pigeon velocity - JK Oct23
    public void FirePoop(Vector3 target, Vector3 playerVelocity)
    {

            var projectile = Instantiate(projectilePrefab,spawnPoint.transform.position,spawnPoint.transform.rotation);
            projectile.Launch(target, currentPoopType, this, playerVelocity);
        
    }

    public void FireGroundPoop()
    {

        var projectile = Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * forwardVelocity + Vector3.up * verticalVelocity, ForceMode.Impulse);
        Debug.Log(projectile.transform.position);

    }

    public void HandleHitEffects(PoopType type, Vector3 position)
    {
        if (type.splatVFX) Instantiate(type.splatVFX, position, Quaternion.identity);
        if (type.splatSFX) Debug.Log("Play poop splat sound here"); // Delegate to AudioManager
    }




}
