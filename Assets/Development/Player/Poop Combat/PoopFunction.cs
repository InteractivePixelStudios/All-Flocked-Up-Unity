using System.Collections.Generic;
using UnityEngine;

public class PoopFunction : MonoBehaviour
{
    //Use cooldown to manage poop shooting frequency
    //Trigger splat anims/vfx on hit
    //Trigger audio on shoot and hit

    [Header("Script References")]
    private Audio_Player audioPlayer;

    [Header("Poop Settings")]
    [SerializeField] private PoopType currentPoopType;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PoopProjectile projectilePrefab;
    [SerializeField] private int poolSize = 10; //adjust as needed
    [SerializeField] float forwardVelocity = 10f;
    [SerializeField] private float verticalVelocity = 5f;

    [Header("Variables")]
    // Sound Index is temp and only for trailer use - IPM
    private int soundIndex = 0; // 0 = poop, 1 = splat 

    private void Start() // Added by Isaiah PM.
    {
        if (audioPlayer == null)
        {
            audioPlayer = GetComponent<Audio_Player>();
            if (audioPlayer == null)
            {
                Debug.LogError("Audio_Player component not found on the GameObject.");
            }
        }
    }

    //Update to accept pigeon velocity - JK Oct23
    public void FirePoop(Vector3 target, Vector3 playerVelocity)
    {
        var projectile = Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        projectile.Launch(target, currentPoopType, this, playerVelocity);
        SoundCaller(currentPoopType, soundIndex = 0); // Added by Isaiah PM.
    }

    public void FireGroundPoop()
    {
        var projectile = Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * forwardVelocity + Vector3.up * verticalVelocity, ForceMode.Impulse);
        SoundCaller(currentPoopType, soundIndex = 0); // Added by Isaiah PM.
    }

    public void HandleHitEffects(PoopType type, Vector3 position)
    {
        SoundCaller(type, soundIndex = 1); // Added by Isaiah PM.
        if (type.splatVFX) Instantiate(type.splatVFX, position, Quaternion.identity);
        if (type.splatSFX) Debug.Log("Play poop splat sound here"); // Delegate to AudioManager
    }

    //Method added by Isaiah PM.
    private void SoundCaller(PoopType type, int soundIndex) // Type may be used down the line, would be interesting.
    {
        if (soundIndex == 0)
            audioPlayer.Poop();
        else if (soundIndex == 1)
            audioPlayer.Splat();
    }

}

// Notes - Edit History 
// January 14th, 2026 - Added SoundCaller method and integrated sound calls into FirePoop and HandleHitEffects methods. - Isaiah PM
