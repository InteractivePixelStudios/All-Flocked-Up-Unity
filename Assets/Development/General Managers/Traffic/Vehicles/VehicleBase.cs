using UnityEngine;
using UnityEngine.AI;
public class VehicleBase :MonoBehaviour
{
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [SerializeField] protected NavMeshAgent navAgent;
    private float vehicleSpeed => navAgent.speed;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask trafficLayer;
    private float stopTimer = 2.5f;
    [SerializeField] private bool isMoving;
    public TrafficManager manager;
    bool isLeftTurn;
    bool isRightTurn;
    private bool stoppedAtLight;
    private bool isBlocked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        StartMove();
    }
    public bool GetNavAgentStopped()
    {
        return navAgent.isStopped;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentNode == null || !navAgent.isOnNavMesh)
        {
            if (navAgent.isOnNavMesh) StopVehicle();
            manager?.RemoveVehicleFromList(this);
            Destroy(gameObject);
            return;
        }
        if (!navAgent.pathPending && navAgent.hasPath && navAgent.remainingDistance <= navAgent.stoppingDistance + 0.1f)
        {
            navAgent.ResetPath();
            ChooseNextDirection(currentNode);
            if (currentNode != null && navAgent.isActiveAndEnabled)
            {
                MoveVehicleToLocation();
            }
            else
            {
                currentNode = null;
                manager?.RemoveVehicleFromList(this);
                Destroy(gameObject);
                return;
            }
        }
        if (navAgent.isStopped && !stoppedAtLight && !isBlocked)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0) StartMove();
        }   
    }

    public void SetStoppedAtLight(bool stopped)
    {
        stoppedAtLight = stopped;
        UpdateStopState();
    }

    public bool GetStoppedAtLight()
    {
        return stoppedAtLight;
    }

    public void SetBlocked(bool blocked)
    {
        isBlocked = blocked;
        UpdateStopState();
    }

    private void UpdateStopState()
    {
        if (stoppedAtLight || isBlocked)
        {
            if (!navAgent.isStopped) StopVehicle();
        }
        else if (navAgent.isStopped)
        {
            StartMove();
        }
    }
    //Anims
    public bool GetIsMoving()
    {
        return isMoving;
    }
    public bool GetIsLeftTurn()
    {
        return isLeftTurn;
    }
    public bool GetIsRightTurn()
    {
        return isRightTurn;
    }
    //Logic
    protected virtual void SetMoveToLocation(Waypoint location)
    {
        currentNode = location;
    }
    //call this to run like wind
    public virtual void MoveVehicleToLocation()
    {
        if (currentNode == null || navAgent == null||stoppedAtLight || isBlocked || !navAgent.isOnNavMesh) return;
        navAgent.speed = 3.5f;
        navAgent.isStopped = false;
        navAgent.SetDestination(currentNode.transform.position);
    }
    public virtual void StopVehicle()
    {
        navAgent.isStopped = true;
        isMoving = false;

    }
    public virtual void StartMove()
    {
        if (stoppedAtLight || isBlocked) return;
        stopTimer = 2.5f;
        isMoving = true;
        MoveVehicleToLocation();
    }

    public virtual void TriggerCollisions()
    {
        HonkHorn();
        navAgent.speed = 2;
        SetBlocked(true);
    }
    protected void ChooseNextDirection(Waypoint node)
    {
        if (node == null)
            return;
        Waypoint next;
        next = node.nextWaypoint;
        if (next == null)
        {
            var num = Random.Range(0, 2);
            if (node.branches.Count > 0 && num == 0)
            {
                next = node.branches[0];
                previousNode = currentNode;
                SetMoveToLocation(next);
                
            }
            else
            {
                manager.RemoveVehicleFromList(this);
                Destroy(this.gameObject);                
            }
        }
        else
        {
            previousNode = currentNode;
            SetMoveToLocation(next);
            
        }
    }
    protected virtual void HonkHorn()
    {
        //add horn SFX/possible headlight VFX? 
        Debug.Log("HONNKKKKKKKKKKKK");
    }
}
