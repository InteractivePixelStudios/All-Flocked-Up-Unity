using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct ObjectiveDetails
{
    //Store all the Objective variables & refs here.

    public LocalizedString objectiveName;
    public LocalizedString objectiveDescription;
    public string objectiveType;
    public string objectiveID;
    public int quantityToComplete;
    public bool isOptional;
    public int bonusEXP;
}

