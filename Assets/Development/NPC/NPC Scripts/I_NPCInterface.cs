using UnityEngine;

public interface I_NPCInterface
{
    public void LookAtNPC();
    public void InteractWithNPCDialogue();

    public void MoveToLocation();

    public void SetMoveToLocation(Transform location);

}
