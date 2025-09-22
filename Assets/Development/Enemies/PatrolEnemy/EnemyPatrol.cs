using UnityEngine;

public class EnemyPatrol : MonoBehaviour, I_EnemyBase
{
    public Transform[] patrolPoints;
    public Transform player;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRange = 5f;
    public float loseSightRange = 8f;

    private int currentPointIndex = 0;
    private enum EnemyState { Patrolling, Chasing }
    private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState == EnemyState.Patrolling && distanceToPlayer < detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
        else if (currentState == EnemyState.Chasing && distanceToPlayer > loseSightRange)
        {
            currentState = EnemyState.Patrolling;
        }

        if (currentState == EnemyState.Patrolling)
        {
            Patrol();
        }
        else if (currentState == EnemyState.Chasing)
        {
            ChasePlayer();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 targetPos = patrolPoints[currentPointIndex].position;
        targetPos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);

        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;

        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    public void TakeDamage(int damage)
    {


    }

    public void OnDeath(bool IsDead)
    {

    }
}
