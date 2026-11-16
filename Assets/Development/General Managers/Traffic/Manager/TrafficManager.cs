using UnityEngine;
using System.Collections.Generic;



public class TrafficManager : MonoBehaviour
{
    [SerializeField] private List<TrafficLightChanger> trafficLights;
    [SerializeField] private List<TrafficLightChanger> groupALights;
    [SerializeField] private List<TrafficLightChanger> groupBLights;
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField] private List<Waypoint> respawnWaypoints;
    [SerializeField]float respawnDelay = 3f;
    private int lightIndex = 1;
    int vehicleCache;
    private int lastIndex;
    [SerializeField] private int numberOfCars;
    [SerializeField] private List<VehicleBase> vehicleTypes = new();
    [SerializeField] private List<VehicleBase> vehicles;
    public float timer = 10f;
    [SerializeField] private LayerMask trafficLayer;


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
        if(respawnDelay <= 0)
        {
            if(vehicles.Count < numberOfCars)
            {
                CallSpawnNew();
            }
            respawnDelay = 3f;          
        }
    }

    private void InitLights()
    {
        trafficLights.Clear();
        trafficLights.AddRange(FindObjectsByType<TrafficLightChanger>());
    }

    private void FindWaypoints()
    {
        waypoints.Clear();
        respawnWaypoints.Clear();
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
            light.ChangeLightState(CreateLightState(light,lightState), lightState);
        }
    }

    public void ChangeGroupBLightState(ITrafficInterface state, ETrafficLightState lightState)
    {
        foreach (var light in groupBLights)
        {
            light.ChangeLightState(CreateLightState(light, lightState),lightState);
        }
    }

    private ITrafficInterface CreateLightState(TrafficLightChanger light, ETrafficLightState state)
    {
        return state switch
        {
            ETrafficLightState.Green => new GreenState(light),
            ETrafficLightState.Yellow => new YellowState(light),
            ETrafficLightState.Red => new RedState(light),
            _ => null


        };
    }
    private void SwitchLightGroups()
    {
        if (groupALights.Count == 0 || groupBLights.Count == 0)
            return;
        if (lightIndex == 0)
        {
            ChangeGroupALightState(new GreenState(groupALights[0]), ETrafficLightState.Green);
            ChangeGroupBLightState(new RedState(groupBLights[0]), ETrafficLightState.Red);
            timer = 10f;
        }
        else if (lightIndex == 1)
        {
            ChangeGroupALightState(new YellowState(groupALights[0]), ETrafficLightState.Yellow);
            ChangeGroupBLightState(new RedState(groupBLights[0]), ETrafficLightState.Red);
            timer = 10f;
        }
        else if (lightIndex == 2)
        {
            ChangeGroupALightState(new RedState(groupALights[0]), ETrafficLightState.Red);
            ChangeGroupBLightState(new GreenState(groupBLights[0]), ETrafficLightState.Green);
            timer = 10f;
        }
        else if (lightIndex == 3)
        {
            ChangeGroupALightState(new RedState(groupALights[0]), ETrafficLightState.Red);
            ChangeGroupBLightState(new YellowState(groupBLights[0]), ETrafficLightState.Yellow);
            timer = 10f;
        }
        lightIndex = (lightIndex + 1) % 4;
    }

    private  void SpawnCarsAtWaypoints()
    {
        if (waypoints.Count == 0) return;
        List<Waypoint> used = new();
        List<Waypoint> tempList = new(waypoints);
        for(int i = 0; i < numberOfCars && tempList.Count > 0; i++)
        {
            var randomIndex = Random.Range(0, tempList.Count);
            Waypoint spawn = tempList[randomIndex];
            Waypoint next = spawn.nextWaypoint;
            var car = Instantiate(vehicleTypes[Random.Range(0, vehicleTypes.Count)], spawn.transform.position, spawn.transform.rotation);
            vehicles.Add(car);
            car.currentNode = next;
            used.Add(spawn);
            tempList.Remove(spawn);
            car.manager = this;
        }
    }

    public void RemoveVehicleFromList(VehicleBase vehicle)
    {
        vehicles.Remove(vehicle);
        vehicleCache++;
    }

    private  void CallSpawnNew()
    {
        SpawnNewCar();
        vehicleCache--;                
    }

    private void SpawnNewCar()
    {
        int attempts = 10;

        while (attempts-- > 0)
        {
            if (respawnWaypoints.Count == 0 || vehicleTypes.Count == 0) return;
            int randomIndex = Random.Range(0, respawnWaypoints.Count);
            Waypoint spawn = respawnWaypoints[randomIndex];

            if (spawn == null || spawn.nextWaypoint == null)
                continue;

            if (Physics.CheckSphere(spawn.transform.position, 4f, trafficLayer))
                continue;
            var car = Instantiate(vehicleTypes[Random.Range(0, vehicleTypes.Count)], spawn.transform.position, spawn.transform.rotation);
            vehicles.Add(car);

            //car.transform.position = waypoint.transform.position;
            car.currentNode = spawn.nextWaypoint;
            car.manager = this;
            return;
        }
    }    
}
