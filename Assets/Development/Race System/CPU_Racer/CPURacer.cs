using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;


public class CPURacer : MonoBehaviour 
{
    public List<RaceCheckpoint> targetLocation;
    public int index;
    public int totalCheckpoints;
    [SerializeField] private RaceCheckpoint currentLocation;
    [SerializeField] private RaceBase raceBase;
    [SerializeField] private NavMeshAgent navAgentComponent;
    [SerializeField] private RacerFlightComponent flightComponent;
    public bool isMoving = false;
    [SerializeField] private float detectObjectRange;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRange;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Rigidbody body;
    [SerializeField] private RacerStats racerStats;

    [SerializeField] private float speed;
    [SerializeField] private float accel;
    [SerializeField] private float weight;
    [SerializeField] private float stamina;

    private bool isGliding;
    private bool isDiving;
    private bool flapUp;
    private bool isFlying;
    private bool slowFlap;
    private float currentSpeed;
    [SerializeField]private bool wantsToFly;

    public float finishTime;

    public bool GetFlapUp()
    {
        return flapUp;
    }
    public bool GetIsDiving()
    {
        return isDiving;
    }
    public float GetSpeedForward()
    {
        return currentSpeed; 
    }
    public bool GetIsGliding()
    {
        return isGliding;
    }
    public bool GetIsFlying()
    {
        return isFlying;
    }
    public bool GetIsJumping()
    {
        return isJumping;
    }
    public bool GetIsSlowFlap()
    {
        return slowFlap;
    }


    //on load
    public void Awake()
    {

    }
    //on start
    public void Start()
    {
        flightComponent = GetComponent<RacerFlightComponent>();
        body = GetComponent<Rigidbody>();
        navAgentComponent = GetComponent<NavMeshAgent>();
        raceBase = FindAnyObjectByType<RaceBase>();
        SetRacerStats();
        SetRacerSpeed();
        SetRacerWeight();
        SetRacerAcceleration();
        GetCheckpoints();
        if (raceBase.countdownComplete)
        {
            isMoving = true;
        }else isMoving = false;


    }
    //raycasts for groundcheck and obstacle detection... if has targetlocation and isMoving then it moves
    public void Update()
    {
        if(!raceBase.countdownComplete)return;
        if (wantsToFly)
        {
            RacerFly();
        }
        GroundCheck();
        CheckForObstacles();
        Debug.Log("CurrentSpeed = " + currentSpeed);
        if (targetLocation != null && isMoving && index < targetLocation.Count)
        {
            isFlying = flightComponent.isFlying;
            currentLocation = targetLocation[index];
            if (!isFlying)
            {
                SetMoveToLocation(index);
            }
            else
            {
                FlightNavigation();
            }
                currentSpeed = GetComponent<Rigidbody>().linearVelocity.magnitude;

        }

        if(index > targetLocation.Count-1)
        {
            //Set placing in race finish
            StopMoving();
        }
    }

    //sets the location to move to
    public void SetMoveToLocation(int index)
    {
        if (targetLocation == null || index < 0 || index >= targetLocation.Count) return;
        MoveToLocation(targetLocation[index]);
    }
    //moves the nav agent to the location given
    public void MoveToLocation(RaceCheckpoint checkpoint)
    {
        if (checkpoint == null || navAgentComponent == null || !navAgentComponent.isOnNavMesh) return;
        Vector3 destination = checkpoint.transform.position;
        if (!navAgentComponent.hasPath ||  Vector3.Distance(navAgentComponent.destination,destination)>0.1f)
        {

            navAgentComponent.SetDestination(destination);
        }
    }


