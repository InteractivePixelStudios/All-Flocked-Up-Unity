using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SurfaceType))]
public class SurfaceTypeEditor : Editor
{
    // use this one to fully lock the script and prevent changes.
    /*
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Please contact Isaiah if you wish to modify this.",
            MessageType.Warning
        );


        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("THIS SCRIPT IS LOCKED", style);
    }
    */

    /*
    // Use the following for a simple warning.
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "DO NOT TOUCH without contacting Isaiah first!",
            MessageType.Warning
        );

        DrawDefaultInspector();
    }
    */

    // Full on password Lock.
    private bool isUnlocked = false;
    private string unlockText = "";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "DO NOT TOUCH - Please contact Isaiah first before modifying.",
            MessageType.Error
        );

        GUILayout.Space(5);

        if (!isUnlocked)
        {
            EditorGUILayout.LabelField("Type UNLOCK to reveal the inspector:");
            unlockText = EditorGUILayout.TextField("Confirmation", unlockText);

            if (GUILayout.Button("Unlock Inspector"))
            {
                if (unlockText == "UNLOCK")
                {
                    isUnlocked = true;
                }
            }

            return;
        }

        EditorGUILayout.HelpBox(
            "Inspector unlocked. Lock it again when finished.",
            MessageType.Warning
        );

        if (GUILayout.Button("Lock Inspector"))
        {
            isUnlocked = false;
            unlockText = "";
            return;
        }

        GUILayout.Space(5);
        DrawDefaultInspector();
    }
}
