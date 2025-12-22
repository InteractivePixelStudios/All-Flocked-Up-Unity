using UnityEngine;
using System.Collections.Generic;

public class ShellSpawner : MonoBehaviour
{
    public Vector3 roomSize = new();
    [SerializeField] private List<GameObject> wallPlanes = new();
    [SerializeField] private LayerMask interiorLayer;
    [SerializeField] private InteriorStudioManager studioManager;
    [SerializeField] private const int wallCount = 6;
    [SerializeField] private int index;
    [Header("Wall Objects")]
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject floorPlane;
    [SerializeField] private GameObject ceilPlane;

    [Header("WallMaterials")]
    public Material floorMaterial;
    public Material ceilMaterial;
    public Material frontMaterial;
    public Material backMaterial;
    public Material leftMaterial;
    public Material rightMaterial;
    private List<Material> materialList = new List<Material>();

    private void Start()
    {
        studioManager.roomSize = roomSize;
        SpawnWalls(roomSize);
    }

    public void SpawnWalls(Vector3 size)
    {
        for (int i = 0; i < wallPlanes.Count; i++)
        {
            if (wallPlanes[i] != null)
                Destroy(wallPlanes[i]);
        }
        wallPlanes.Clear();
        for (int i = 0; i < wallCount; i++)
        {
            index = i;
            switch (index)
            {
                case 0:
                    frontWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    frontWall.transform.SetParent(this.transform, true);
                    frontWall.name = "Front Wall";
                    frontWall.transform.rotation = Quaternion.Euler(new Vector3(90,90,0));
                    frontWall.transform.localScale = frontWall.transform.localScale + new Vector3(roomSize.x / 10, roomSize.y / 10, roomSize.z / 10);
                    frontWall.transform.position = transform.position + new Vector3(-roomSize.x * 1.5f, 0,0);
                    frontWall.GetComponent<MeshRenderer>().material = frontMaterial;
                    wallPlanes.Add(frontWall);
                    break;
                case 1:
                    backWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    backWall.transform.SetParent(this.transform, true);
                    backWall.name = "Back Wall";
                    backWall.transform.rotation = Quaternion.Euler(new Vector3(90, -90, 0));
                    backWall.transform.localScale = backWall.transform.localScale + new Vector3(roomSize.x / 10, roomSize.y / 10, roomSize.z / 10);
                    backWall.transform.position = transform.position + new Vector3(roomSize.x * 1.5f, 0, 0);
                    backWall.GetComponent<MeshRenderer>().material = backMaterial;
                    wallPlanes.Add(backWall);
                    break;
                case 2:
                    leftWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    leftWall.transform.SetParent(this.transform, true);
                    leftWall.name = "Left Wall";
                    leftWall.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
                    leftWall.transform.localScale =leftWall.transform.localScale+ new Vector3(roomSize.x / 10, roomSize.y / 10, roomSize.z / 10);
                    leftWall.transform.position = transform.position + new Vector3(0,0,roomSize.x * 1.5f);
                    leftWall.GetComponent<MeshRenderer>().material= leftMaterial;
                    wallPlanes.Add(leftWall);
                    break;
                case 3:
                    rightWall = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    rightWall.transform.SetParent(this.transform, true);
                    rightWall.name = "Right Wall";
                    rightWall.transform.rotation = Quaternion.Euler(new Vector3(-90, -180, 0));
                    rightWall.transform.localScale = rightWall.transform.localScale + new Vector3(roomSize.x/10, roomSize.y / 10, roomSize.z / 10);
                    rightWall.transform.position = transform.position + new Vector3(0,0, -roomSize.x*1.5f);
                    rightWall.GetComponent<MeshRenderer>().material= rightMaterial;
                    wallPlanes.Add(rightWall);
                    break;
                case 4:
                    floorPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    floorPlane.transform.SetParent(this.transform, true);
                    floorPlane.name = "Floor";
                    floorPlane.transform.localScale = floorPlane.transform.localScale + new Vector3(roomSize.x / 10, roomSize.y / 10, roomSize.z / 10);
                    floorPlane.transform.position = transform.position + new Vector3(0, -roomSize.y * 1.5f, 0);
                    floorPlane.GetComponent<MeshRenderer>().material = floorMaterial;
                    wallPlanes.Add(floorPlane);
                    break;
                case 5:
                    ceilPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    ceilPlane.transform.SetParent(this.transform, true);
                    ceilPlane.name = "Cieling";
                    ceilPlane.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    ceilPlane.transform.localScale = ceilPlane.transform.localScale + new Vector3(roomSize.x / 10, roomSize.y / 10, roomSize.z / 10);
                    ceilPlane.transform.position = transform.position + new Vector3(0, roomSize.y * 1.5f, 0);
                    ceilPlane.GetComponent<MeshRenderer>().material = ceilMaterial;
                    wallPlanes.Add(ceilPlane);
                    AddCollisionToWalls();
                    break;

            }
        }
       
    }

    void AddCollisionToWalls()
    {
        foreach(var wall in wallPlanes)
        {
            wall.layer = LayerMask.NameToLayer("Ground");
            wall.tag = "Ground";
            var mesh = wall.GetComponent<MeshCollider>();
            DestroyImmediate(mesh);
           var comp =  wall.AddComponent<BoxCollider>();
            comp.includeLayers = LayerMask.NameToLayer("Player");
            
        }
    }

    public void UpdateMaterials()
    {
        ceilPlane.GetComponent<MeshRenderer>().material = ceilMaterial;
        floorPlane.GetComponent<MeshRenderer>().material = floorMaterial;
        leftWall.GetComponent<MeshRenderer>().material = leftMaterial;
        rightWall.GetComponent<MeshRenderer>().material = rightMaterial;
        frontWall.GetComponent<MeshRenderer>().material = frontMaterial;
        backWall.GetComponent<MeshRenderer>().material = backMaterial;
    }

    public void ResizeRoom()
    {
        frontWall.transform.localScale = roomSize;
        backWall.transform.localScale = roomSize;
        leftWall.transform.localScale = roomSize;
        rightWall.transform.localScale = roomSize;
        floorPlane.transform.localScale = roomSize;
        ceilPlane.transform.localScale = roomSize;
    }
}
