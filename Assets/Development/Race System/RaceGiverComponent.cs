using UnityEngine;


public class RaceGiver : MonoBehaviour
{
    private RaceBase race;
    public RaceData raceData;
    public UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        race = FindFirstObjectByType<RaceBase>();
    }

    //called by PlayerInteraction... spawns the race giver canvas
    public void InteractWithRaceGiver()
    {
        race.currentRaceGiver = this;

        canvasController.OpenRaceGiver();
    }
}
