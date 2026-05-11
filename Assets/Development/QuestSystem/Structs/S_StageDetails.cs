using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct StageDetails
{
    //Store the Stage variables and refs here.

    public LocalizedString stageName;
     public LocalizedString stageDescription;

    public ObjectiveDetails[] objectivesToComplete;

    public int expReward;
    public int trinketReward;
    public bool hasDialogueAfter;
}
