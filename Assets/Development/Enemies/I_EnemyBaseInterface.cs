using System.Xml.Serialization;
using UnityEngine;

public interface I_EnemyBase
{


    void TakeDamage(int damage);

    void OnDeath(bool IsDead);
    
}
