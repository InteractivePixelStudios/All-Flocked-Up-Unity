using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WaypointNode : MonoBehaviour
{
    public LayerMask trafficWaypoints;
    public List<WaypointConnection> connections = new List<WaypointConnection>();

    private static readonly Vector3[] directions =
{
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        (Vector3.forward + Vector3.left).normalized,
        (Vector3.forward + Vector3.right).normalized,
        (Vector3.back + Vector3.left).normalized,
        (Vector3.back + Vector3.right).normalized
    };

    private void Awake()
    {
        connections.Clear();

        foreach (var dir in directions)
        {
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 20f, trafficWaypoints))
            {
                var hitNode = hit.collider.GetComponent<Waypoint>();
                if (hitNode != null && hitNode != this)
                {
                    var connection = new WaypointConnection
                    {
                        node = hitNode,
                        cost = hit.distance
                    };
                    connections.Add(connection);

                    Debug.DrawLine(transform.position, hitNode.transform.position, Color.cyan, 2f);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);

        Gizmos.color = Color.cyan;
        foreach (var connection in connections)
        {
            if (connection.node != null)
                Gizmos.DrawLine(transform.position, connection.node.transform.position);
        }
    }
}
