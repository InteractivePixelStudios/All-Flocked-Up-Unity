using Unity.VisualScripting;
using UnityEngine;

public class HumanSkinSelector : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer bodyMesh;
    [SerializeField] SkinnedMeshRenderer hairMesh;
    [SerializeField] SkinnedMeshRenderer browMesh;
    [SerializeField] SkinnedMeshRenderer shirtMesh;
    [SerializeField] SkinnedMeshRenderer pantsMesh;
    [SerializeField] SkinnedMeshRenderer shoeMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh[] skinMesh = FindAnyObjectByType(typeof(HumanGeneratorManager)).GetComponent<HumanGeneratorManager>().GenerateHuman();

        bodyMesh.sharedMesh = skinMesh[0];
        hairMesh.sharedMesh = skinMesh[1];
        browMesh.sharedMesh = skinMesh[2];
        shirtMesh.sharedMesh = skinMesh[3];
        pantsMesh.sharedMesh = skinMesh[4];
        shoeMesh.sharedMesh = skinMesh[5];
    }
}
