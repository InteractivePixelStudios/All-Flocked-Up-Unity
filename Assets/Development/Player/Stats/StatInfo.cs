

using System;

public class StatInfo 
{
    float health, stamina, stealth, speed, poopAmount, stunDuration;

    public enum stats
    {
        Health, Stamina, Stealth, Speed, PoopAmount, StunDuration
    }

    public float GetStat(stats stat)
    {
        switch (stat)
        {
            case stats.Health:
                return health;
            case stats.Stamina:
                return stamina;
            case stats.Stealth:
                return stealth;
            case stats.Speed:
                return speed;
            case stats.PoopAmount:
                return poopAmount;
            case stats.StunDuration:
                return stunDuration;
        }

        return 0;
    }

    public void SetStat(stats stat, float value)
    {
        switch (stat)
        {
            case stats.Health:
                health = value;
                break;
            case stats.Stamina:
                stamina = value;
                break;
            case stats.Stealth:
                stealth = value;
                break;
            case stats.Speed:
                speed = value;  
                break;
            case stats.PoopAmount:
                poopAmount = value;
                break;
            case stats.StunDuration:
                stunDuration = value;
                break;
        }
    }

    public void LevelStat(stats stat, float value)
    {
        switch (stat)
        {
            case stats.Health:
                health += value;
                break;
            case stats.Stamina:
                stamina += value;
                break;
            case stats.Stealth:
                stealth += value;
                break;
            case stats.Speed:
                speed += value;
                break;
            case stats.PoopAmount:
                poopAmount += value;
                break;
            case stats.StunDuration:
                stunDuration += value;
                break;
        }
    }

}
