#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[ExecuteInEditMode]
public class InteriorStudioManager : EditorWindow
{
    [Header("Room Info")]
    public Vector3 roomSize = new Vector3();
    string interiorName = "";
    [SerializeField] List<ShellSpawner> shells = new();
    ShellSpawner currentShell;
    [Header("Interior List")]
    [SerializeField] List<string> shellNames = new();
    [SerializeField] private int indexer;
    int currentIndex;
    [Header("Prefabs")]
    [SerializeField] ShellSpawner shellSpawnerPrefab;
    [SerializeField] EntranceSpawner entranceSpawner;
    [SerializeField] EntranceSpawner currentEntrance;
    [SerializeField] ExitSpawner exitSpawner;
    [SerializeField] ExitSpawner currentExit;
    Vector3 entranceLocation = new();
    Vector3 exitLocation = new();
    [Header("Triggers")]
    [SerializeField] bool isPlacingEntrance = false;
    [SerializeField] bool isPlacingExit = false;
    [Header("WallMaterials")]
    [SerializeField] private Material frontMaterial;
    [SerializeField] private Material backMaterial;
    [SerializeField] private Material leftMaterial;
    [SerializeField] private Material rightMaterial;
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material ceilMaterial;


    private void OnSceneGUI(SceneView sceneView)
    {
        if (shellNames.Count < 1)
        {
            shellNames.Clear();
        }
        Event click = Event.current;

        if (click.type == EventType.MouseDown && click.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(click.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (isPlacingEntrance)
                {
                    entranceLocation = hit.point;
                    currentEntrance = Instantiate(entranceSpawner, entranceLocation, Quaternion.identity);
                    currentEntrance.name = "Entrance";
                    currentEntrance.locationName = interiorName;
                    currentEntrance.transform.SetParent(currentShell.transform, true);
                    isPlacingEntrance = false;
                    click.Use();
                }
                else if (isPlacingExit)
                {
                    exitLocation = hit.point;
                    currentExit = Instantiate(exitSpawner, exitLocation, Quaternion.identity);
                    currentExit.name = "Exit";
                    currentExit.locationName = interiorName;
                    currentExit.transform.SetParent(currentShell.transform, true);
                    isPlacingExit = false;
                    click.Use();
                }
            }
        }

        SceneView.RepaintAll(); 
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void Update()
    {
        if (shellNames.Count < 1)
        {
            shellNames.Clear();
        }
        SceneView.RepaintAll();

        if (isPlacingEntrance)
            HandleSceneClick(SetEntranceLocation);

        if (isPlacingExit)
            HandleSceneClick(SetExitLocation);

    }

    void HandleSceneClick(System.Action callback)
    {
        Event e = Event.current;
        if (e == null) return;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            callback();
            e.Use();
        }
    }



