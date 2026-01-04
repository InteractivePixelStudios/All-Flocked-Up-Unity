using UnityEngine;
using UnityEngine.AI;

public class VehicleScript : VehicleBase
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void SetMoveToLocation(Waypoint location)
    {
        base.SetMoveToLocation(location);
    }

    //call this to run like wind
    public override void MoveVehicleToLocation()
    {
        base.MoveVehicleToLocation();
    }

    public override void StopVehicle()
    {
        base.StopVehicle();
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
