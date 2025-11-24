using UnityEngine;

public enum FootstepSurface // Set in editor per surface type - This is so we don't need to worry about materials, layers or physics materials.
{
    Default = 0,
    Grass = 1,
    Metal = 2,
    Concrete = 3,
    Wood = 4,
    // add more as needed
}

[DisallowMultipleComponent]
public class SurfaceType : MonoBehaviour
{
    public FootstepSurface surface = FootstepSurface.Default;
}
