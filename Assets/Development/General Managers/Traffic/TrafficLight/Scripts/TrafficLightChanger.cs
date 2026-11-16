using UnityEngine;


public class TrafficLightChanger : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private GameObject greenLight;
    [SerializeField] private GameObject yellowLight;
    [SerializeField] private GameObject redLight;

    [Header("SystemRefs")]
    [SerializeField] private ETrafficLightState currentLightState;
    [SerializeField] private ITrafficInterface currentState;
    public TrafficManager trafficManager;

    public ETrafficLightState state = new();
    [SerializeField] private LayerMask trafficLayer;
    [SerializeField]private bool redLightStop;

    [SerializeField]private TrafficLightTrigger trigger;
    public float lightTimer;

    void Awake()
    {
        trafficManager = FindAnyObjectByType<TrafficManager>();
        greenLight.SetActive(false);
        yellowLight.SetActive(false);
        redLight.SetActive(false);
        trigger = GetComponentInChildren<TrafficLightTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentLightTimer();    

    }

    public void GetCurrentLightTimer()
    {
        lightTimer = trafficManager.timer;
    }

    public void ChangeLightState(ITrafficInterface newState, ETrafficLightState lightState)
    {
        currentState?.ExitTrafficState();
        currentState = newState;
        state = lightState;
        currentLightState = lightState;
        SetState(lightState);
        currentState?.EnterTrafficState();
    }

    public void SetState(ETrafficLightState state)
    {
        greenLight.SetActive(state == ETrafficLightState.Green);
        yellowLight.SetActive(state == ETrafficLightState.Yellow);
        redLight.SetActive(state == ETrafficLightState.Red);
        switch (state)
        {
            case ETrafficLightState.Green:
                redLightStop = false;
                trigger.redLightBox.enabled = false;
                trigger.StartMoveAfterLight();
                break;
            case ETrafficLightState.Yellow:

                break;
            case ETrafficLightState.Red:
                redLightStop = true;
                trigger.redLightBox.enabled = true;
                break;
        }
    }


}
