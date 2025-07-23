using UnityEngine;

[System.Serializable]
public struct StageDetails
{
    public string stageName;
    [TextArea] public string stageDescription;

    public ObjectiveDetails[] objectivesToComplete;

    public int expReward;
    public int trinketReward;
}
