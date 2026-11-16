using UnityEngine;
public class AchievementUnlocker : MonoBehaviour
{
    [SerializeField] private AchievementList achievementBase;
    protected static string achievement;
    void Awake()
    {
        achievementBase = FindAnyObjectByType<AchievementList>();   
    }
}
