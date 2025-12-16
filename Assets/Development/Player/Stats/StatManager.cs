using UnityEngine;

public class StatManager : Singleton<StatManager>
{
    StatInfo _stats;

    private void Start()
    {
        _stats = new StatInfo();

        // Load stats from save here

    }

    public void LevelUpStats(float healthIncr, float staminaIncr, float stealthIncr, float speedIncr, float poopIncr, float stunIncr)
    {
        float[] statincr = { healthIncr, staminaIncr, stealthIncr, speedIncr, poopIncr, stunIncr };

        for (int i = 0; i < statincr.Length; i++)
        {
            _stats.LevelStat((StatInfo.stats)i, statincr[i]);
        }

    }
}
