using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AI_Cat : EnemyBaseComponent
{
    [Header("Patrol")]
    public GameObject patrolPoints;
    public GameObject player;
    public PlayerStealthSystem playerStealth;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    float distanceToNode;
    Vector3 retreatLocation;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    [Header("Swat")]
    public float swatRange = 1f;
    public float swatCooldown = 3f;
    [SerializeField] protected SphereCollider swatCollider;
    [SerializeField] private GameObject swatColliderParent;
    [Header("Pounce")]
    public float pounceRange = 3f;
    public Vector3 pounceForce;
    public float pounceCooldown = 3f;
    [Header("Waypoints")]
    [SerializeField] private List<Waypoint> waypoints;
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [Header("Components")]
    [SerializeField] protected Rigidbody rigidbodyComp;
    [SerializeField] protected Enemy_AlertIcon icon;
    [SerializeField] protected Animator animator;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isStopped;
    [SerializeField] protected bool isRetreating;
    [SerializeField] protected bool canSeePlayer;

    ReactionState currentReactionState;

   // private int currentPointIndex = 0;
    public enum EnemyState { Patrolling, Chasing, Swat, Pounce, Stop, Hit, Retreat }
    private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;
    [SerializeField]bool isTutCat;

    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        playerStealth = player.GetComponent<PlayerStealthSystem>();
        animator = GetComponent<Animator>();
        icon = GetComponent<Enemy_AlertIcon>();
        rigidbodyComp = GetComponent<Rigidbody>();
        FindWaypoints();
    }

    public void SetCurrentState(EnemyState state)
    {
        currentState = state;
    }

    void Update()
    {
        if (swatCooldown >= 0) swatCooldown -= Time.deltaTime;
        if (pounceCooldown >= 0) pounceCooldown -= Time.deltaTime;
        if (canSeePlayer) {  icon.SetPlayerSeen(true); } else if(!canSeePlayer) { icon.SetPlayerSeen(false); }
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(currentNode != null) {  distanceToNode = Vector3.Distance(transform.position, currentNode.transform.position); }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (playerStealth.GetStealth()<10 && distanceToPlayer < detectionRange && !this.isHit)
                    currentState = EnemyState.Chasing;
                else if (this.isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > detectionRange && !this.isHit)
                    currentState = EnemyState.Patrolling;
                else if (distanceToPlayer < swatRange && !this.isHit)
                    currentState = EnemyState.Swat;
                else if (distanceToPlayer < pounceRange && !this.isHit)
                    currentState = EnemyState.Pounce;
                else if (this.isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Swat:
                if (distanceToPlayer > swatRange && !this.isHit)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Pounce:
                if (distanceToPlayer > pounceRange && !this.isHit)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Stop:
                if (this.isHit)
                {
                    currentState = EnemyState.Retreat;
                    isStopped = true;
                    
                }
                break;

            case EnemyState.Retreat:
                if (isStopped && !this.isHit)
                {
                    isStopped = false;
                    currentState = EnemyState.Hit;
                }
                else if (!this.isHit)
                {
                    currentState = EnemyState.Patrolling;
                }
                break;

            case EnemyState.Hit:
                this.isHit = true;
                break;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                canSeePlayer = false;
                MoveCatToLocation();
                if (distanceToNode < 0.5f && !this.isHit)
                    ChooseNextDirection(currentNode);
                break;
            case EnemyState.Chasing:
                canSeePlayer = true;
                ChasePlayer();
                break;
            case EnemyState.Swat:
                if (swatCooldown <= 0 && !this.isHit)
                {
                    SwatPlayer();
                    Debug.Log("SwatCalled");
                    swatCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Pounce:
                if (pounceCooldown <= 0 && !this.isHit)
                {
                    Pounce();
                    Debug.Log("PounceCalled");
                    pounceCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Stop:
                StopMove();
                break;
            case EnemyState.Hit:
                HitReact();
                break;
            case EnemyState.Retreat:
                if (!isRetreating)
                {
                    isRetreating = true;
                    Retreat();
                }
                RetreatToLocation();
                break;
        }

    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", rigidbodyComp.maxLinearVelocity);
    }

    private void FindWaypoints()
    {
        if (isTutCat) return;
        var waypointsArray = patrolPoints.GetComponentsInChildren<Waypoint>();
        foreach (var waypoint in waypointsArray)
        {
            if (waypoint.CompareTag("Cat"))
            {
                waypoints.Add(waypoint);
            }

        }
        FindRandomWaypoint();
    }


    private void FindRandomWaypoint()
    {
        var randomIndex = Random.Range(0, waypoints.Count);
        this.currentNode = waypoints[randomIndex];

    }


    protected void StopMove()
    {
        rigidbodyComp.linearVelocity = Vector3.zero;
    }

    protected void HitReact()
    {

        isHit = false;
        currentState = EnemyState.Retreat;
    }

    public override void OnHit(PoopType type)
    {
        currentReactionState = type.poopReaction;
        switch (currentReactionState)
        {
            case ReactionState.Normal:
                //animator.SetTrigger("isHit");
                break;
            case ReactionState.Fire:
                //animator.SetTrigger("isHit");
                break;
            case ReactionState.Confetti:
                //animator.SetTrigger("isHit");
                break;
            case ReactionState.Glow:
                //animator.SetTrigger("isHit");
                break;
        }
        isHit = true;
        Debug.Log("HitCat");
        SetCurrentState(EnemyState.Hit);
    }

    protected void Retreat()
    {
        var radius = 2000f;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0f;
        retreatLocation = transform.position + randomDirection;
        Debug.Log(retreatLocation);
        isRetreating = true;
        
    }

    void RetreatToLocation()
    {
        transform.position = Vector3.MoveTowards(transform.position, retreatLocation, chaseSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, retreatLocation) < 5)
        {
            isRetreating = false;
            isHit = false;
            isStopped = false;
            currentState = EnemyState.Patrolling;
        }
        Vector3 dir = (retreatLocation - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    protected void ChasePlayer()
    {
        Vector3 targetPos = player.transform.position;
        targetPos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    protected async void SwatPlayer()
    {
        animator.SetTrigger("isClaw");
        var spawnedCollider = swatColliderParent.AddComponent<SphereCollider>();
        var comp = spawnedCollider.AddComponent<KickComponent>(); //used as damage comp
        comp.damage = 3;
        spawnedCollider.includeLayers = LayerMask.GetMask("Player");
        spawnedCollider.isTrigger = true;
        spawnedCollider.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        swatCooldown = 3f;
        await Task.Delay(3000);
        Destroy(spawnedCollider);
        Destroy(comp);
    }

    protected async void Pounce()
    {
        animator.SetTrigger("isPounce");
        rigidbodyComp.linearVelocity = Vector3.zero;
        await Task.Delay(1000);
        Vector3 dirToPlayer = (player.transform.position-transform.position).normalized;
        dirToPlayer.y = 0;
        Vector3 force = dirToPlayer * pounceForce.z + Vector3.up * pounceForce.y;
        rigidbodyComp.AddForce(force,ForceMode.Impulse);
        var spawnedCollider = swatColliderParent.AddComponent<SphereCollider>();
        spawnedCollider.includeLayers = LayerMask.GetMask("Player");
        spawnedCollider.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        spawnedCollider.isTrigger = true;
        var comp = spawnedCollider.AddComponent<KickComponent>(); //used as damage comp
        comp.damage = 3;
        Destroy(spawnedCollider);
        pounceCooldown = 3f;
        Destroy(comp);
    }

    protected virtual void SetMoveToLocation(Waypoint location)
    {
        currentNode = location;
    }

    //call this to run like wind
    public virtual void MoveCatToLocation()
    {
        if (currentNode == null || rigidbodyComp == null)
            return;

        Vector3 direction = (currentNode.transform.position - transform.position).normalized;
        rigidbodyComp.MovePosition(transform.position + direction * patrolSpeed * Time.deltaTime);
        transform.forward = direction;

    }

    public virtual void StopVehicle()
    {
        rigidbodyComp.linearVelocity = Vector3.zero;
        //Debug.Log("Stopping");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Poop"))
        {
            var type = collision.gameObject.GetComponent<PoopProjectile>().GetPoopType();
            TakeDamage(1, type);
        }
    }


    protected void ChooseNextDirection(Waypoint node)
    {

        if (node.nextWaypoint == null)
        {
            FindRandomWaypoint();
            return;
        }
        else
        {
            Waypoint nextNode = node.nextWaypoint;
            if (nextNode == null)
                return;
            previousNode = currentNode;
            SetMoveToLocation(nextNode);
            MoveCatToLocation();
        }




    }
}
