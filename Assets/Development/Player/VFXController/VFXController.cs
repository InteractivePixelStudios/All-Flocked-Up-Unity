using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem featherParticles;
    [SerializeField] private TrailRenderer streakLeft;
    [SerializeField] private TrailRenderer streakRight;
    [SerializeField] private ParticleSystem diveParticles;
    [SerializeField] private CapsuleCollider capsuleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        ToggleStreakOff();
    }

    private void OnCollisionEnter(Collision collision)
    {
        featherParticles.Play();
    }

    public void ToggleStreakOn()
    {
        streakLeft.emitting = true;
        streakRight.emitting = true;
    }

    public void ToggleStreakOff()
    {
        streakLeft.emitting = false;
        streakRight.emitting = false;
    }
}
