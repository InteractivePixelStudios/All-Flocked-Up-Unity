using UnityEngine;

public class EXPSystem : MonoBehaviour
{
    [SerializeField] private int currentXP;
    [SerializeField] private int requiredToNextLevel;
    [SerializeField] private int currentLevel;
    public int PLAYERLEVEL => GetLevel();


    //Get Level
    private int GetLevel()
    {
        return currentLevel;
    }

    //Increment XP
    public void IncrementXP(int value)
    {
        currentXP+=value;
        while(currentXP >= requiredToNextLevel) 
        {
            var temp = currentXP - requiredToNextLevel;
            LevelUP();
        }
    }

    //LevelUP
    private void LevelUP()
    {
        requiredToNextLevel *= 2;
        currentLevel++;
        OnLevelUpEffect();
    }

    //OnLevelUPEffect

    private void OnLevelUpEffect()
    {
        GameObject playerRef = FindFirstObjectByType<PlayerGroundMovement>().gameObject;
        playerRef.GetComponent<PlayerHealth>().maxHealth += 5;
        //Add stamina
        //Add poop
    }
}
