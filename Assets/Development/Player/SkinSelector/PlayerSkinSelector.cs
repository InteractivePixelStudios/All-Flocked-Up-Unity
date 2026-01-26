using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class PlayerSkinSelector : MonoBehaviour
{
    [SerializeField] List<SkinnedMeshRenderer> skinnedMesh = new();
    [SerializeField] List<Material> materials = new();
    Material currentMertial;
    int skinIndex;

    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CameraController controller;
    [SerializeField] private GameObject camLocation;
    [SerializeField] private GameObject backdropPrefab;
    [SerializeField] private GameObject spawnedBackdrop;
    [SerializeField] private GameObject backdropSpawnPoint;
    [SerializeField]private bool isSelecting;
    [SerializeField] private GameObject canvasPrefab;
    [SerializeField] private GameObject spawnedCanvas;

    private void Start()
    {


    }

    public void StartSkinSelector()
    {
        cam.GetComponent<CinemachineInputAxisController>().enabled = false;
        isSelecting = true;
        SpawnBackdrop();
        PivotCamera();
        OpenUI();
    }
    void SpawnBackdrop()
    {
        spawnedBackdrop = Instantiate(backdropPrefab, backdropSpawnPoint.transform.position, backdropSpawnPoint.transform.rotation);
    }

    public void DestroyBackdrop()
    {
        Destroy(spawnedBackdrop);
    }

    void PivotCamera()
    {
        if (isSelecting)
        {
            cam.GetComponent<CinemachineOrbitalFollow>().enabled = false;
            cam.transform.position = camLocation.transform.position;
            cam.transform.rotation = camLocation.transform.rotation;

        }
        
    }


    public void SelectSkin(int index)
    {
        currentMertial = materials[index];
        SetSkinToMesh(currentMertial);
    }

    void SetSkinToMesh(Material material)
    {
        foreach(var mesh in skinnedMesh)
        {
            mesh.material = material;
        }
    }

    public void NextSkin()
    {
        if(skinIndex == materials.Count)
        {
            skinIndex = 0;
            SelectSkin(skinIndex);
        }
        else
        {
            skinIndex++;
            SelectSkin(skinIndex);
        }
    }

    public void PrevSkin()
    {
        if (skinIndex == 0) { skinIndex = materials.Count; }
        else
        {
            skinIndex--;
            SelectSkin(skinIndex);
        }
    }

    void OpenUI()
    {
        spawnedCanvas = Instantiate(canvasPrefab);
    }

    public void ConfirmSelection()
    {
        isSelecting = false;
        cam.GetComponent<CinemachineInputAxisController>().enabled = true;
        cam.GetComponent<CinemachineOrbitalFollow>().enabled = true;

    }
}
