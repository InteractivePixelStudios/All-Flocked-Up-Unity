using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WaypointNode : MonoBehaviour
{
    public LayerMask trafficWaypoints;
    public List<WaypointConnection> connections = new List<WaypointConnection>();

    private static readonly Vector3 direction = Vector3.forward;

    

    private void Awake()
    {

        
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
