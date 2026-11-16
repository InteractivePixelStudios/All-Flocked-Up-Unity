using UnityEngine;
using System.Collections.Generic;

public class TrafficLightTrigger : MonoBehaviour
{
    private readonly List<VehicleBase> stoppedVehicles = new();
    public BoxCollider redLightBox;


    private void Awake()
    {
        if (redLightBox == null) redLightBox = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        VehicleBase vehicle = other.GetComponentInParent<VehicleBase>();

        if (vehicle == null)
            return;

        if (!stoppedVehicles.Contains(vehicle))
        {
            stoppedVehicles.Add(vehicle);
            vehicle.SetStoppedAtLight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        VehicleBase vehicle = other.GetComponentInParent<VehicleBase>();
        if (vehicle != null && stoppedVehicles.Remove(vehicle))
        {
            vehicle.SetStoppedAtLight(false);
        }
    }

    public void StartMoveAfterLight()
    {
        foreach (VehicleBase vehicle in stoppedVehicles)
        {
            if (vehicle != null)
            {
                vehicle.SetStoppedAtLight(false);
            }
        }

        stoppedVehicles.Clear();
    }
}