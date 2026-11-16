using UnityEngine;

public class LargeNest :  NestBase
{
    [Header("LargeNestRefs")]
    [SerializeField] private MeshFilter mesh;
    [SerializeField] private Material material;

    private void ChangeNestMesh()
    {

    }

    private void ChangeNestMaterial()
    {

    }

    private void BuildNest()
    {
        ChangeNestMaterial();
        ChangeNestMesh();
    }
}
