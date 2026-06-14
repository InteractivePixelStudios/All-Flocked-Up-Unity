using UnityEngine;

public class HAS_NPC : NPCBase
{
    HAS_Controller controller;
    bool isFound;



    public void CallFound()
    {
        if (controller == null) { controller = FindAnyObjectByType<HAS_Controller>(); }
        if (controller.CheckForNPC(this) && !isFound)
        {
            SetMoveToLocation(controller.GetSpawnLocation().transform);
            isMoving = true;
            isFound = true;
        }else return;
    }

    public void SetNewHomeLoc(GameObject home)
    {
        base.SetNewHome(home);
    }

}
