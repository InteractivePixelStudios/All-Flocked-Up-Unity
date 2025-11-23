using Unity.Cinemachine;
using UnityEngine;


public class CinematicController : MonoBehaviour
{
   [SerializeField] private CinemachineSplineDolly splineDollyRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineDollyRef = GetComponentInChildren<CinemachineSplineDolly>(); 

    }

    // Update is called once per frame
    void Update()
    {
        
        if( splineDollyRef.CameraPosition>=1) 
        {
            DestroyPrefab();
        }
    }

    private void DestroyPrefab()
    {
        Destroy(gameObject);
        Destroy(FindFirstObjectByType<CreditRoll>().gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
