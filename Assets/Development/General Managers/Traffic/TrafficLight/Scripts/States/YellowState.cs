using UnityEngine;

public class YellowState : ITrafficInterface
{
    [SerializeField]private TrafficLightChanger lightChanger;
    [SerializeField] private ETrafficLightState light;
    [SerializeField] private float timer=3f;

    public YellowState(TrafficLightChanger lightChanger) { this.lightChanger = lightChanger; }
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
