using UnityEngine;
using UnityEngine.InputSystem;

public class VFXController : MonoBehaviour
{
    PlayerInput input;
    InputAction diveAction;
    [SerializeField] private ParticleSystem featherParticles;
    [SerializeField] private TrailRenderer streakLeft;
    [SerializeField] private TrailRenderer streakRight;
    [SerializeField] private ParticleSystem diveParticles;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private ParticleSystem questCompleteParticles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        ToggleStreakOff();
        diveAction = input.actions.FindAction("Dive");
        if (diveAction != null) diveAction.performed += ToggleDiveStreaks;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 10)
        {
            featherParticles.Play();
        }
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

    void ToggleDiveStreaks(InputAction.CallbackContext ctx)
    {
        diveParticles.Play();
    }

    public void PlayQuestParticles()
    {
        questCompleteParticles.Play();
    }
}
