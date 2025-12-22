using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AI_Dog : MonoBehaviour, I_EnemyBase
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public GameObject player;
    private PlayerStealthSystem playerStealth;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    [Header("Bite")]
    public float biteRange = 1f;
    public float biteCooldown = 3f;
    [SerializeField] protected SphereCollider biteCollider;
    [SerializeField] private GameObject biteColliderParent;
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
    private enum EnemyState { Patrolling, Chasing, Bite, Stop, Hit, Retreat }
    private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
        playerStealth = player.GetComponent<PlayerStealthSystem>();
        animator = GetComponent<Animator>();
        FindWaypoints();
    }

    void Update()
    {
        if (biteCooldown >= 0) biteCooldown -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (playerStealth.GetStealth() < 10 && distanceToPlayer < detectionRange)
                    currentState = EnemyState.Chasing;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Patrolling;
                else if (distanceToPlayer < biteRange)
                    currentState = EnemyState.Bite;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Bite:
                if (distanceToPlayer > biteRange)
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
                MoveDogToLocation();
                if (navAgent.remainingDistance < 5f)
                    ChooseNextDirection(currentNode);
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Bite:
                if (biteCooldown <= 0)
                {
                    BitePlayer();
                    Debug.Log("BiteCalled");
                    biteCooldown = 3f;
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
            if (waypoint.CompareTag("Dog"))
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
        TakeDamage(1);
        animator.SetTrigger("isHit");
        await Task.Delay(3000);
    }

    protected void Retreat()
    {
        var centerPoint = transform.position;
        var radius = 5f;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        Vector3 randomPosition = centerPoint + randomDirection;
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

    protected async void BitePlayer()
    {

        var spawnedCollider = biteColliderParent.AddComponent<SphereCollider>();
        var comp = spawnedCollider.AddComponent<KickComponent>();
        comp.damage = 1;
        //animator.SetTrigger("isBiting");
        biteCooldown = 3f;
        await Task.Delay(3000);
        Destroy(spawnedCollider);
        Destroy(comp);
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
    public virtual void MoveDogToLocation()
    {
        if (currentNode == null || navAgent == null)
            return;

        navAgent.isStopped = false;
        navAgent.SetDestination(currentNode.transform.position);

    }

    public virtual void StopVehicle()
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
        MoveDogToLocation();

    }
}
