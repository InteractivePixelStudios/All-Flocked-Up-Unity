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
    [SerializeField] private SkinShopLocation shopLocation;
    [SerializeField] private GameObject playerSpawnPoint;
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField]private bool isSelecting;

    public Material GetCurrentMaterial()
    {
        return currentMertial;
    }

    public void SetLoadedMaterial(Material loaded)
    {
        currentMertial = loaded;
    }
    private void Start()
    {
        shopLocation = FindAnyObjectByType<SkinShopLocation>();
        playerSpawnPoint = shopLocation.playerSpawnPoint;
        canvasController = FindAnyObjectByType<UI_CanvasController>();
    }

    public void StartSkinSelector()
    {
        cam.GetComponent<CinemachineInputAxisController>().enabled = false;
        isSelecting = true;
        SetPlayerLocation();
        PivotCamera();
        OpenUI();
    }

    void SetPlayerLocation()
    {
        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        transform.position = playerSpawnPoint.transform.position;
        transform.rotation = playerSpawnPoint.transform.rotation;
    }

    public void DestroyBackdrop()
    {
        //Destroy(spawnedBackdrop);
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
        canvasController.ShowSkinSelector();
    }

    public void ConfirmSelection()
    {
        isSelecting = false;
        cam.GetComponent<CinemachineInputAxisController>().enabled = true;
        cam.GetComponent<CinemachineOrbitalFollow>().enabled = true;
        canvasController.HideSkinSelector();
    }
}
