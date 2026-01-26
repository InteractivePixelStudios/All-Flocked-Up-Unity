using UnityEngine;


//this script exists to control the state of the player and alternate between modes like camera, and normal behavior. 
//First iteration will only have groundmove, camera, and flymove. The controller will be used to regulate what the player can do based on state
//camera state - movement disabled, camera will instead move, can take pictures
//groundmove state - normal movement when not flying
//flymove state - movement while flying


//will require integration into the player movement system and other scripts to function



public enum PlayerState {GroundMove, PhotoMode, FlyMove}
public class PlayerStateController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; } = PlayerState.GroundMove;

    public void EnterPhotoMode()
    {
        CurrentState = PlayerState.PhotoMode;
    }

    public void ExitPhotoMode()
    {
        CurrentState = PlayerState.GroundMove;
    }

    public void EnterFlyMode()
    {
        CurrentState = PlayerState.FlyMove;
    }

    public void ExitFlyMode()
    {
        CurrentState = PlayerState.GroundMove;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