    //Stops nav agent movement
    private void StopMoving()
    {
        isMoving = false;
        if (navAgentComponent.enabled && navAgentComponent.isOnNavMesh)
        {
            navAgentComponent.isStopped = true;
        }
        body.linearVelocity = Vector3.zero;
    }
    public void NextCheckpoint()
    {
        index += 1 ;

    }
    //raycast to check for obstacles
    private void CheckForObstacles()
    {
        float turnDegree = 30f;
        RaycastHit hit;
        if(Physics.Raycast(transform.position + transform.up / 4, transform.forward*detectObjectRange, out hit,detectLayer))
        {
            Debug.DrawRay(transform.position + transform.up/4, transform.forward * detectObjectRange, Color.red);
            TurnRacer(turnDegree);
        }

        //needs to angle down
        if (Physics.Raycast(transform.position + transform.up / 4, transform.forward * detectObjectRange/10 - transform.up / 4, out hit, detectLayer))
        {
            Debug.DrawRay(transform.position + transform.up / 4, transform.forward * detectObjectRange/10 - transform.up / 8, Color.red);
            Jump();

        }
        if (Physics.Raycast(transform.position + transform.up / 4, transform.right * detectObjectRange, out hit, detectLayer))
        {
            Debug.DrawRay(transform.position + transform.up / 4, transform.right * detectObjectRange, Color.red);
            TurnRacer(turnDegree);
        }
        if (Physics.Raycast(transform.position + transform.up / 4, -transform.right* detectObjectRange, out hit, detectLayer))
        {
            Debug.DrawRay(transform.position + transform.up / 4, -transform.right * detectObjectRange, Color.red);
            TurnRacer(-turnDegree);
        }
    }
    //raycast for groundcheck bool 
    public bool GroundCheck()
    {
        Vector3 origin = transform.position + transform.up * 0.25f;
        isGrounded = Physics.Raycast(origin, -transform.up, out RaycastHit hit, groundRange, groundLayer);
        if (isGrounded)
        {
            Debug.DrawRay(origin, -transform.up * groundRange, Color.red);
            isJumping = false;
        }
        return isGrounded;
    }
    //turns the racer when raycast detected in obstacle detection
    private void TurnRacer(float turnDeg)
    {
        transform.Rotate(0f, turnDeg * Time.deltaTime, 0f);
    }
    
    //YUMP-ING
    private void Jump()
    {
        if (!isGrounded || isJumping || isFlying) return;
        isJumping = true;
        wantsToFly = true;
        if (navAgentComponent.enabled)
        {
            navAgentComponent.enabled = false;
        }
        body.isKinematic = false;
        body.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse); ;
    }

    //... they believe they can touch the sky
    private void RacerFly()
    {
        if (flightComponent.isFlying) return;
        if (navAgentComponent.enabled) navAgentComponent.enabled = false;
        flightComponent.InitiateFlight();
        isFlying = flightComponent.isFlying;
    }

    private void FlightNavigation()
    {
        if (currentLocation == null || flightComponent == null) return;
        Vector3 direction = currentLocation.transform.position - transform.position;
        float distance = direction.magnitude;
        if (distance <= 0.1f) return;
        Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        if (distance > 3f)
        {
            if (direction.y > 0.5f)
            {
                flightComponent.gliding = false;
                flightComponent.FlapUp();
            }
            else if (direction.y < -0.5f)
            {
                flightComponent.gliding = true;
            }
        }
    }

    public void OnFlightEnded()
    {
        isFlying = false;
        wantsToFly = false;
    }
    //gets the checkpoints for the race and orders them
    private void GetCheckpoints()
    {
        if (targetLocation == null)
        {
            targetLocation = new List<RaceCheckpoint>();
        }
        else targetLocation.Clear();
        if (raceBase == null || raceBase.activeCheckpoints == null) return;
        foreach (var checkpoint in raceBase.activeCheckpoints)
        {
            if (checkpoint == null) continue;
            targetLocation.Add(checkpoint);
        }
        totalCheckpoints = targetLocation.Count;
    }
    //Sets the racer stats to global current variables
    private void SetRacerStats()
    {
        speed = GetRacerSpeed();
        accel = GetRacerAcceleration();
        weight = GetRacerWeight();
        stamina = GetRacerStamina();
    }

    //gets speed from racer stats and +- a random float
    private float GetRacerSpeed()
    {
        var speed = racerStats.speed;
        speed += Random.Range(-5f, 5f);
        return speed;
    }

    //gets accel from racer stats and +- a random float
    private float GetRacerAcceleration()
    {
        var accel = racerStats.acceleration;
        accel += Random.Range(-5f, 5f);
        return accel;
    }

    //gets weight from racer stats and +- a random float
    private float GetRacerWeight()
    {
        var weight = racerStats.weight;
        weight += Random.Range(-5f, 5f);
        return weight;
    }

    //gets stamina from racer stats and +- a raandom float
    private float GetRacerStamina()
    {
        var stamina = racerStats.stamina;
        stamina += Random.Range(-5f, 5f);
        return stamina;
    }

    private void SetRacerSpeed()
    {
        navAgentComponent.speed = speed;
    }

    private void SetRacerWeight()
    {
        body.mass = weight;
    }
    private void SetRacerStamina()
    {

    }

    private void SetRacerAcceleration()
    {
        navAgentComponent.acceleration = accel;
    }

    public void StartMoving()
    {
        isMoving = true;
        if (navAgentComponent.enabled && navAgentComponent.isOnNavMesh)
        {
            navAgentComponent.speed = speed;
            navAgentComponent.isStopped = false;
        }
    }

    public void ToggleNavCompOn()
    {
        navAgentComponent.enabled = true;
    }


}
