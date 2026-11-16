using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RaceCheckpoint : MonoBehaviour
{
    [SerializeField] private RaceBase raceBase;
    public string raceID;
    public int checkpointNumber;
    private int num;
    [SerializeField] private ParticleSystem checkpointParticle;
    [SerializeField] private ParticleSystem finishParticles;
    [SerializeField]private bool isFinish;

    private void Start()
    {
        num = raceBase.activeCheckpoints.Count;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void ShowCheckpoint()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
    //calls UpdateCheckpoints on raceBase
    private  void TriggerCheckpoint()
    {
        if (raceBase != null && raceBase.checkpointIndex == checkpointNumber)
        {
            raceBase.UpdateCheckpoints(checkpointNumber);
            raceBase.UpdatePlayerArrow(checkpointNumber);
            checkpointParticle.Play();
            GetComponent<MeshRenderer>().enabled = false;                       
        }
    }

    public void DestroyCheckpoint()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!raceBase.raceStarted) return;
        //this triggers early ....why me....activeCheckpoints get removed when hit so try to save count on Start but still nothing
        if (checkpointNumber >= num || isFinish) { raceBase.AddRacerToCompleted(other.gameObject, this); finishParticles.Play(); }
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerCheckpoint();
        }
        if (other.gameObject.CompareTag("Race"))
        {
            CPURacer racer = other.gameObject.GetComponent<CPURacer>();
            racer.NextCheckpoint(); 
        }
    }


}
