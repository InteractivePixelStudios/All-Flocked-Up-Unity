using UnityEngine;

public class AudioAnim : MonoBehaviour // Dirty temp script - will be updated when audio system is redone.
{
    private Animator animator;
    private AIMovePoints aiMovePoints;

    public bool walk;
    public bool run;
    public bool jump;
    public bool sneak;
    public bool humanWalk;

    public float moveSpeed;

    public bool defeated;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aiMovePoints = GetComponent<AIMovePoints>();
    }

    private void Update()
    {
        if (aiMovePoints != null)
        {
            moveSpeed = aiMovePoints.moveSpeed;
        }
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    private void Start() // DIRTY I KNOW BUT IT WORKS FOR NOW
    {
        if (walk) animator.SetTrigger("Walk");
        if (run) animator.SetTrigger("Run");
        if (jump) animator.SetTrigger("Jump");
        if (sneak) animator.SetTrigger("Sneak");
        if (defeated) animator.SetTrigger("Defeat");
        if (humanWalk) animator.SetTrigger("HumanWalk");
    }
}
