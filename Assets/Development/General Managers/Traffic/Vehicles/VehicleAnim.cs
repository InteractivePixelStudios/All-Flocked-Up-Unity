using UnityEngine;

public class VehicleAnim : MonoBehaviour
{
    [SerializeField] private Animator animComp;
    [SerializeField] private VehicleBase vehicleBase;
    [SerializeField] private bool isMoving =>vehicleBase.GetIsMoving();
    [SerializeField] private bool isLeftTurn => vehicleBase.GetIsLeftTurn();
    [SerializeField] private bool isRightTurn=> vehicleBase.GetIsRightTurn();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animComp = GetComponent<Animator>();
        vehicleBase = GetComponentInParent<VehicleBase>();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleMoving();
        ToggleLeftTurn();
        ToggleRightTurn();
    }

    private void ToggleMoving()
    {
        if(animComp != null && isMoving != false)
        {
            animComp.SetBool("IsMoving",isMoving);
        }
    }

    private void ToggleRightTurn()
    {
        if (animComp != null && isRightTurn != false)
        {
            animComp.SetBool("IsRightTurn", isRightTurn);
        }
    }

    private void ToggleLeftTurn()
    {
        if (animComp != null && isLeftTurn != false)
        {
            animComp.SetBool("IsLeftTurn", isLeftTurn);
        }
    }


}
