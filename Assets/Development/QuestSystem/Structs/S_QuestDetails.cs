using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class QuestDetails : ScriptableObject
{
    //Store the QUEST variables and refs here.

    public string questName;
    public string questID;
    [TextArea] public string questLogDescription;
    [TextArea] public string trackerDescription;

    public bool isMainQuest;
    public int stagesToComplete;

    public bool isQuestTimed;
    public float questTime;

    public bool autoAcceptQuest;
    public bool autoCompleteQuest;

    public StageDetails[] stages;
}
