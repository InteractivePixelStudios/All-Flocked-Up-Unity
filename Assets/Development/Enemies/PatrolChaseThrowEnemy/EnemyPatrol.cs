using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(Enemy_AlertIcon))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : EnemyBaseComponent
{
    [Header("Patrol")]
    public GameObject patrolPoint;
    public GameObject player;
    [SerializeField] private float distanceToPlayer;
    public PlayerStealthSystem playerStealth;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float retreatSpeed = 8f;
    float moveAfterCooldown = 1.5f;
    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseSightRange = 8f;
    [Header("Kick")]
    public float kickRange = 1f;
    public float kickCooldown = 3f;
    [SerializeField] protected SphereCollider kickCollider;
    [SerializeField] private GameObject kickColliderParent;
    bool canKick;
    bool isKicking;
    [Header("Throw")]
    public float throwRange=3f;
    public float throwForce = 10f;
    public float throwCooldown = 3f;
    bool canThrow;
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
    bool hitActive;
    [SerializeField]ReactionState currentReactionState;
    public enum EnemyState { Patrolling, Chasing, Kicking, Throwing,Stop,Hit,Retreat }
    [SerializeField]private EnemyState currentState = EnemyState.Patrolling;

    public bool IsDead = false;
    [SerializeField] private GameObject vfxSpawnLoc;
    public GameObject GetVFXSpawn()
    {
        Debug.Log("GotVFXSpawn");
        return vfxSpawnLoc;
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerGroundMovement>().gameObject;
        if (player != null)
        {
            playerStealth = player.GetComponent<PlayerStealthSystem>();
        }
        animController = GetComponent<Animator>();
        alertIcon = GetComponent<Enemy_AlertIcon>();
        FindWaypoints();
        currentState = EnemyState.Patrolling;
        var rand = Random.Range(0, 1);
        if (holdList != null && holdList.Count >0 && Random.Range(0,2) ==0) {  SpawnHeldItem(); }
        if(navAgent != null) navAgent.updatePosition = true;
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
        if(navAgent == null || !navAgent.isOnNavMesh) return;
        kickCooldown = Mathf.Max(0, kickCooldown - Time.deltaTime);
        throwCooldown = Mathf.Max(0, throwCooldown - Time.deltaTime);
        moveAfterCooldown = Mathf.Max(0, moveAfterCooldown - Time.deltaTime);

        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            canSeePlayer = true;
            if (alertIcon!= null && !iconActive)
            {
                alertIcon.SetPlayerSeen(true);
                iconActive = true;
            }
        }
        else
        {
            canSeePlayer = false;
            if (alertIcon != null && iconActive)
            {
                alertIcon.SetPlayerSeen(false);
                iconActive = false;
            }
            SetCurrentState(EnemyState.Patrolling);
       }
        canKick = distanceToPlayer <= kickRange && kickCooldown <= 0f;
        canThrow = distanceToPlayer <= throwRange && throwCooldown <= 0f;

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (isHit) currentState = EnemyState.Hit; 
                else if (isIdleStart)  currentState = EnemyState.Stop; 
                else if (playerStealth!=null && playerStealth.GetStealth() < 10 && canSeePlayer)  currentState = EnemyState.Chasing; 
                break;
            case EnemyState.Chasing:
                if (isHit) currentState = EnemyState.Hit; 
                else if (!canSeePlayer)  currentState = EnemyState.Patrolling; 
                else if (!isKicking && !isThrowing && moveAfterCooldown <=0f) 
                { 
                    if(canKick) currentState = EnemyState.Kicking; 
                    else if (canThrow && !isHoldingItem) currentState = EnemyState.Throwing;
                }
                break;
            case EnemyState.Kicking:
                if (isHit) { currentState = EnemyState.Hit; }
                else if (!isKicking && moveAfterCooldown <= 0) currentState = EnemyState.Chasing;
                else if(!canSeePlayer &&!isKicking) currentState = EnemyState.Patrolling;
                break;
            case EnemyState.Throwing:
                if (isHit) { currentState = EnemyState.Hit; }
                else if (!isThrowing && moveAfterCooldown <= 0) currentState = EnemyState.Chasing;
                else if (!canSeePlayer && !isThrowing) currentState = EnemyState.Patrolling;
                break;
            case EnemyState.Stop:
                if (isHit) { currentState = EnemyState.Retreat;}
                break;
            case EnemyState.Retreat:
                if (isHit) { currentState = EnemyState.Hit; }
                break;
            case EnemyState.Hit:
                if (!isHit && hitActive){ currentState = EnemyState.Retreat; }
                break;

    }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (!isStopped)
                {
                    MoveHumanToLocation();
                    if (navAgent.remainingDistance <= 1f && !navAgent.pathPending && navAgent.hasPath)
                    {
                        ChooseNextDirection(currentNode);
                    }
                }
                else { StopMove(); }
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Kicking: // throws until kick anim done
                if (!isKicking && canKick)
                {
                    ThrowObject();
                }
                break;
            case EnemyState.Throwing:
                if (!isThrowing&&canThrow) 
                {
                    ThrowObject();
                }
                break;
            case EnemyState.Stop:
                StopMove();
                break;
            case EnemyState.Hit:
                if (!hitActive)
                {
                    HitReact();
                }
                break;
            case EnemyState.Retreat:
                 Retreat();
                break;
        }
    }

    private void FindWaypoints()
    {
        if (patrolPoint == null) return;
        if(waypoints == null) waypoints = new List<Waypoint>();
        waypoints.Clear();
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
        if (waypoints.Count == 0 || waypoints == null) return;
        if (waypoints.Count == 1) 
        {
            //StopMove();
            currentNode = waypoints[0];
        }
        var randomIndex = Random.Range(0, waypoints.Count);
        currentNode = waypoints[randomIndex];

    }

    protected void StopMove()
    {
        isStopped = true;
        navAgent.isStopped = true;
        animController.SetFloat("Speed", 0f);
    }

    protected async void HitReact()
    {
        if (hitActive) return;
        CallHit();
        await Task.Delay(750);
        isHit = false;
        currentState = EnemyState.Retreat;
    }

    protected void CallHit()
    {
        StopMove();
        isHit = true;
        if (isHoldingItem && currentHeldObject != null) { DropHeldItem(); }
    }

    public override void OnHit(PoopType type)
    {
        currentReactionState = type.poopReaction;
        switch (currentReactionState)
        {
            // separate if wanting to add different react anims
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
        currentState = EnemyState.Hit;
        HitReact();
    }
    protected void Retreat()
    {
        isHit = false;
        isStopped = false;
        navAgent.isStopped = false;
        navAgent.speed = retreatSpeed;
        isRetreating = true;
        animController.SetFloat("Speed", navAgent.velocity.magnitude);
        if (currentNode != null)
        {
            navAgent.SetDestination(currentNode.transform.position);
            if (navAgent.remainingDistance <= 1f)
            {
                isRetreating = false;
                currentState = EnemyState.Patrolling;
            }
        }
        else
        {
            navAgent.SetDestination(transform.position + new Vector3(0, 0, 5));

            if (!navAgent.pathPending && navAgent.hasPath && navAgent.remainingDistance <= 1f)
            {
                if (Physics.Raycast(transform.position, Vector3.forward, 2f, LayerMask.GetMask("PropBuilding")))
                {
                    isRetreating = false;
                    isStopped = true;
                    currentState = EnemyState.Patrolling;
                    return;
                }
                isRetreating = false;
                currentState = EnemyState.Patrolling;

            }
        }
    }

   protected void ChasePlayer()
    {
        if (isKicking || isThrowing || !canSeePlayer) return;
        isStopped = false;
        navAgent.isStopped = false;
        navAgent.SetDestination(player.transform.position);
        animController.SetFloat("Speed", navAgent.velocity.magnitude);
    }

    protected async void KickPlayer()
    {
        if (isKicking || isThrowing || !canSeePlayer) return;
        StopMove();
        isKicking = true;
        if (kickColliderParent != null)
        {
            var spawnedCollider = kickColliderParent.AddComponent<SphereCollider>();
            var comp = spawnedCollider.AddComponent<KickComponent>();
            comp.damage = 1;

            animController.SetTrigger("isKicking");
            kickCooldown = 3f;
            await Task.Delay(3000);
            Destroy(spawnedCollider);
            Destroy(comp);
        }
        isKicking = false;
        moveAfterCooldown = 1.5f;
        isStopped = false;

    }

    protected async void ThrowObject()
    {
        if (isThrowing || isHoldingItem || isKicking || !canSeePlayer) return;
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
        moveAfterCooldown = 1.5f;
        isStopped = false;
    }

    protected void SetThrowPoint()
    {
        Vector3 targetPos = player.transform.position;
        targetPos.y = objectSpawnPoint.position.y;
        Vector3 dir = (targetPos - objectSpawnPoint.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            objectSpawnPoint.forward = dir;

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
            navAgent.isStopped = false;
            navAgent.speed = patrolSpeed;
            navAgent.SetDestination(currentNode.transform.position);
        }
        else
        {
            var found = FindObjectsByType<Waypoint>();
            foreach (var obj in found)
            {
            if (obj.gameObject.CompareTag("Human") || obj == null) continue;

                patrolPoint = obj.gameObject;
                FindWaypoints();
                navAgent.isStopped = false;
                if (currentNode != null)
                {
                    navAgent.SetDestination(currentNode.transform.position);
                }
                break;
            }
                
            
        }
        animController.SetFloat("Speed", navAgent.velocity.magnitude);

    }

    protected void ChooseNextDirection(Waypoint node)
    {
        if(node == null) return;
        if (node.nextWaypoint == null)
        {
            FindRandomWaypoint();
            return;
        }      
        Waypoint nextNode = node.nextWaypoint;
        if (nextNode == null)return;
        previousNode = currentNode;
        SetMoveToLocation(nextNode);
        MoveHumanToLocation();       
        locationSet = true;
    }

    void SpawnHeldItem()
    {
        if (holdList == null || holdList.Count == 0 || objectSpawnPoint == null) return;
        var rand = Random.Range(0, holdList.Count);
        if (holdList[rand] == null) return;

        var spawned = Instantiate(holdList[rand]);
        currentHeldObject = spawned;
        spawned.GetComponent<Rigidbody>().isKinematic = true;
        spawned.transform.SetParent(objectSpawnPoint, false);
        spawned.transform.position = objectSpawnPoint.position;
        spawned.GetComponentInChildren<ParticleSystem>().Stop();
        isHoldingItem = true;

    }

    void DropHeldItem()
    {
        currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
        currentHeldObject.transform.SetParent(null, true);
        currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
        currentHeldObject = null;
        objectSpawnPoint.transform.DetachChildren();
        isHoldingItem = false;
    }
}
