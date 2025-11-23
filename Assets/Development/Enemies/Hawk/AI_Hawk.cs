using UnityEngine;

public class AI_Hawk : MonoBehaviour,I_EnemyBase
{
    [SerializeField] private bool isCooper;
    private enum birdType { CooperHawk, RedTailHawk}
    private birdType currentType=birdType.CooperHawk;
    private enum EnemyState { Patrolling, Chasing, Dive, Roll, Stop, Hit, Retreat,Perch }
    private EnemyState currentState = EnemyState.Patrolling;
    private GameObject player;
    private Rigidbody birdRB;
    public Vector3 takeOffForce;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    private Vector3 patrolTarget;
    [SerializeField] private float patrolRadius = 10f;
    private bool isHit;
    private bool isStopped;
    [Header("Flying")]
    [SerializeField] private float flySpeed = 10f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float altitude = 8f;
    [SerializeField] private float altChangeSpeed = 2f;
    [SerializeField] private float maxVelocity = 15f;
    [Header("Dive")]
    public float diveRange = 1f;
    private float diveForce = 20f;
    public float diveCooldown = 3f;
    [SerializeField] protected SphereCollider diveCollider;
    [SerializeField] private GameObject diveColliderParent;
    [Header("Roll")]
    public float rollRange = 3f;
    public float rollForce = 100f;
    public float rollCooldown = 3f;
    [Header("Perch")]
    private float perchRange = 2f;
    private Transform perchLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        birdRB = GetComponent<Rigidbody>(); 
        player = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
        birdRB.useGravity = false;
        birdRB.linearDamping = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        diveCooldown -= Time.deltaTime;
        rollCooldown -= Time.deltaTime;
        switch (currentType)
        {
            case birdType.CooperHawk:
                currentType = birdType.CooperHawk;
                UpdateCooper();
                break;
            case birdType.RedTailHawk:
                currentType=birdType.RedTailHawk;
                UpdateCooper();
                break;

        }
        
    }


    protected void UpdateCooper()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chasing;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Patrolling;
                else if (distanceToPlayer < diveRange)
                    currentState = EnemyState.Dive;
                else if (distanceToPlayer < rollRange)
                    currentState = EnemyState.Roll;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Dive:
                if (distanceToPlayer > diveRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Roll:
                if (distanceToPlayer > rollRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Stop:
                if (isHit)
                {
                    isHit = false;
                    isStopped = true;
                    currentState = EnemyState.Retreat;
                }
                break;

            case EnemyState.Retreat:
                if (isStopped)
                {
                    isStopped = false;
                    currentState = EnemyState.Patrolling;
                }
                else if (isHit)
                {
                    currentState = EnemyState.Hit;
                }
                break;

            case EnemyState.Hit:
                isHit = true;
                currentState = EnemyState.Stop;
                break;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:

                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Dive:
                if (diveCooldown <= 0)
                {
                    Dive();
                    Debug.Log("DiveCalled");
                    diveCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Roll:
                if (rollCooldown <= 0)
                {
                    Roll();
                    Debug.Log("PounceCalled");
                    rollCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Stop:
                StopMove();
                Debug.Log("StopCalled");
                break;
            case EnemyState.Hit:
                HitReact();
                Debug.Log("HitCalled");
                break;
            case EnemyState.Retreat:
                Retreat();
                Debug.Log("RetreatCalled");
                break;
        }
    }

    protected void UpdateRedTail()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chasing;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Patrolling;
                else if (distanceToPlayer < diveRange)
                    currentState = EnemyState.Dive;
                else if (distanceToPlayer < rollRange)
                    currentState = EnemyState.Roll;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Dive:
                if (distanceToPlayer > diveRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Roll:
                if (distanceToPlayer > rollRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Stop:
                if (isHit)
                {
                    isHit = false;
                    isStopped = true;
                    currentState = EnemyState.Retreat;
                }
                break;

            case EnemyState.Retreat:
                if (isStopped)
                {
                    isStopped = false;
                    currentState = EnemyState.Patrolling;
                }
                else if (isHit)
                {
                    currentState = EnemyState.Hit;
                }
                break;

            case EnemyState.Hit:
                isHit = true;
                currentState = EnemyState.Stop;
                break;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:

                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Dive:
                if (diveCooldown <= 0)
                {
                    Dive();
                    Debug.Log("DiveCalled");
                    diveCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Roll:
                if (rollCooldown <= 0)
                {
                    Roll();
                    Debug.Log("PounceCalled");
                    rollCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Stop:
                StopMove();
                Debug.Log("StopCalled");
                break;
            case EnemyState.Hit:
                HitReact();
                Debug.Log("HitCalled");
                break;
            case EnemyState.Retreat:
                Retreat();
                Debug.Log("RetreatCalled");
                break;
        }
    }

    protected void KeepAltitude()
    {
        if(Physics.Raycast(transform.position,Vector3.down, out RaycastHit hit, 50f))
        {
            float targetY = hit.point.y + altitude;
            float altOffset = targetY-transform.position.y;
            birdRB.AddForce(Vector3.up * altOffset * altChangeSpeed, ForceMode.Acceleration);
        }
    }

    protected void DetectPlayer()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            if(currentState == EnemyState.Patrolling)
            {
                currentState = EnemyState.Chasing;
            }
        }
    }

    protected void TakeOff()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        dirToPlayer.y = 0;
        Vector3 force = dirToPlayer * takeOffForce.z + Vector3.up * takeOffForce.y;
        birdRB.AddForce(force, ForceMode.Impulse);
    }

    protected void Fly()
    {
        birdRB.linearVelocity = transform.forward * flySpeed;
    }

    protected void Patrol()
    {
        if(Vector3.Distance(transform.position,patrolTarget)>2f||patrolTarget == Vector3.zero)
        {
            Vector2 randomRad = Random.insideUnitCircle * patrolRadius;
            patrolTarget = new Vector3(
                transform.position.x + randomRad.x,
                transform.position.y + Random.Range(-2f, 2f),
                transform.position.z + randomRad.y);
        };

        Vector3 dirToTarget = (patrolTarget-transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dirToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);

        Fly();
    }

    protected void ChasePlayer()
    {
        KeepAltitude();
        Vector3 diveDir = (player.transform.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(diveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        birdRB.linearVelocity = transform.forward * flySpeed;

        if (diveCooldown <= 0)
        {
            currentState = EnemyState.Dive;
        }
    }

    protected void Dive()
    {

        Vector3 diveDir = (player.transform.position- transform.position).normalized;
        birdRB.AddForce(diveDir*flySpeed*2f, ForceMode.Impulse);
        diveCooldown = 3f;
        currentState = EnemyState.Chasing;
        
    }

    protected void Roll()
    {
        birdRB.AddTorque(transform.forward * 300f, ForceMode.Impulse);
        rollCooldown = 3f;
        currentState = EnemyState.Chasing;
    }

    protected void Perch()
    {

    }

    protected void StopMove()
    {
        birdRB.linearVelocity = Vector3.zero;
        birdRB.angularVelocity = Vector3.zero;
    }

    protected void HitReact()
    {
        birdRB.AddForce(-transform.forward * 5f + Vector3.up * 2f, ForceMode.Impulse);
        currentState = EnemyState.Retreat;
    }

    protected void Retreat()
    {
        KeepAltitude();
        birdRB.AddForce(-transform.forward*flySpeed*0.5f,ForceMode.Acceleration);
        if (Vector3.Distance(transform.position, player.transform.position) > detectionRange * 2f)
        {
            currentState = EnemyState.Patrolling;
        }
    }

    public void OnDeath(bool isDead)
    {

    }

    public void TakeDamage(int damage)
    {

    }
}
