using UnityEngine;

public class RedState : ITrafficInterface
{

    [SerializeField] private TrafficLightChanger lightChanger;
    [SerializeField] private ETrafficLightState light;
    [SerializeField] private float timer=10f;

    public RedState(TrafficLightChanger lightChanger) { this.lightChanger = lightChanger; }

    public void EnterTrafficState()
    {
        this.lightChanger = lightChanger;

    }

    public void UpdateTrafficState() 
    {



    }

    public void ExitTrafficState()
    {

    }
}
