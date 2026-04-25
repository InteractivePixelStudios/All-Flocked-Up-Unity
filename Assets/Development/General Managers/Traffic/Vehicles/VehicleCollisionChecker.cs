using UnityEngine;

public class VehicleCollisionChecker : MonoBehaviour
{
    VehicleBase vehicleBase;
    int type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") ||
             other.gameObject.CompareTag("Vehicle") ||
             other.gameObject.CompareTag("Enemy") ||
             other.gameObject.CompareTag("NPC"))
        {
            vehicleBase = other.GetComponent<VehicleBase>();
            vehicleBase.isStopped = true;
            vehicleBase.TriggerCollisions();
        } 
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player") ||
             other.gameObject.CompareTag("Vehicle") ||
             other.gameObject.CompareTag("Enemy") ||
             other.gameObject.CompareTag("NPC"))
        {
           
            vehicleBase.MoveVehicleToLocation();


        }
    }


        
}
