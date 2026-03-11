using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;


[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class QuestDetails : ScriptableObject
{
    //Store the QUEST variables and refs here.

    public LocalizedString questName;
    public string questID;
    public LocalizedString questLogDescription;
    public LocalizedString trackerDescription;

    public bool isMainQuest;
    public int stagesToComplete;

    public bool isQuestTimed;
    public float questTime;

    public bool autoAcceptQuest;
    public bool autoCompleteQuest;

    public StageDetails[] stages;

    public List<GameObject> itemRewards = new();
}
