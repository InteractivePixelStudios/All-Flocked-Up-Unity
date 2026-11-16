using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CinematicController : MonoBehaviour
{
   public CinemachineSplineDolly splineDollyRef;
    [SerializeField] bool isCredits;
    public bool isPlaying;

    void Start()
    {
        splineDollyRef = GetComponentInChildren<CinemachineSplineDolly>(); 
        isPlaying = true;
    }

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
            Destroy(FindAnyObjectByType<CreditRoll>().gameObject);
            DestroyImmediate(AudioWizard.Instance.gameObject);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
