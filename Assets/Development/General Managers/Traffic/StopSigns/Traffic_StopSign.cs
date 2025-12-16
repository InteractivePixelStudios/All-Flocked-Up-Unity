using UnityEngine;

public class Traffic_StopSign : MonoBehaviour
{
    private VehicleBase stoppedVehicle;
    public BoxCollider redLightBox;
    [SerializeField] private float stopDelay;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isStopped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        redLightBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopped && currentTime > 0)
        {
            currentTime-=Time.deltaTime;
            stoppedVehicle.isStopped = true;
        } if(!isStopped || currentTime < 0)
        {
            isStopped = false;
            stoppedVehicle.isStopped = false;
            StartMoveAfterDelay();
            currentTime = stopDelay;
        }
        if (!this.isActiveAndEnabled)
        {
            stoppedVehicle.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle"))
        {
            stoppedVehicle = other.gameObject.GetComponent<VehicleBase>();
            stoppedVehicle.isStopped = true;
            isStopped = true;
            currentTime = stopDelay;
            Debug.Log("triggerHit");
        }
    }

    public void StartMoveAfterDelay()
    {
        stoppedVehicle.MoveVehicleToLocation();
    }

}


