using UnityEngine;

public class YellowState : ITrafficInterface
{
    [SerializeField]private TrafficLightChanger lightChanger;
    [SerializeField] private ETrafficLightState light;


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
