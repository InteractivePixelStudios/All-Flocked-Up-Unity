using UnityEngine;

public class LargeNest :  NestBase
{
    [Header("LargeNestRefs")]
    [SerializeField] private MeshFilter mesh;
    [SerializeField] private Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
