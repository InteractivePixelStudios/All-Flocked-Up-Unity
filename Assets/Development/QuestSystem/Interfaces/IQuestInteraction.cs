using UnityEngine;
//Interface for Quest functions.
public interface IQuestInteraction
{
    public void InteractWithNPC(QuestLog playerQuestLog);
    public void LookAtNPC();
    
}
