
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPerchSystem : MonoBehaviour
{
    public I_Perchable currentPerchPoint;
    [SerializeField] private IconToggle icon;
    public bool isReady;
    bool isPerching;
    //[SerializeField] private float checkDistance = 5f;
    public bool moveLeft;
    public bool moveRight;
    float x;

    PlayerInput playerInput;
    InputAction moveAction;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

    }


    void Update()
    {

        if (isPerching && x < 0f) { currentPerchPoint.MovePosition(x); }

    }

    public void Perch(I_Perchable currentPerchPoint)
    {
        if (isReady)
        {
            InteractWithPerch(currentPerchPoint);
            Debug.Log("InteractWithPerch");
        }
    }

    private void InteractWithPerch(I_Perchable currentPerchPoint)
    {
        try
        {
            currentPerchPoint.StartPerch();
        }
        catch
        {
            if(PerchableObject_Tree.Equals(currentPerchPoint, true))
            {
                currentPerchPoint.StartPerch();
            }
            
        }
        finally
        {
            
        }
    }


}
