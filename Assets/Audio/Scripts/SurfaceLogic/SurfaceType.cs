using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
/**
-Surface Types will always be a manual order.
    - Default (0), Grass (1), Metal (2), Concrete (3), Wood (4), Water (5), Dirt (6), etc. - this will be used for sound variation in footsteps and other surface interactions.
*/

[System.Serializable] // This is only here for organization, it is not technically needed.
public class SurfaceMaterialGroups
{
    public SurfaceTypes surfaceType;
    public List<Material> materials; // old idea, in the bin now.
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