

using System;

public class StatInfo 
{
    float health, stamina, stealth, speed, poopAmount, damage;

    public enum stats
    {
        Health, Stamina, Stealth, Speed, PoopAmount, Damage
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
            case stats.Damage:
                return damage;
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
            case stats.Damage:
                damage = value;
                break;
        }
    }

}
