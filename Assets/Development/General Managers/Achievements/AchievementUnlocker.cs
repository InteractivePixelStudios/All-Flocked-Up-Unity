using UnityEngine;

public class AchievementUnlocker : MonoBehaviour
{

    [SerializeField] private AchievementList achievementBase;
    protected static string achievement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        achievementBase = FindAnyObjectByType<AchievementList>();   
    }


}
