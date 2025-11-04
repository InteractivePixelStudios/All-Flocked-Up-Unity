using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem featherParticles;
    [SerializeField] private ParticleSystem streakParticles;
    [SerializeField] private ParticleSystem diveParticles;
    [SerializeField] private CapsuleCollider capsuleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        featherParticles.Play();
    }
}
