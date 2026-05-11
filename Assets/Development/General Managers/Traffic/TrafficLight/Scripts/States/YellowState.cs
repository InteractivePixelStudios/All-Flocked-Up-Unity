using UnityEngine;

public class YellowState : ITrafficInterface
{
    [SerializeField]private TrafficLightChanger lightChanger;
    [SerializeField] private ETrafficLightState light;
    [SerializeField] private float timer=3f;

    public YellowState(TrafficLightChanger lightChanger) { this.lightChanger = lightChanger; }
    public void EnterTrafficState()
    {
       // lightChanger.SetState();
        light = ETrafficLightState.Yellow;
    }

    public void UpdateTrafficState()
    {

    }

    public void ExitTrafficState()
    {

    }
}
