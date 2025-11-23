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
        if(!this.isActiveAndEnabled)
        {
            stoppedVehicle.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle")) { 
            stoppedVehicle = other.gameObject.GetComponent<VehicleBase>();
            stoppedVehicle.isStopped = true;
            Debug.Log("triggerHit");
        }
    }

    public void StartMoveAfterLight()
    {
        stoppedVehicle.MoveVehicleToLocation();
    }

}
