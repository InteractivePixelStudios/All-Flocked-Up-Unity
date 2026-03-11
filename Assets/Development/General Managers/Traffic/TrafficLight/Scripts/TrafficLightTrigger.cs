using UnityEngine;

public class TrafficLightTrigger : MonoBehaviour
{
    private VehicleBase stoppedVehicle;
    public BoxCollider redLightBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        redLightBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!redLightBox.enabled)
        {
            if (stoppedVehicle != null)
            {
                stoppedVehicle.isStopped = false;
                StartMoveAfterLight();
            }
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle")) { 
            stoppedVehicle = other.gameObject.GetComponent<VehicleBase>();
            stoppedVehicle.isStopped = true;

        }
    }

    public void StartMoveAfterLight()
    {
        stoppedVehicle.MoveVehicleToLocation();
        stoppedVehicle = null;
    }

}
