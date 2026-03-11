using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AchievementInfo", menuName = "Scriptable Objects/AchievementInfo")]
public class AchievementInfo : ScriptableObject
{
    public string achievementName;
    [TextArea]public string achievementDescription;
    public string achievementID;
    public bool unlocked;
}
