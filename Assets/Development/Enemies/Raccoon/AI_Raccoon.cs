using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class AI_Raccoon : MonoBehaviour, I_EnemyBase
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public GameObject player;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    [Header("Climb")]
    private bool isClimbing;
    [SerializeField]private LayerMask climbLayer;
    [SerializeField] private float climbDuration = 1.5f;
    [SerializeField] private float climbSpeed = 2f;
    private float climbTimer = 0f;
    private Vector3 climbStartPos;
    private Vector3 climbEndPos;
    public float climbRange = 1f;
    public float climbCooldown = 3f;
    [Header("Search")]
    private bool isSearching;
    public float searchRange = 3f;
    public float searchCooldown = 3f;
    private float searchTimer = 0f;
    [SerializeField]private float maxSearchTime = 30f;
    [Header("Waypoints")]
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField] private List<WaypointConnection> connections = new();
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [Header("Components")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isStopped;
    [SerializeField] protected bool isRetreating;

    private int currentPointIndex = 0;
    private enum EnemyState { Patrolling, Chasing, Climb, Search, Stop, Hit, Retreat }
    private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
        animator = GetComponent<Animator>();
        FindWaypoints();
    }

    void Update()
    {
        if (climbCooldown >= 0) climbCooldown -= Time.deltaTime;
        if (searchCooldown >= 0) searchCooldown -= Time.deltaTime;
        if(isSearching&&searchTimer>0) searchTimer -= Time.deltaTime; else { searchTimer = maxSearchTime; }
        if (climbCooldown > 0) climbCooldown -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceToNode = Vector3.Distance(transform.position, currentNode.transform.position);
        CheckForClimb();
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chasing;
                else if (isHit)
                    currentState = EnemyState.Hit;
                else if (isClimbing)
                    currentState = EnemyState.Climb;
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Patrolling;
                else if (isClimbing)
                    currentState = EnemyState.Climb;
                else if (isSearching)
                    currentState = EnemyState.Search;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Climb:
                if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Search;
                else if (distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Search:
                if (distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chasing;
                else if (searchTimer < maxSearchTime)
                    currentState = EnemyState.Patrolling;
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
                MoveRacoonToLocation();

                if (distanceToNode < 1f)
                    ChooseNextDirection(currentNode);
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Climb:
                if (climbCooldown <= 0)
                {
                    Climb();
                    Debug.Log("ClimbCalled");
                    climbCooldown = 3f;
                }
                break;
            case EnemyState.Search:
                if (searchCooldown <= 0)
                {
                    Search();
                    Debug.Log("SearchCalled");
                    searchCooldown = 3f;
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


        if (isClimbing)
            Climb();

    }


    private void FindWaypoints()
    {
        var waypointsArray = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        foreach (var waypoint in waypointsArray)
        {
            if (waypoint.CompareTag("Racoon"))
            {
                waypoints.Add(waypoint);
            }

        }
        FindRandomWaypoint();
        Debug.Log("CheckforWaypoints");
    }


    private void FindRandomWaypoint()
    {
        var randomIndex = Random.Range(0, waypoints.Count);
        var transform = waypoints[randomIndex].transform.position;
        this.transform.position = transform;
        this.currentNode = waypoints[randomIndex];
        Debug.Log("H");
    }


    protected void StopMove()
    {
        navAgent.isStopped = true;
    }

    protected async void HitReact()
    {
        isHit = true;
        animator.SetTrigger("isHit");
        TakeDamage(1);
        await Task.Delay(1000);
        isHit = false;
        currentState = EnemyState.Retreat;
    }

    protected void Retreat()
    {
        var radius = 5f;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0f;
        Vector3 randomPosition = transform.position + randomDirection;
        navAgent.SetDestination(randomPosition);
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
    protected void CheckForClimb()
    {
        if (isClimbing || climbCooldown > 0) return;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, transform.forward*0.5f,out RaycastHit hit, 5f, climbLayer))
        {
            StartClimb(hit);
            
        }
    }

    protected void StartClimb(RaycastHit hit)
    {


    }

    protected void Climb()
    {


    }

    protected void Search()
    {

    }



    public void TakeDamage(int damage)
    {


    }

    public void OnDeath(bool IsDead)
    {

    }

    protected virtual void SetMoveToLocation(Waypoint location)
    {
        currentNode = location;
    }

    //call this to run like wind
    public virtual void MoveRacoonToLocation()
    {
        if (currentNode == null || navAgent == null)
            return;

        Vector3 direction = (currentNode.transform.position - transform.position).normalized;
        navAgent.SetDestination(transform.position + direction * patrolSpeed * Time.deltaTime);
        transform.forward = direction;

    }

    public virtual void StopRacoon()
    {
        navAgent.isStopped = true;
        //Debug.Log("Stopping");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Poop"))
        {

            TakeDamage(1);
        }
    }


    protected void ChooseNextDirection(Waypoint node)
    {
        connections.Clear();

        foreach (var connection in node.connections)
            connections.Add(connection);

        if (connections.Count == 0 && node.nextWaypoint != null)
        {
            connections.Add(new WaypointConnection { node = node.nextWaypoint });

        }
        else
        {
            FindRandomWaypoint();
            return;
        }

        int randomIndex = Random.Range(0, connections.Count);
        Waypoint nextNode = connections[randomIndex].node;
        if (nextNode == null)
            return;
        previousNode = currentNode;
        SetMoveToLocation(nextNode);
        MoveRacoonToLocation();

    }
}
