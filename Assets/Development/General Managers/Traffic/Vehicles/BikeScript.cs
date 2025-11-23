using UnityEngine;

public class BikeScript : VehicleBase
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

    protected override void CheckForCollisions()
    {
        base.CheckForCollisions();
    }

    protected override void HonkHorn()
    {
        //add horn SFX/possible headlight VFX? 
    }
}
