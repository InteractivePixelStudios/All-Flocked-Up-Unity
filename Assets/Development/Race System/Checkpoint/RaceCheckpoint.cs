using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class RaceCheckpoint : MonoBehaviour
{
    [SerializeField] private RaceBase raceBase;
    public string raceID;
    public int checkpointNumber;
    private GameObject hitObject;
   private int num;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        num = raceBase.activeCheckpoints.Count;
    }
    //calls UpdateCheckpoints on raceBase
    private void TriggerCheckpoint()
    {
        if (raceBase != null && raceBase.checkpointIndex == checkpointNumber)
        {
            raceBase.UpdateCheckpoints(checkpointNumber);
            
        }

    }

    public void DestroyCheckpoint()
    {
        Destroy(this.gameObject);
    }

    //trigger enter will call above function and destroy checkpoint
    private void OnTriggerEnter(Collider other)
    {
        //this triggers early ....why me....activeCheckpoints get removed when hit so try to save count on Start but still nothing
        if (checkpointNumber >= num)raceBase.AddRacerToCompleted(other.gameObject, this);

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
