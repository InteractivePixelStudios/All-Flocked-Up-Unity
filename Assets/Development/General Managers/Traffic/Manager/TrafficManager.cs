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
    [SerializeField] private List<Waypoint> respawnWaypoints;
    [SerializeField]float respawnDelay = 2f;
    int vehicleCache;
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
        respawnDelay -= Time.deltaTime;

        if (timer <= 0)
        {
            SwitchLightGroups();
        }
        if(respawnDelay <= 0 && vehicleCache > 0 && vehicles.Count < numberOfCars)
        {

                CallSpawnNew();
                respawnDelay = 2f;
            
        }
    }



    private void InitLights()
    {
        trafficLights.Clear();
        trafficLights.AddRange(FindObjectsByType<TrafficLightChanger>());
    }

    private void SetLights()
    {

    }
    private void FindWaypoints()
    {
        var waypointsArray = FindObjectsByType<Waypoint>();
        foreach (var waypoint in waypointsArray)
        {
            if (waypoint.CompareTag("Traffic"))
            {
                waypoints.Add(waypoint);
                if (waypoint.gameObject.layer == LayerMask.NameToLayer("TrafficWaypoints"))
                {
                    respawnWaypoints.Add(waypoint);
                }
                else continue;
            }

        }
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
            ChangeGroupBLightState(new YellowState(groupBLights[0]), ETrafficLightState.Yellow);
            timer = 10f;
        }
        else if (groupAState == ETrafficLightState.Red)
        {
            ChangeGroupALightState(new GreenState(groupALights[0]), ETrafficLightState.Green);
            ChangeGroupBLightState(new RedState(groupBLights[0]), ETrafficLightState.Red);
            timer = 10f;
        }
    }

    private  void SpawnCarsAtWaypoints()
    {
        if (waypoints.Count == 0) return;
        List<Waypoint> used = new();
        List<Waypoint> tempList = waypoints;
        for(int i = 0; i < numberOfCars; i++)
        {
            var randomIndex = Random.Range(0, waypoints.Count);
            Waypoint waypoint = tempList[randomIndex];
            var car = Instantiate(vehicleTypes[Random.Range(0, vehicleTypes.Count)],waypoint.transform.position,waypoint.transform.rotation);
            vehicles.Add(car);
            car.transform.position = waypoint.transform.position + new Vector3(0.1f,0.1f,0.1f);
            car.currentNode = waypoint;
            used.Add( waypoint);
            tempList.Remove(waypoint);
            car.manager = this;
            if(tempList.Count <= 0) { Debug.Log("tempList empty... no more waypoints?"); }
        }

        //await Task.Yield();
    }

    public void RemoveVehicleFromList(VehicleBase vehicle)
    {
        vehicles.Remove(vehicle);
        vehicleCache++;
        CallSpawnNew();
    }

    private  void CallSpawnNew()
    {
        Debug.Log("CallSpawnNew");
                SpawnNewCar();
                vehicleCache--;
            
        
    }

    private void SpawnNewCar()
    {
        var randomIndex = Random.Range(0, respawnWaypoints.Count);
            Waypoint waypoint = respawnWaypoints[randomIndex];
            var car = Instantiate(vehicleTypes[Random.Range(0, vehicleTypes.Count)], waypoint.transform.position, waypoint.transform.rotation);
            vehicles.Add(car);
            car.transform.position = waypoint.transform.position;
            car.currentNode = respawnWaypoints[randomIndex];
            car.manager = this;

    }
    
}
