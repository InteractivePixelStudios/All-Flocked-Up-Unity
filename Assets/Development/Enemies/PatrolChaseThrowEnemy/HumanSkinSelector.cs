using Unity.VisualScripting;
using UnityEngine;

public class HumanSkinSelector : MonoBehaviour
{
    [SerializeField] Mesh bodyMesh;
    [SerializeField] Mesh hairMesh;
    [SerializeField] Mesh browMesh;
    [SerializeField] Mesh shirtMesh;
    [SerializeField] Mesh pantsMesh;
    [SerializeField] Mesh shoeMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh[] skinMesh = FindAnyObjectByType(typeof(HumanGeneratorManager)).GetComponent<HumanGeneratorManager>().GenerateHuman();

        bodyMesh = skinMesh[0];
        hairMesh = skinMesh[1];
        browMesh = skinMesh[2];
        shirtMesh = skinMesh[3];
        pantsMesh = skinMesh[4];
        shoeMesh = skinMesh[5];
    }
}
