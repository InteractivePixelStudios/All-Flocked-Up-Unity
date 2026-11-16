using UnityEngine;
using System.Collections.Generic;

public class VehicleCollisionChecker : MonoBehaviour
{
    VehicleBase vehicleBase;
    private HashSet<VehicleBase> stoppedVehicles = new();

    private void Awake()
    {
        vehicleBase = GetComponentInParent<VehicleBase>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var hitVehicle = other.GetComponentInParent<VehicleBase>();
        if(hitVehicle == null|| hitVehicle == vehicleBase) { return; }
        stoppedVehicles.Add(hitVehicle);
        vehicleBase.SetBlocked(true);
            
        
    }

    private void OnTriggerExit(Collider other)
    {
        var hitVehicle = other.GetComponentInParent<VehicleBase>();
        if (hitVehicle == null || hitVehicle == vehicleBase) { return; }
        stoppedVehicles.Remove(hitVehicle);
        if(stoppedVehicles.Count== 0)
        {
            vehicleBase.SetBlocked(false);
        }
    }


        
}
