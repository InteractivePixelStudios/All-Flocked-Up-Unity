using System.Collections.Generic;
using System.Linq.Expressions;
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
    float MoveAfterCooldown = 1.5f;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    [Header("Kick")]
    public float kickRange = 1f;
    public float kickCooldown = 3f;
    [SerializeField] protected SphereCollider kickCollider;
    [SerializeField] private GameObject kickColliderParent;
    bool isKicking;
    [Header("Throw")]
    public float throwRange=3f;
    public float throwForce = 10f;
    public float throwCooldown = 3f;
    bool isThrowing;
    [SerializeField] private GameObject throwObjectPrefab;
    [SerializeField] private Transform objectSpawnPoint;
    [Header("HeldObject")]
    [SerializeField] private GameObject currentHeldObject;
    [SerializeField] private List<GameObject> holdList = new();
    [SerializeField] private bool isHoldingItem;
    [Header("Waypoints")]
    [SerializeField] private List<Waypoint> waypoints;
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [Header("Components")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected Animator animController;
    [SerializeField] protected Enemy_AlertIcon alertIcon;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isStopped;
    [SerializeField] protected bool isRetreating;
    [SerializeField] protected bool isIdleStart;
    [SerializeField]bool canSeePlayer;
    bool iconActive;
    bool locationSet;
    [SerializeField]ReactionState currentReactionState;
    private int currentPointIndex = 0;
    public enum EnemyState { Patrolling, Chasing, Kicking, Throwing,Stop,Hit,Retreat }
    private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;

    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        playerStealth = player.GetComponent<PlayerStealthSystem>();
        animController = GetComponent<Animator>();
        alertIcon = GetComponent<Enemy_AlertIcon>();
        FindWaypoints();
        currentState = EnemyState.Patrolling;
        var rand = Random.Range(0, 1);
        if (rand == 0) {  SpawnHeldItem(); } else return;
    }

    public  void SetIsHit()
    {
        Debug.Log("setIsHit");
        isHit = true;
        isHit = false;
    }
    public void SetCurrentState(EnemyState state)
    {
        currentState = state;
    }

    void SetNavAgentDestination(Vector3 pos, ReactionState state)
    {
        navAgent.SetDestination(pos);
        currentReactionState = state;
    }

    void Update()
    {
        navAgent.updatePosition = true;
        if(kickCooldown>=0) kickCooldown -= Time.deltaTime;
        if(throwCooldown>=0)throwCooldown -= Time.deltaTime;
        if(MoveAfterCooldown>=0) MoveAfterCooldown -= Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < detectionRange)
        {
            canSeePlayer = true;
            if (!iconActive)
            {
                alertIcon.SetPlayerSeen(true);
                iconActive = true;
            }
        }
        else
        {
            canSeePlayer = false;
            if (iconActive)
            {
                alertIcon.SetPlayerSeen(false);
                iconActive = false;
            }
        }
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (this.isHit) currentState = EnemyState.Hit;
                if (this.isIdleStart && !this.isHit) currentState = EnemyState.Stop;
                else if (playerStealth.GetStealth() < 10 && canSeePlayer && !this.isHit)
                    currentState = EnemyState.Chasing;

                break;

            case EnemyState.Chasing:
                if (this.isHit)
                    currentState = EnemyState.Hit;
                else if (!canSeePlayer)
                    currentState = EnemyState.Patrolling;
                else if (!isKicking && !isThrowing && distanceToPlayer < kickRange)
                    currentState = EnemyState.Kicking;
                else if (!isHoldingItem &&!isKicking && !isThrowing && distanceToPlayer < throwRange)
                    currentState = EnemyState.Throwing;
                break;

            case EnemyState.Kicking:
                if (this.isHit)
                    currentState = EnemyState.Hit;
                if (distanceToPlayer > kickRange && !isHit)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Throwing:
                if (this.isHit)
                    currentState = EnemyState.Hit;
                if (distanceToPlayer > throwRange && !this.isHit)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Stop:
                if (this.isHit)
                {
                    isStopped = false;
                    currentState = EnemyState.Retreat;

                }
                else if (distanceToPlayer > throwRange && !this.isHit)
                { currentState = EnemyState.Chasing; }
                 else
                isStopped = true;
                break;

            case EnemyState.Retreat:
                if (this.isHit)
                    currentState = EnemyState.Hit;
                else if (isStopped && !this.isHit)
                {
                    isStopped = false;
                    currentState = EnemyState.Patrolling;
                }
                break;

            case EnemyState.Hit:
                this.isHit = true;
                currentState = EnemyState.Retreat;
                break;

    }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                //Debug.Log("Patrol");
                if (locationSet)
                {
                    MoveHumanToLocation();
                    if(navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
                    {
                        locationSet = false;
                    }
                }

                if (!locationSet)
                {
                    if (waypoints.Count > 1)
                    {
                        ChooseNextDirection(currentNode);
                    }
                    else { StopMove(); }
                    
                }
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Kicking: // throws until kick anim done
                if (throwCooldown <= 0 && !isKicking)
                {
                    ThrowObject();
                    throwCooldown = 3f;
                    MoveAfterCooldown = 1.5f;
                    if (MoveAfterCooldown <= 0)
                    {
                        currentState = EnemyState.Chasing;
                    }
                    isKicking = true;
                }
                //if (kickCooldown <= 0)
                //{
                //    KickPlayer();

                //    kickCooldown = 3f;
                //    currentState = EnemyState.Chasing;
                //}
                break;
            case EnemyState.Throwing:
                if (throwCooldown <= 0 && !isThrowing)
                {
                    ThrowObject();
                    throwCooldown = 3f;
                    MoveAfterCooldown = 1.5f;
                    if (MoveAfterCooldown <= 0)
                    {
                        currentState = EnemyState.Chasing;
                    }
                    isThrowing = true;
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

    }


    private void FindRandomWaypoint()
    {
        if (waypoints.Count == 0) return;
        if (waypoints.Count == 1) { StopMove(); }
        if(waypoints.Count < 2)
        {
            var randomIndex = Random.Range(0, waypoints.Count - 1);
            this.currentNode = waypoints[randomIndex];
        }
        else
        {
            this.currentNode = waypoints[0];
        }

    }

    protected void StopMove()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        animController.SetFloat("Speed", 0f);
    }

    protected  void HitReact()
    {
        
        isHit = false;
        if(isHoldingItem && currentHeldObject!=null) { DropHeldItem(); }
        currentState = EnemyState.Retreat;
    }

    //ReactionState GetReactionState(PoopType type)
    //{
    //    switch (type)
    //    {
    //        case Poop
    //    }
    //}
    public override void OnHit(PoopType type)
    {
        currentReactionState = type.poopReaction;
        switch (currentReactionState)
        {
            case ReactionState.Normal:
                animController.SetTrigger("isHit");
                Debug.Log("Hit by Normal ");
                break;
            case ReactionState.Fire:
                animController.SetTrigger("isHit");
                Debug.Log("Hit by Fire");
                break;
            case ReactionState.Confetti:
                animController.SetTrigger("isHit");
                Debug.Log("Hit by Confetti");
                break;
            case ReactionState.Glow:
                animController.SetTrigger("isHit");
                Debug.Log("Hit by Glow");
                break;
        }
        isHit = true;
        Debug.Log("HitHuman");
        SetCurrentState(EnemyState.Hit);
    }
    protected void Retreat()
    {
        navAgent.isStopped = false;
        animController.SetFloat("Speed",navAgent.speed);
        Debug.Log("retreating");
        bool set = false;
        if (currentNode != null)
        {
            navAgent.SetDestination(currentNode.transform.position);
            set = true;
            if (navAgent.remainingDistance <= 1f)
            {

                isRetreating = false;
                isStopped = true;
            }
        }
        else
        {
            if (!set)
            {
                navAgent.SetDestination(transform.position + new Vector3(0, 0, 5));
                set = true;
            }
            if(navAgent.remainingDistance <= 1f)
            {
                if(Physics.Raycast(transform.position,Vector3.forward, 2f, LayerMask.NameToLayer("PropBuilding")))
                {
                    isRetreating = false;
                    isStopped = true;
                }else 
                isRetreating = false;
                isStopped = true;
            }
        }
    }

   protected void ChasePlayer()
    {
        if (isKicking || isThrowing || !canSeePlayer) return;
        animController.SetFloat("Speed", navAgent.speed);
        Vector3 targetPos = player.transform.position;
        targetPos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    protected async void KickPlayer()
    {
        if (!isKicking || isThrowing || !canSeePlayer) return;
        isKicking = true;
        StopMove();
        var spawnedCollider = kickColliderParent.AddComponent<SphereCollider>();
        var comp = spawnedCollider.AddComponent<KickComponent>();
        comp.damage = 1;
        animController.SetTrigger("isKicking");
        kickCooldown = 3f;
        await Task.Delay(3000);
        Destroy(spawnedCollider);
        Destroy(comp);
        isKicking = false;

    }

    protected async void ThrowObject()
    {
        if (isHoldingItem || isKicking || isThrowing || !canSeePlayer) return;
        isThrowing = true;
        StopMove();
        Vector3 facingDir = (player.transform.position - transform.position).normalized;
        float diff = Vector3.Dot(transform.forward, facingDir);
        if(diff <0.5f)
        {
            isThrowing = false;
            return;
        }
        animController.SetTrigger("isThrowing");
        await Task.Delay(1200);
        var spawnedObj = Instantiate(throwObjectPrefab,objectSpawnPoint.position,objectSpawnPoint.rotation);
        spawnedObj.transform.position = objectSpawnPoint.transform.position;
        spawnedObj.transform.rotation = objectSpawnPoint.transform.rotation;
        var objRB = spawnedObj.GetComponent<Rigidbody>();
        SetThrowPoint();
        objRB.AddForce((objectSpawnPoint.forward+(Vector3.down*0.5f))*throwForce,ForceMode.Impulse);
        throwCooldown = 3f;
        isThrowing = false;

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
        if (navAgent == null)return;
        if (currentNode != null)
        {
            //Debug.Log("Moving to: " + currentNode);
            navAgent.isStopped = false;
            FindWaypoints();
            navAgent.SetDestination(currentNode.transform.position);
        }
        else
        {
            var found = FindObjectsByType<Waypoint>();
            foreach (var obj in found)
            {
                if (obj.gameObject.CompareTag("Human"))
                {
                    if (obj != null)
                    {
                        patrolPoint = obj.gameObject;
                        FindWaypoints();
                        //Debug.Log("Moving to: " + currentNode);
                        navAgent.isStopped = false;
                        navAgent.SetDestination(currentNode.transform.position);
                        break;
                    }
                }
            }
        }
        animController.SetFloat("Speed", navAgent.speed);

    }

    public virtual void StopHuman()
    {
        navAgent.isStopped = true;
        //Debug.Log("Stopping");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Poop"))
        {
            var type = collision.gameObject.GetComponent<PoopProjectile>().GetPoopType();
            TakeDamage(1,type);
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
        locationSet = true;

    }

    void SpawnHeldItem()
    {
        var rand = Random.Range(0, holdList.Count);
        var spawned = Instantiate(holdList[rand]);
        currentHeldObject = spawned;
        spawned.GetComponent<Rigidbody>().isKinematic = true;
        spawned.transform.SetParent(objectSpawnPoint, false);
        spawned.transform.position = objectSpawnPoint.transform.position;
        spawned.GetComponent<ParticleSystem>().Stop();
        isHoldingItem = true;

    }

    void DropHeldItem()
    {
        currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
        objectSpawnPoint.transform.DetachChildren();
        isHoldingItem = false;
    }
}
