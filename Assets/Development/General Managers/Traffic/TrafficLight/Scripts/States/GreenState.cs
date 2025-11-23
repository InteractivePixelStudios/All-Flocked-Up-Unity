using UnityEngine;

public class GreenState : ITrafficInterface
{
    [SerializeField] private TrafficLightChanger lightChanger;
    [SerializeField] private ETrafficLightState light;
    [SerializeField] private float timer = 10f;

    public GreenState(TrafficLightChanger lightChanger) { this.lightChanger = lightChanger; }
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
