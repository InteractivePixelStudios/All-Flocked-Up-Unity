using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

//Allows you to create QuestData scriptable objects. Inherits from scriptable object class and  holds the questID, info, and array of objectives (S_ObjectiveDetails)
// To create a quest, Open Quests Folder, Right-Click - Create - ScriptableObject - QuestData. This will
//make a scriptable object asset that can be assigned to QuestGivers.


[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questTitle;
    [TextArea] public string questDescription;

    public ObjectiveDetails[] objectives;

  
}
