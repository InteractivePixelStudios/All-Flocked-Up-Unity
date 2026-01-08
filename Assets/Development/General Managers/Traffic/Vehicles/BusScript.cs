using UnityEngine;

public class BusScript : VehicleBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    protected override void SetMoveToLocation(Waypoint location)
    {

    }

    //call this to run like wind
    public override void MoveVehicleToLocation()
    {
        base.MoveVehicleToLocation();
    }

    public override void StopVehicle()
    {
        base.navAgent.isStopped = true;
    }

    protected override void TriggerCollisions()
    {
        base.TriggerCollisions();
    }

    protected override void HonkHorn()
    {
        //add horn SFX/possible headlight VFX? 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") ||
    other.gameObject.CompareTag("Vehicle") ||
    other.gameObject.CompareTag("Enemy") ||
    other.gameObject.CompareTag("NPC"))
        {

            TriggerCollisions();

        }
    }
}
