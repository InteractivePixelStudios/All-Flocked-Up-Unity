using UnityEngine;
using UnityEngine.AI;

public class CarController : MonoBehaviour
{
    private MeshCollider mC;
    private NavMeshAgent nMA;
    void Start()
    {
        if (mC == null)
        {
            mC = GetComponent<MeshCollider>();
        }
        if (nMA == null)
        {
            nMA = GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
