#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;
    public List<Waypoint> branches = new List<Waypoint>();
    public List<WaypointConnection> connections = new List<WaypointConnection>();

    public float width = 1f;

    public Vector3 GetPosition()
    {
        Vector3 min = transform.position + transform.right * width / 2f;
        Vector3 max = transform.position - transform.right * width / 2f;
        return Vector3.Lerp(min,max,Random.Range(0,1f));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (nextWaypoint != null)
            Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);

        Gizmos.color = Color.cyan;
        foreach (var branch in branches)
        {
            if (branch != null)
                Gizmos.DrawLine(transform.position, branch.transform.position);
        }


#if UNITY_EDITOR
        if(nextWaypoint != null)
        {
            float size = 5f;
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(size, transform.position, nextWaypoint.transform.position);

            Vector3 arrowDirection = (nextWaypoint.transform.position - transform.position).normalized;
            float arrowSize = 0.5f;

            Vector3 rightArm = Quaternion.LookRotation(arrowDirection) * Quaternion.Euler(0, 120, 0) * Vector3.forward;
            Vector3 leftArm = Quaternion.LookRotation(arrowDirection) * Quaternion.Euler(0, -120, 0) * Vector3.forward;

            Handles.DrawLine(nextWaypoint.transform.position, nextWaypoint.transform.position + rightArm * arrowSize);
            Handles.DrawLine(nextWaypoint.transform.position, nextWaypoint.transform.position + leftArm * arrowSize);
        }
#endif
    }
}
