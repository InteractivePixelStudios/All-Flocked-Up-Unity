using UnityEngine;

[System.Serializable]
public struct ObjectiveDetails
{
    public string objectiveName;
    [TextArea] public string objectiveDescription;
    public string objectiveType;
    public string objectiveID;
    public int quantityToComplete;
    public bool isOptional;
    public int bonusEXP;
}

