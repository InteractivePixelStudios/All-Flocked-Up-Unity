using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : EnemyBaseComponent
{
    [Header("Patrol")]
    public GameObject patrolPoint;
    public GameObject player;
    public PlayerStealthSystem playerStealth;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    [Header("Kick")]
    public float kickRange = 1f;
    public float kickCooldown = 3f;
    [SerializeField] protected SphereCollider kickCollider;
    [SerializeField] private GameObject kickColliderParent;
    [Header("Throw")]
    public float throwRange=3f;
    public float throwForce = 10f;
    public float throwCooldown = 3f;
    [SerializeField] private GameObject throwObjectPrefab;
    [SerializeField] private Transform objectSpawnPoint;
    [Header("Waypoints")]
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField] private List<WaypointConnection> connections = new();
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [Header("Components")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected Animator animController;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isStopped;
    [SerializeField] protected bool isRetreating;

    private int currentPointIndex = 0;
    public enum EnemyState { Patrolling, Chasing, Kicking, Throwing,Stop,Hit,Retreat }
    private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;

    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        playerStealth = player.GetComponent<PlayerStealthSystem>();
        animController = GetComponent<Animator>();
        FindWaypoints();
        currentState = EnemyState.Patrolling;
    }
    public void SetCurrentState(EnemyState state)
    {
        currentState = state;
    }

    void Update()
    {
        if(kickCooldown>=0) kickCooldown -= Time.deltaTime;
        if(throwCooldown>=0)throwCooldown -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState) 
        {
            case EnemyState.Patrolling:
                if (playerStealth.GetStealth() < 10&& distanceToPlayer < detectionRange && !isHit)
                    currentState = EnemyState.Chasing;
                else if (isHit)
                    currentState = EnemyState.Hit;
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer > detectionRange && !isHit)
                    currentState = EnemyState.Patrolling;
                else if (distanceToPlayer < kickRange && !isHit)
                    currentState = EnemyState.Kicking;
                else if(distanceToPlayer < throwRange && !isHit)
                    currentState = EnemyState.Throwing;
                else if(isHit)
                    currentState = EnemyState.Hit;
                break; 

            case EnemyState.Kicking:
                if (distanceToPlayer > kickRange && !isHit)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Throwing:
                if(distanceToPlayer > throwRange && !isHit)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Stop:
                if (isHit)
                {
                    currentState = EnemyState.Retreat;
                    isStopped = true;

                }
                break;

            case EnemyState.Retreat:
                if (isStopped && !isHit)
                {
                    isStopped = false;
                    currentState = EnemyState.Hit;
                }
                else if (!isHit)
                {
                    currentState = EnemyState.Patrolling;
                }
                break;

            case EnemyState.Hit:
                isHit = true;
                break;

    }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                MoveHumanToLocation();
                if (navAgent.remainingDistance < 5f)
                    ChooseNextDirection(currentNode);
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Kicking:
                if (kickCooldown <= 0)
                {
                    KickPlayer();
                    Debug.Log("KickCalled");
                    kickCooldown = 3f;
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Throwing:
                if (throwCooldown <= 0)
                {
                    ThrowObject();
                    Debug.Log("ThrowCalled");
                    throwCooldown = 3f;
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
                if (!isRetreating)
                {
                    isRetreating = true;
                    Retreat();
                    Debug.Log("RetreatCalled");
                }
                break;
        }

    }

    private void FindWaypoints()
    {
        var waypointArray = patrolPoint.GetComponentsInChildren<Waypoint>();
        foreach (var waypoint in waypointArray)
        {
            if (waypoint.CompareTag("Human"))
            {
                waypoints.Add(waypoint);
            }

        }
        FindRandomWaypoint();
        Debug.Log("CheckforWaypoints");
    }


    private void FindRandomWaypoint()
    {
        if (waypoints.Count == 0) return;
       // var randomIndex = Random.Range(0, waypoints.Count);
        this.currentNode = waypoints[1];     
        Debug.Log("H");
    }




    //void Patrol()
    //{
    //    if (patrolPoints.Length == 0) return;

    //    Vector3 targetPos = patrolPoints[currentPointIndex].position;
    //    targetPos.y = transform.position.y;

    //    transform.position = Vector3.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);

    //    Vector3 dir = (targetPos - transform.position).normalized;
    //    dir.y = 0;
    //    if (dir != Vector3.zero)
    //        transform.forward = dir;

    //    if (Vector3.Distance(transform.position, targetPos) < 0.2f)
    //    {
    //        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    //    }
    //}
    protected void StopMove()
    {
        navAgent.isStopped = true;
        animController.SetFloat("Speed", 0f);
    }

    protected async void HitReact()
    {
        currentState = EnemyState.Stop;
        animController.SetTrigger("isHit");
        await Task.Delay(1000);
    }

    protected void Retreat()
    {
        animController.SetFloat("Speed",navAgent.speed);
        var centerPoint = transform.position;
        var radius = 5f;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        Vector3 randomPosition = centerPoint + randomDirection;
        navAgent.SetDestination(randomPosition);
    }

   protected void ChasePlayer()
    {
        animController.SetFloat("Speed", navAgent.speed);
        Vector3 targetPos = player.transform.position;
        targetPos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    protected void KickPlayer()
    {
        var spawnedCollider = kickColliderParent.AddComponent<SphereCollider>();
        var comp = spawnedCollider.AddComponent<KickComponent>();
        comp.damage = 1;
        animController.SetTrigger("isKicking");
        kickCooldown = 3f;
        Task.Delay(3000);
        Destroy(spawnedCollider);
        Destroy(comp);

    }

    protected void ThrowObject()
    {
        animController.SetTrigger("isThrowing");
        var spawnedObj = Instantiate(throwObjectPrefab,objectSpawnPoint.position,objectSpawnPoint.rotation);
        spawnedObj.transform.position = objectSpawnPoint.transform.position;
        spawnedObj.transform.rotation = objectSpawnPoint.transform.rotation;
        var objRB = spawnedObj.GetComponent<Rigidbody>();
        SetThrowPoint();
        objRB.AddForce(objectSpawnPoint.forward*throwForce,ForceMode.Impulse);
        throwCooldown = 3f;

    }

    protected void SetThrowPoint()
    {
        Vector3 targetPos = player.transform.position;
        targetPos.y = objectSpawnPoint.transform.position.y;
        Vector3 dir = (targetPos - objectSpawnPoint.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            objectSpawnPoint.transform.forward = dir;

    }

    protected virtual void SetMoveToLocation(Waypoint location)
    {
        currentNode = location;
    }

    //call this to run like wind
    public virtual void MoveHumanToLocation()
    {
        if (currentNode == null || navAgent == null)
            return;

        navAgent.isStopped = false;
        navAgent.SetDestination(currentNode.transform.position);
        animController.SetFloat("Speed",navAgent.speed);

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
    //protected virtual void CheckForCollisions()
    //{
    //    RaycastHit hit;
    //    int combinedMask = trafficLayer | playerLayer | enemyLayer;

    //    if (Physics.Raycast(transform.position, transform.forward, out hit, detectObjectRange, combinedMask))
    //    {
    //        StopVehicle();
    //        Debug.DrawLine(transform.position, hit.point, Color.yellow);
    //    }
    //    else
    //    {
    //        if (!navAgent.isStopped)
    //            MoveVehicleToLocation();
    //    }
    //}



    protected void ChooseNextDirection(Waypoint node)
    {
        if(node == null) return;

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
            MoveHumanToLocation();
        }
    }
}
