using UnityEngine;

public class Traffic_StopSign : MonoBehaviour
{
    private VehicleBase stoppedVehicle;

    [SerializeField] private float stopDelay = 3f;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isStopped;

    void Update()
    {
        if (stoppedVehicle == null || !isStopped)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            StartMoveAfterDelay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        VehicleBase vehicle = other.GetComponentInParent<VehicleBase>();

        if (vehicle == null || stoppedVehicle != null)
            return;

        stoppedVehicle = vehicle;
        isStopped = true;
        currentTime = stopDelay;

        stoppedVehicle.StopVehicle();
    }

    private void OnTriggerExit(Collider other)
    {
        VehicleBase vehicle = other.GetComponentInParent<VehicleBase>();

        if (vehicle == null || vehicle != stoppedVehicle)
            return;

        stoppedVehicle = null;
        isStopped = false;
        currentTime = 0f;
    }

    public void StartMoveAfterDelay()
    {
        if (stoppedVehicle == null || !isStopped)
            return;

        isStopped = false;
        stoppedVehicle.StartMove();
    }

    private void OnDisable()
    {
        if (stoppedVehicle != null && isStopped)
            stoppedVehicle.StartMove();

        stoppedVehicle = null;
        isStopped = false;
    }
}