using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private List<TrafficLightChanger> trafficLights;
    [SerializeField] private List<TrafficLightChanger> groupALights;
    [SerializeField] private List<TrafficLightChanger> groupBLights;
    [SerializeField] private List<Waypoint> waypoints;
    private int lastIndex;
    [SerializeField] private int numberOfCars;
    [SerializeField] private List<VehicleBase> vehicleTypes = new();
    [SerializeField] private List<VehicleBase> vehicles;
    public float timer;


    async void Start()
    {
        InitLights();
        GroupTrafficLights();
        FindWaypoints();

        SpawnCarsAtWaypoints();
        await System.Threading.Tasks.Task.Yield();
        foreach (var light in groupALights)
        {
            light.ChangeLightState(new GreenState(light), ETrafficLightState.Green);
        }

        foreach (var light in groupBLights)
        {
            light.ChangeLightState(new RedState(light), ETrafficLightState.Red);
        }



    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SwitchLightGroups();
        }
    }



    private void InitLights()
    {
        trafficLights.Clear();
        trafficLights.AddRange(FindObjectsByType<TrafficLightChanger>(FindObjectsSortMode.None));
    }

    private void SetLights()
    {

    }
    private void FindWaypoints()
    {
        var waypointsArray = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        foreach (var waypoint in waypointsArray)
        {
            if (waypoint.CompareTag("Traffic"))
            {
                waypoints.Add(waypoint);
            }

        }
        Debug.Log("CheckforWaypoints");
    }

    private void GroupTrafficLights()
    {
        for (int i = 0; i < trafficLights.Count;)
        {
            if (trafficLights[i].name.Contains("TrafficLightCurved"))
            {
                groupALights.Add(trafficLights[i]);
                i++;
            }
            else
            {
                groupBLights.Add(trafficLights[i]);
                i++;
            }
        }
        trafficLights.Clear();
    }



    public void ChangeGroupALightState(ITrafficInterface state, ETrafficLightState lightState) { 
        foreach(var light in groupALights)
        {
          
            light.ChangeLightState(state,lightState);

        }


    }

    public void ChangeGroupBLightState(ITrafficInterface state, ETrafficLightState lightState)
    {
        foreach (var light in groupBLights)
        {

            light.ChangeLightState( state, lightState);
        }
    }

    private void SwitchLightGroups()
    {
        if (groupALights.Count == 0 || groupBLights.Count == 0)
            return;

        var groupAState = groupALights[0].state;
        if (groupAState ==ETrafficLightState.Green)
        {
            ChangeGroupALightState(new RedState(groupALights[0]), ETrafficLightState.Yellow);
            ChangeGroupBLightState(new GreenState(groupBLights[0]), ETrafficLightState.Red);
            timer = 10f;
        }
        if (groupAState == ETrafficLightState.Yellow)
        {
            ChangeGroupALightState(new RedState(groupALights[0]), ETrafficLightState.Red);
            ChangeGroupBLightState(new GreenState(groupBLights[0]), ETrafficLightState.Green);
            timer = 3f;
        }
        else if (groupAState == ETrafficLightState.Red)
        {
            ChangeGroupALightState(new GreenState(groupALights[0]), ETrafficLightState.Green);
            ChangeGroupBLightState(new RedState(groupBLights[0]), ETrafficLightState.Yellow);
            timer = 10f;
        }
    }

    private async void SpawnCarsAtWaypoints()
    {

        for(int i = 0; i < numberOfCars; i++)
        {
                var randomIndex = Random.Range(0, waypoints.Count - 1);
                if (randomIndex != lastIndex)
                {
                    Transform waypoint = waypoints[randomIndex].transform;
                    var car = Instantiate(vehicleTypes[Random.Range(0, vehicleTypes.Count)],waypoint.position,waypoint.rotation);
                    vehicles.Add(car);
                    car.transform.position = waypoint.position;
                    car.currentNode = waypoints[randomIndex];
                    car.manager = this;

                }
        }

        await Task.Yield();
    }

    public void RemoveVehicleFromList(VehicleBase vehicle)
    {
        vehicles.Remove(vehicle);
        SpawnNewCar();
    }

    private void SpawnNewCar()
    {
        var randomIndex = Random.Range(0, waypoints.Count - 1);
        if (randomIndex != lastIndex)
        {
            Transform waypoint = waypoints[randomIndex].transform;
            var car = Instantiate(vehicleTypes[Random.Range(0, vehicleTypes.Count)], waypoint.position, waypoint.rotation);
            vehicles.Add(car);
            car.transform.position = waypoint.position;
            car.currentNode = waypoints[randomIndex];
            car.manager = this;

        }

    }
}
