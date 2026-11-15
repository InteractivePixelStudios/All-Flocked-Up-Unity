using System.Threading.Tasks;
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

    public int sitDelayMs;
    public int exitSitDelayMs;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleWalk(bool walking, float speed)
    {
       
    }

    public void ToggleHit(bool hit)
    {
        if (hit)
        {
            animator.SetBool("isHit",true);
        }else
        animator.SetBool("isHit", false);
    }

    public async void StartSit()
    {
        animator.SetTrigger("StartSit");
        await Task.Delay(sitDelayMs);
        isSitting = true;
        ToggleSitting(isSitting);
    }

    public async void ExitSit()
    {
        animator.SetTrigger("ExitSit");
        await Task.Delay(exitSitDelayMs);
        isSitting= false;
        ToggleSitting(isSitting);
    }

    public void ToggleSitting(bool sitting)
    {
        if (sitting)
        {
            animator.SetBool("isSitting", true);
        }else 
        animator.SetBool("isSitting", false);
    }

    public void ToggleThrowing(bool throwing)
    {
        if (throwing)
        {
            animator.SetBool("isThrowing", true);
        }
        else animator.SetBool("isThrowing", false);
    }

    public void ToggleKicking(bool kicking)
    {
        if (kicking)
        {
            animator.SetBool("isKicking", true);
        }
        else animator.SetBool("isKicking", false);
    }
}
