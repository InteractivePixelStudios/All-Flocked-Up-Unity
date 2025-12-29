using UnityEngine;

public class Anim_HumanController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyPatrol enemy;
    [SerializeField] private KickComponent kick;

    public float speed;
    public bool isHit;
    public bool isSitting;
    public bool isThrowing;
    public bool isWalking;
    public bool isKicking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleWalk(bool walking)
    {
        if (walking && speed>0.1)
        {
            animator.SetBool("isWalking", true);
        }
        else animator.SetBool("isWalking", true);
    }

    void ToggleHit(bool hit)
    {
        if (hit)
        {
            animator.SetBool("isHit",true);
        }else
        animator.SetBool("isHit", false);
    }

    void ToggleSitting(bool sitting)
    {
        if (sitting)
        {
            animator.SetBool("isSitting", true);
        }else 
        animator.SetBool("isSitting", false);
    }

    void ToggleThrowing(bool throwing)
    {
        if (throwing)
        {
            animator.SetBool("isThrowing", true);
        }
        else animator.SetBool("isThrowing", false);
    }

    void ToggleKicking(bool kicking)
    {
        if (kicking)
        {
            animator.SetBool("isKicking", true);
        }
        else animator.SetBool("isKicking", false);
    }
}
