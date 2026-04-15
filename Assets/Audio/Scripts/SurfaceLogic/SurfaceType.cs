using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SurfaceMaterialGroups
{
    public SurfaceTypes surfaceType;
    public List<Material> materials;
}

public enum SurfaceTypes
{
    Default,   // 0
    Grass,     // 1
    Metal,     // 2
    Concrete,  // 3
    Wood,      // 4
    Water      // 5
}

public class SurfaceType : MonoBehaviour
{
    [Header("Surface Material Settings")]
    public List<SurfaceMaterialGroups> surfaceMaterialGroups;
}