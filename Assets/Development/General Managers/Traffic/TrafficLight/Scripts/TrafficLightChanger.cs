using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
        state = currentLightState;     

    }

    public void GetCurrentLightTimer()
    {
        lightTimer = trafficManager.timer;
    }

    public void ChangeLightState(ITrafficInterface state, ETrafficLightState lightState)
    {
        currentState?.ExitTrafficState();
        currentLightState = lightState;
        currentState = state;
        SetState(lightState);
    }

    private void ChangeLightColor(string color)
    {
        var lightColor = color;
        if (lightColor == "Green")
        {
            greenLight.SetActive(true);
            yellowLight.SetActive(false);
            redLight.SetActive(false);
        }
        else if (lightColor == "Yellow")
        {
            greenLight.SetActive(false);
            yellowLight.SetActive(true);
            redLight.SetActive(false);
        }
        else if (lightColor == "Red")
        {
            greenLight.SetActive(false);
            yellowLight.SetActive(false);
            redLight.SetActive(true);
        }

    }

    public void SetState(ETrafficLightState state)
    {
        switch (state)
        {
            case ETrafficLightState.Green:
                currentState = new GreenState(this);
                trigger.StartMoveAfterLight();
                ChangeLightColor("Green");
                redLightStop = false;
                trigger.redLightBox.enabled = false;
                break;
            case ETrafficLightState.Yellow:
                currentState = new YellowState(this);
                ChangeLightColor("Yellow");
                break;
            case ETrafficLightState.Red:
                currentState = new RedState(this);
                ChangeLightColor("Red");
                redLightStop = true;
                trigger.redLightBox.enabled = true;
                break;
        }
        currentState.EnterTrafficState();
    }


}
