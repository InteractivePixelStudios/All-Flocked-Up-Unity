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
        GetCheckpoints();
        if (raceBase.countdownComplete)
        {
            isMoving = true;
        }else isMoving = false;


    }
    //raycasts for groundcheck and obstacle detection... if has targetlocation and isMoving then it moves
    public void Update()
    {
        if(!raceBase.countdownComplete) { return; }
        if (wantsToFly)
        {
            RacerFly();
        }
        GroundCheck();
        CheckForObstacles();
        Debug.Log("CurrentSpeed = " + currentSpeed);
        if (targetLocation != null && isMoving && index < targetLocation.Count)
        {
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
        int num;
        for (num = index; num < targetLocation.Count;num++)
        {
            // Debug.Log(targetLocation[num].name); Debug.Log(currentLocation);
            MoveToLocation(targetLocation[num]);
        }

    }
    //moves the nav agent to the location given
    public void MoveToLocation(RaceCheckpoint checkpoint)
    {
        if (navAgentComponent.destination != currentLocation.transform.position)
        {

            navAgentComponent.SetDestination(currentLocation.transform.position);
        }
        else return;
    }
    //Stops nav agent movement
    private void StopMoving()
    {
        navAgentComponent.isStopped = true;
        navAgentComponent.speed = 0;
        //says obsolete...it lies...it works... actually setting isStopped only stops movement and doesnt cancel velocity LOL...why unity
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
        RaycastHit ground;
        if(Physics.Raycast(transform.position + transform.up / 4, -transform.up * groundRange, out ground, groundLayer))
        {
            Debug.DrawRay(transform.position + transform.up / 4, -transform.up * groundRange, Color.red);
            isGrounded = true;
            isJumping = false;
        }
        return isGrounded;
    }
    //turns the racer when raycast detected in obstacle detection
    private void TurnRacer(float turnDeg)
    {
        Quaternion startRot = Quaternion.LookRotation(Vector3.forward);
        Quaternion endRot = Quaternion.LookRotation(Vector3.right);
        Quaternion.RotateTowards(startRot, endRot, turnDeg);
    }
    
    //YUMP-ING
    private void Jump()
    {

        if (GroundCheck() == true && !isJumping)
        {
            isJumping = true;
            // add verticle force to make the player jump
            body.AddForce(Vector3.up * jumpHeight);

                wantsToFly = true;
            
        }
        else if (GroundCheck() == true && isJumping && wantsToFly)
        {


            if (!isFlying)
            {
                flightComponent.InitiateFlight();
                Debug.Log("ThisGotCalled");
            }
        }
        else return;

    }

    //... they believe they can touch the sky
    private void RacerFly()
    {
        navAgentComponent.enabled = false;
        if (!isFlying)
        {
            Debug.Log("FLYING");
            flightComponent.InitiateFlight();
        }
        isFlying = true;
    }

    private void FlightNavigation()
    {
        var dir = (currentLocation.transform.position- transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10);
        Debug.Log(dir);
        var distance = Vector3.Distance(currentLocation.transform.position, transform.forward);
        Debug.Log(distance);
        if (distance > 3)
        {
            if (currentLocation.transform.position.y > transform.position.y)
            {
                Debug.Log("NotHighEnough");
                if(flightComponent.isFlying != true) { flightComponent.InitiateFlight(); }
                    flightComponent.FlapUp();
                
            }else if (currentLocation.transform.position.y < transform.position.y)
            {
                flightComponent.gliding = true;
                Debug.Log("Glide");
            }
        }

    }
    //gets the checkpoints for the race and orders them
    private void GetCheckpoints()
    {
        raceBase.activeCheckpoints.ForEach(checkpoint => { targetLocation.Add(checkpoint); totalCheckpoints++; });
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
        Debug.Log("RacerMOVE");
    }

    public void ToggleNavCompOn()
    {
        navAgentComponent.enabled = true;
    }


}
