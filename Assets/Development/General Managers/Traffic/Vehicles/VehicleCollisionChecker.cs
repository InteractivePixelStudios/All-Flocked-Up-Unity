using UnityEngine;

public class VehicleCollisionChecker : MonoBehaviour
{
    VehicleScript vehicleScript;
    BusScript busScript;
    BikeScript bikeScript;
    int type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            bikeScript = GetComponentInParent<BikeScript>();
            if (bikeScript != null)
            {
                type = 1;
            }
        }
        catch
        {
            busScript = GetComponentInParent<BusScript>();
            if (busScript != null)
            {
                type = 2;
            }
        }
        finally
        {
            vehicleScript = GetComponentInParent<VehicleScript>();
            if (vehicleScript != null)
            {
                type = 3;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") ||
             other.gameObject.CompareTag("Vehicle") ||
             other.gameObject.CompareTag("Enemy") ||
             other.gameObject.CompareTag("NPC"))
        {
            if (type == 1)
            {
                if (other.gameObject.GetComponent<BikeScript>().isStopped) { bikeScript.isStopped = true; }
                else { 
                bikeScript.SpeedUp();
                other.gameObject.GetComponent<BikeScript>().SendCollisions();
            }
            }
            if (type == 2)
            {
                if (other.gameObject.GetComponent<BusScript>().isStopped) { busScript.isStopped = true; }
                else
                {
                    busScript.SpeedUp();
                    other.gameObject.GetComponent<BusScript>().SendCollisions();
                }
            }
            if (type == 3)
            {
                if (other.gameObject.GetComponent<VehicleScript>().isStopped) { vehicleScript.isStopped = true; }
                else
                {
                    vehicleScript.SpeedUp();
                    other.gameObject.GetComponent<VehicleScript>().SendCollisions();
                }
            }



        }


    }
}
