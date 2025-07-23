using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questTitle;
    [TextArea] public string questDescription;

    ObjectiveDetails[] objectives;
  
}
