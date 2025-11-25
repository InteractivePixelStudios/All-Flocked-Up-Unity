using UnityEngine;
using UnityEngine.AI;

public class AIMovePoints : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] points;
    private int currentIndex = 0;

    [Header("Movement")]
    public float stoppingDistance = 0.5f;
    public float speed = 3.5f;
    private NavMeshAgent agent;
    public float moveSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (points.Length > 0)
        {
            MoveToNextPoint();
        }
        else
        {
            Debug.LogWarning("No points assigned to AIMovePoints on " + gameObject.name);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            MoveToNextPoint();
        }
        moveSpeed = agent.velocity.magnitude;
    }

    void MoveToNextPoint()
    {
        if (points.Length == 0) return;

        agent.SetDestination(points[currentIndex].position);
        agent.speed = speed;

        currentIndex = (currentIndex + 1) % points.Length;
    }
}
