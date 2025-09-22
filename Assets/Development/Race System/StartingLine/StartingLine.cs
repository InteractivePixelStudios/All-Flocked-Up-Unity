using UnityEngine;

//this is just to store the raceID so the raceBase can pull this gameObjects transform as the starting line location
public class StartingLine : MonoBehaviour
{
    public string raceID;

    public void SetRotationToCheckpoint(RaceCheckpoint checkpoint)
    {

        Vector3 lookAt = checkpoint.transform.position;
        transform.LookAt(lookAt);
    }
}
