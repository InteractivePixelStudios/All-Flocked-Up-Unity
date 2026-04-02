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
    Default,
    Grass,
    Wood,
    Stone,
    Metal,
    Water
}

public class SurfaceType : MonoBehaviour
{
    [Header("Surface Material Settings")]
    public List<SurfaceMaterialGroups> surfaceMaterialGroups;
}