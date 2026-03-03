using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;


public class CinematicController : MonoBehaviour
{
   public CinemachineSplineDolly splineDollyRef;
    [SerializeField] bool isCredits;
    public bool isPlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineDollyRef = GetComponentInChildren<CinemachineSplineDolly>(); 
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if( splineDollyRef.CameraPosition>=1) 
        {
            isPlaying = false;
            Task.Delay(500);
            DestroyPrefab();
        }
    }

    private void DestroyPrefab()
    {
        this.gameObject.SetActive(false);
        if (isCredits)
        {
            Destroy(FindFirstObjectByType<CreditRoll>().gameObject);
        }
    }

}
