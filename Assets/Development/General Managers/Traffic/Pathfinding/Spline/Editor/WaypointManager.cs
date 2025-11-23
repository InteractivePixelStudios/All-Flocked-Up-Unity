#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public class WaypointManager : EditorWindow
{
    [MenuItem("Window/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManager>();
    }

    public Transform waypointRoot;
    private List<Waypoint> trafficPoints = new();
    private Waypoint lastWaypoint;



    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));
        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Select/Assign root transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        if (GUILayout.Button("Branch"))
        {
            BranchLane();
        }
        if (GUILayout.Button("Join 2 Waypoint Loops"))
        {
            JoinWaypointLoop();
        }
        if(GUILayout.Button("Close Waypoint Loop"))
        {
            CloseWaypointLoop();
        }
    }

    void CreateWaypoint()
    {
        if (waypointRoot == null)
        {
            EditorUtility.DisplayDialog("No root", "Please assign a waypoint root.", "OK");
            return;
        }

        Transform parent = waypointRoot;
        Vector3 position = parent.position;
        Vector3 forward = Vector3.forward;

        if (lastWaypoint != null)
        {
            // only use lastWaypoint if it has a valid transform
            Transform lastTransform = lastWaypoint.transform;
            if (lastTransform != null)
            {
                parent = lastTransform.parent != null ? lastTransform.parent : waypointRoot;
                position = lastTransform.position + lastTransform.forward * 5f;
                forward = lastTransform.forward;
            }
        }

        GameObject waypointObj = new GameObject("waypoint" + parent.childCount, typeof(Waypoint));
        waypointObj.transform.SetParent(parent, false);
        waypointObj.transform.position = position;
        waypointObj.transform.forward = forward;

        Waypoint newWaypoint = waypointObj.GetComponent<Waypoint>();
        newWaypoint.tag = "Traffic";

        if (lastWaypoint != null)
        {
            lastWaypoint.nextWaypoint = newWaypoint;
            newWaypoint.previousWaypoint = lastWaypoint;
        }

        lastWaypoint = newWaypoint;
        Selection.activeObject = newWaypoint.gameObject;
    }
    void BranchLane()
    {
        if (lastWaypoint == null)
        {
            EditorUtility.DisplayDialog("No waypoint selected", "Click a waypoint first.", "OK");
            return;
        }

        Waypoint selectedPoint = lastWaypoint;

        Transform currentRoot = selectedPoint.transform.parent;
        if (currentRoot == null)
        {
            EditorUtility.DisplayDialog("No waypoint root", "The selected waypoint has no parent transform.", "OK");
            return;
        }

        GameObject branchObj = new GameObject("Branch" + currentRoot.childCount, typeof(Waypoint));
        branchObj.transform.SetParent(currentRoot, false);

        Waypoint branchWaypoint = branchObj.GetComponent<Waypoint>();
        branchWaypoint.transform.position = selectedPoint.transform.position + selectedPoint.transform.right * 5f;
        branchWaypoint.transform.forward = selectedPoint.transform.forward;
        branchWaypoint.tag = "Traffic";

        selectedPoint.branches.Add(branchWaypoint);
        Selection.activeObject = branchWaypoint.gameObject;

        lastWaypoint = branchWaypoint;
        branchWaypoint.previousWaypoint = lastWaypoint;
        EditorUtility.SetDirty(selectedPoint);
        EditorUtility.SetDirty(branchWaypoint);
    }
    void JoinWaypointLoop()
    {
        if (waypointRoot == null || waypointRoot.childCount == 0)
        {
            EditorUtility.DisplayDialog("Missing root", "Assign a root waypoint group first.", "OK");
            return;
        }

        if (lastWaypoint == null)
        {
            EditorUtility.DisplayDialog("No waypoint selected", "Click a waypoint first.", "OK");
            return;
        }

        Waypoint selectedPoint = lastWaypoint;

        Waypoint[] allWaypoints = GameObject.FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        Waypoint closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var wp in allWaypoints)
        {
            if (wp == selectedPoint) continue;
            if (wp.transform.IsChildOf(selectedPoint.transform.root)) continue;

            float dist = Vector3.Distance(selectedPoint.transform.position, wp.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = wp;
            }
        }

        if (closest == null)
        {
            EditorUtility.DisplayDialog("No nearby waypoints", "Couldn’t find a suitable waypoint in another path.", "OK");
            return;
        }

        selectedPoint.nextWaypoint = closest;
        closest.previousWaypoint = selectedPoint;

        lastWaypoint = closest; // update reference to newly connected waypoint

        EditorUtility.SetDirty(selectedPoint);
        EditorUtility.SetDirty(closest);
    }

    void CloseWaypointLoop()
    {
        if (waypointRoot == null || waypointRoot.childCount == 0)
        {
            EditorUtility.DisplayDialog("Missing root", "Assign a root waypoint group first.", "OK");
            return;
        }

        if (lastWaypoint == null)
        {
            EditorUtility.DisplayDialog("No waypoint selected", "Click a waypoint first.", "OK");
            return;
        }

        Waypoint selectedPoint = lastWaypoint;

        Waypoint[] allWaypoints = GameObject.FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        Waypoint closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var wp in allWaypoints)
        {
            if (wp == selectedPoint) continue;

            float dist = Vector3.Distance(selectedPoint.transform.position, wp.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = wp;
            }
        }

        if (closest == null)
        {
            EditorUtility.DisplayDialog("No nearby waypoints", "Couldn’t find a suitable waypoint in another path.", "OK");
            return;
        }

        selectedPoint.nextWaypoint = closest;
        closest.previousWaypoint = selectedPoint;

        lastWaypoint = closest; // update reference to newly connected waypoint

        EditorUtility.SetDirty(selectedPoint);
        EditorUtility.SetDirty(closest);
    }


    private void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
        {
            Waypoint selected = Selection.activeGameObject.GetComponent<Waypoint>();
            if (selected != null && selected.transform.IsChildOf(selected.transform.root))
            {
                lastWaypoint = selected;
                Repaint(); // refresh the editor window so GUI updates if needed
            }
        }
    }
}
#endif