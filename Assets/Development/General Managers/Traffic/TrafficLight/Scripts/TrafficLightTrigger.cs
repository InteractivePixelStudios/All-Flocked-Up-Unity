using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Android;

public class TrafficLightTrigger : MonoBehaviour
{
    private List<VehicleBase> stoppedVehicle = new();
    public BoxCollider redLightBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        redLightBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle")) { 
            var vehicle = other.GetComponent<VehicleBase>();
            if (vehicle != null && !stoppedVehicle.Contains(vehicle))
            {
                if (!vehicle.isStopped)
                {
                    stoppedVehicle.Add(vehicle);
                    vehicle.isStopped = true;
                    vehicle.StopVehicle();
                    Debug.Log("HitLight");
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            var vehicle = other.GetComponent<VehicleBase>();
            if (vehicle != null)
            {
                stoppedVehicle.Remove(vehicle);
            }
        }
    }

    public void StartMoveAfterLight()
    {
        foreach(var vehicle in stoppedVehicle)
        {
            if(vehicle != null)
            {
                vehicle.isStopped = false;
                vehicle.MoveVehicleToLocation();
            }
        }
       stoppedVehicle.Clear();
    }

}
