using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AI_Cat : MonoBehaviour, I_EnemyBase
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public GameObject player;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
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
    [SerializeField] private List<WaypointConnection> connections = new();
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [Header("Components")]
    [SerializeField] protected Rigidbody rigidbodyComp;
    [SerializeField] protected Animator animator;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isStopped;
    [SerializeField] protected bool isRetreating;

    private int currentPointIndex = 0;
    private enum EnemyState { Patrolling, Chasing, Swat, Pounce, Stop, Hit, Retreat }
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
        if (swatCooldown >= 0) swatCooldown -= Time.deltaTime;
        if (pounceCooldown >= 0) pounceCooldown -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceToNode = Vector3.Distance(transform.position, currentNode.transform.position);

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
                else if (distanceToPlayer < swatRange)
                    currentState = EnemyState.Swat;
                else if (distanceToPlayer < pounceRange)
                    currentState = EnemyState.Pounce;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Swat:
                if (distanceToPlayer > swatRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Pounce:
                if (distanceToPlayer > pounceRange)
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
                MoveCatToLocation();
                if (distanceToNode < 1f)
                    ChooseNextDirection(currentNode);
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Swat:
                if (swatCooldown <= 0)
                {
                    SwatPlayer();
                    Debug.Log("SwatCalled");
                    swatCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Pounce:
                if (pounceCooldown <= 0)
                {
                    Pounce();
                    Debug.Log("PounceCalled");
                    pounceCooldown = 3f;
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


    private void FindWaypoints()
    {
        var waypointsArray = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        foreach (var waypoint in waypointsArray)
        {
            if (waypoint.CompareTag("Cat"))
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
        rigidbodyComp.linearVelocity = Vector3.zero;
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
        rigidbodyComp.MovePosition(randomPosition);
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

        var spawnedCollider = swatColliderParent.AddComponent<SphereCollider>();
        var comp = spawnedCollider.AddComponent<KickComponent>();
        comp.damage = 1;
        //animator.SetTrigger("isKicking");
        swatCooldown = 3f;
        await Task.Delay(3000);
        Destroy(spawnedCollider);
        Destroy(comp);
    }

    protected void Pounce()
    {
        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        Vector3 dirToPlayer = (player.transform.position-transform.position).normalized;
        dirToPlayer.y = 0;
        Vector3 force = dirToPlayer * pounceForce.z + Vector3.up * pounceForce.y;
        rigidbodyComp.AddForce(force,ForceMode.Impulse);
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
        MoveCatToLocation();

    }
}
