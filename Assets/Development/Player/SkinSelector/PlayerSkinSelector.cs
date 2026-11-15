using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class PlayerSkinSelector : MonoBehaviour
{
    [SerializeField] List<SkinnedMeshRenderer> skinnedMesh = new();
    [SerializeField] List<Material> materials = new();
    Material currentMertial;
    int skinIndex;

    public Material GetCurrentMaterial()
    {
        return currentMertial;
    }

    public void SetLoadedMaterial(Material loaded)
    {
        currentMertial = loaded;
    }


    public void SelectSkin(int index)
    {
        currentMertial = materials[index];
        SetSkinToMesh(currentMertial);
    }

    void SetSkinToMesh(Material material)
    {
        foreach (var mesh in skinnedMesh)
        {
            mesh.material = material;
        }
    }

    public void NextSkin()
    {
        if (skinIndex == materials.Count)
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

}
