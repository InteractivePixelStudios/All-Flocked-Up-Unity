using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class CarController : MonoBehaviour
{
    [Header("Car Components")]
    private MeshCollider MC;
    private SplineAnimate SA;

    [Header("Car Settings")]
    [SerializeField] public bool isStopped = false;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float acceleration = 2f;
    [SerializeField] public float currentSpeed = 0f;
    [SerializeField] public Spline spline;

    void Start()
    {
        if (MC == null)
        {
            MC = GetComponent<MeshCollider>();
        }
        if (SA == null)
        {
            SA = GetComponent<SplineAnimate>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SA.MaxSpeed = currentSpeed;
    }

    public void SetNewSpline(Spline newSpline)
    {
        spline = newSpline;
    }

    public void StartCar()
    {
        isStopped = false;
    }

    public void Accelerate(float deltaTime)
    {
        if (isStopped) return;
        currentSpeed = Mathf.Min(currentSpeed + acceleration * deltaTime, maxSpeed);
    }
    public void Decelerate(float deltaTime)
    {
        if (isStopped) return;
        currentSpeed = Mathf.Max(currentSpeed - acceleration * deltaTime, 0f);

        if (currentSpeed == 0f)
        {
            isStopped = true;
        }
    }
}
