using NUnit.Framework;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.ProBuilder.MeshOperations;

public class VehicleBase :MonoBehaviour
{
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [SerializeField] protected NavMeshAgent navAgent;
    private float vehicleSpeed => navAgent.speed;
    [SerializeField] protected float detectRadius=2f;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask trafficLayer;
    public bool isStopped;
    private float stopTimer = 5f;
    [SerializeField] private bool isMoving;
    [SerializeField] private List<WaypointConnection> connections = new();
    [SerializeField] protected float detectObjectRange=2f;
    public TrafficManager manager;

    bool isLeftTurn;
    bool isRightTurn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        MoveVehicleToLocation();
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentNode == null)
        {
            StopVehicle();
            Debug.Log("Cant find CurrentNode");
            return;
        }
        //if (!navAgent.hasPath && !navAgent.pathPending)
        //{
        //    manager.RemoveVehicleFromList(this);
        //   // Destroy(this.gameObject);

        //}
        if (isStopped)
        {
            StopVehicle();
        }

        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            ChooseNextDirection(currentNode);
        }

    }

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

    protected virtual void SetMoveToLocation(Waypoint location)
    {
        currentNode = location;
        navAgent.speed = 3.5f;
    }

    //call this to run like wind
    public virtual void MoveVehicleToLocation()
    {
        if (currentNode == null || navAgent == null)
            return;

        navAgent.isStopped = false;
        navAgent.SetDestination(currentNode.transform.position);
        isMoving = true;
    }

    public virtual void StopVehicle()
    {
        if (!navAgent.isStopped)
        {
            stopTimer = 5f; // reset when entering stop
        }

        stopTimer -= Time.deltaTime;

        if (stopTimer <= 0)
        {
            navAgent.isStopped = false;
            isStopped = false;
            isMoving = true;
        }
        else
        {
            navAgent.isStopped = true;
            isMoving = false;
        }
    }

    public virtual void TriggerCollisions()
    {
        if(isStopped)
        {
            StopVehicle();
        }
        HonkHorn();
        navAgent.speed = 2;
        if (navAgent.isStopped)
        {
            isStopped = false;
            isMoving = true;
            MoveVehicleToLocation();
        }
    }



    protected void ChooseNextDirection(Waypoint node)
    {
        if (node == null)
            return;
        Waypoint next;
        next = node.nextWaypoint;
        if (next == null)
        {
            var num = Random.Range(0, 1);
            if (node.branches.Count > 0 && num == 0)
            {
                next = node.branches[0];
                previousNode = currentNode;
                SetMoveToLocation(next);
                MoveVehicleToLocation();
            }
            else return;

        }
        else
        {
            previousNode = currentNode;
            SetMoveToLocation(next);
            MoveVehicleToLocation();
        }

    }

    protected virtual void HonkHorn()
    {
        //add horn SFX/possible headlight VFX? 
        Debug.Log("HONNKKKKKKKKKKKK");
    }


}
