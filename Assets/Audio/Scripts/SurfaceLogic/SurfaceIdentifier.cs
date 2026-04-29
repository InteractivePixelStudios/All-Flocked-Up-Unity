using UnityEngine;

/*

---Notes---
- This will be used on obejcts that can not utalize the Material system for footsteps. Just place it on an object, set the Surface Type, and done.
-
-
*/

public class SurfaceIdentifier : MonoBehaviour
{
    public enum SurfaceIdentifierTypes
    {
        Default,   // 0
        Grass,     // 1
        Metal,     // 2
        Concrete,  // 3
        Wood,      // 4
        Water      // 5
    }

    [SerializeField] private SurfaceTypes surfaceType; // Set the surface type for this object in the inspector. This will be used for footstep sounds and other surface interactions.
    public SurfaceTypes SurfaceType => surfaceType; // Public getter for surface type, used by FootstepLogicV2 and other scripts to determine the surface type of this object.  
}