    [MenuItem("Window/InteriorStudio Window")]
    public static void Open()
    {
        GetWindow<InteriorStudioManager>();
    }

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        SerializedProperty shellPrefab = obj.FindProperty("shellSpawnerPrefab");
        shellSpawnerPrefab = shellPrefab.objectReferenceValue as ShellSpawner;
        SerializedProperty entrancePrefab = obj.FindProperty("entranceSpawner");
        entranceSpawner = entrancePrefab.objectReferenceValue as EntranceSpawner;
        SerializedProperty placingEntrance = obj.FindProperty("isPlacingEntrance");
        placingEntrance.boolValue = isPlacingEntrance;
        SerializedProperty placingExit = obj.FindProperty("isPlacingExit");
        placingExit.boolValue = isPlacingExit;
        SerializedProperty exitPrefab = obj.FindProperty("exitSpawner");
        exitSpawner = exitPrefab.objectReferenceValue as ExitSpawner;
        EditorGUILayout.HelpBox("This will help create a basic interior ready to be decorated and used.", MessageType.Info);
        EditorGUILayout.BeginVertical("box");
        roomSize = EditorGUILayout.Vector3Field("Room Size: ", roomSize);
        interiorName = EditorGUILayout.TextField("Name of Interior: ", interiorName);
        indexer = EditorGUILayout.Popup(indexer, shellNames.ToArray());
        SerializedProperty frontMat = obj.FindProperty("frontMaterial");
        frontMaterial = frontMat.objectReferenceValue as Material;
        SerializedProperty backMat = obj.FindProperty("backMaterial");
        backMaterial = backMat.objectReferenceValue as Material;
        SerializedProperty leftMat = obj.FindProperty("leftMaterial");
        leftMaterial = leftMat.objectReferenceValue as Material;
        SerializedProperty rightMat = obj.FindProperty("rightMaterial");
        rightMaterial = rightMat.objectReferenceValue as Material;
        SerializedProperty floorMat = obj.FindProperty("floorMaterial");
        floorMaterial = floorMat.objectReferenceValue as Material;
        SerializedProperty ceilMat = obj.FindProperty("ceilMaterial");
        ceilMaterial = ceilMat.objectReferenceValue as Material;
        DrawButtons();
        EditorGUILayout.EndVertical();
        EditorGUILayout.PropertyField(shellPrefab);
        EditorGUILayout.PropertyField(entrancePrefab);
        EditorGUILayout.PropertyField(exitPrefab);
        EditorGUILayout.PropertyField(placingEntrance);
        EditorGUILayout.PropertyField(placingExit);
        EditorGUILayout.PropertyField(frontMat);
        EditorGUILayout.PropertyField(backMat);
        EditorGUILayout.PropertyField(leftMat);
        EditorGUILayout.PropertyField(rightMat);
        EditorGUILayout.PropertyField(floorMat);
        EditorGUILayout.PropertyField(ceilMat);
        DrawDeleteButton();
        obj.ApplyModifiedProperties();

    }

    void DrawDeleteButton()
    {
        if (GUILayout.Button("Destroy Interior"))
        {
            DestroyAndRemoveInterior(interiorName);
        }
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Spawn Interior Shell"))
        {
            SpawnInteriorShell();
        }
        if (GUILayout.Button("Place Entrance Location"))
        {
            if (!isPlacingEntrance)
            {
                ReadyPlacement(0);//0 for entrance
                EditorGUILayout.HelpBox("Click where the entrance should be.", MessageType.Warning);
            }else isPlacingEntrance = false;
        }
        if (GUILayout.Button("Place Exit Location"))
        {
            if (!isPlacingExit)
            {
                ReadyPlacement(1); //1 for exit
                EditorGUILayout.HelpBox("Click where the exit should be.", MessageType.Warning);
            }else isPlacingExit=false;
        }
        if (GUILayout.Button("Update Locations"))
        {
            UpdateLocations();
        }
        if(GUILayout.Button("Update Shell Materials"))
        {
            SendMaterialsToShell();
            currentShell.UpdateMaterials();
        }
        if(GUILayout.Button("Resize Interior Shell"))
        {
            ResizeShell();
        }
    }


    private void CreateDropdownOption(string name)
    {
        shellNames.Add(name);
    }

    private void DestroyAndRemoveInterior(string name)
    {
        shellNames.Remove(name);
        var interiors = Object.FindObjectsByType<ShellSpawner>(FindObjectsSortMode.None);
        foreach (var interior in interiors)
        {
            if(interior.name == name)
            {
                DestroyImmediate(interior.gameObject);
                shells.Remove(interior);
            }
        }

    }

    void SpawnInteriorShell()
    {
        currentShell = Instantiate(shellSpawnerPrefab, null, true);
        shells.Add(currentShell);
        var name = currentShell.name = interiorName;
        currentShell.roomSize = roomSize;
        currentShell.SpawnWalls(roomSize);
        CreateDropdownOption(name);
    }

    void ReadyPlacement(int side)
    {
        if (side == 0)
        {
            isPlacingEntrance = true;
        }
        else isPlacingExit = true;

    }

    void SetEntranceLocation()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            entranceLocation = hit.point;
            currentEntrance = Instantiate(entranceSpawner, entranceLocation, Quaternion.identity);
            currentEntrance.locationName = interiorName;
            currentEntrance.endLocation = exitLocation;
            isPlacingEntrance = false;
        }
    }

    void SetExitLocation()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            exitLocation = hit.point;
            currentExit = Instantiate(exitSpawner, exitLocation, Quaternion.identity);
            currentExit.locationName = interiorName;
            currentExit.startLocation = entranceLocation;
            isPlacingExit = false;
        }
    }

    void UpdateLocations()
    {
        currentEntrance.endLocation = exitLocation;
        currentExit.startLocation = entranceLocation;
    }

    void SendMaterialsToShell()
    {
        currentShell.frontMaterial = frontMaterial;
        currentShell.backMaterial = backMaterial;
        currentShell.leftMaterial = leftMaterial;
        currentShell.rightMaterial = rightMaterial;
        currentShell.floorMaterial = floorMaterial;
        currentShell.ceilMaterial=ceilMaterial;
    }

    void ResizeShell()
    {
        currentShell.roomSize = roomSize;
        currentShell.ResizeRoom();
    }



    }
#endif