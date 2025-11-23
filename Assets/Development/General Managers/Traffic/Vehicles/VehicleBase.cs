using NUnit.Framework;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class VehicleBase :MonoBehaviour
{
    public Waypoint currentNode;
    [SerializeField] private Waypoint previousNode;
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] private float vehicleSpeed;
    [SerializeField] protected float detectRadius=2f;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask trafficLayer;
    public bool isStopped;
    [SerializeField] private bool isMoving;
    [SerializeField] private List<WaypointConnection> connections = new();
    [SerializeField] protected float detectObjectRange=2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        MoveVehicleToLocation();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isStopped)
        {
            StopVehicle();
        }else if (!isStopped)
        {
            MoveVehicleToLocation();
        }
        CheckForCollisions();

        if (currentNode == null)
        {
            StopVehicle();
            return;
        }

        if (navAgent.remainingDistance < 5f)
        {
            ChooseNextDirection(currentNode);
        }

    }

    protected virtual void SetMoveToLocation(Waypoint location)
    {
        currentNode = location;
    }

    //call this to run like wind
    public virtual void MoveVehicleToLocation()
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

    protected virtual void CheckForCollisions()
    {
        RaycastHit hit;
        int combinedMask = trafficLayer | playerLayer | enemyLayer;

        if (Physics.Raycast(transform.position, transform.forward, out hit, detectObjectRange, combinedMask))
        {
            StopVehicle();
            HonkHorn();
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
        }
        else
        {
            if (!navAgent.isStopped)
                MoveVehicleToLocation();
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
        else Destroy(this.gameObject);
        
        int randomIndex = Random.Range(0, connections.Count);
        Waypoint nextNode = connections[randomIndex].node;
        if (nextNode == null)
            return;
        previousNode = currentNode;
        SetMoveToLocation(nextNode);
        MoveVehicleToLocation();
        
    }

    protected virtual void HonkHorn()
    {
        //add horn SFX/possible headlight VFX? 
    }
}
