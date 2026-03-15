using NUnit.Framework.Constraints;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseComponent : MonoBehaviour, I_EnemyBase
{
    [SerializeField] private Q_KillComponent questKillComponent;
    public bool isDeadLocal;
    public int currentHealth;
    public GameObject enemyRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        questKillComponent = GetComponent<Q_KillComponent>();
        enemyRef = this.gameObject;
    }


    void Update()
    {

    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDeadLocal = true;
            OnDeath(isDeadLocal);
            Debug.Log("Enemy Is Dead");

        }
    }

    public void OnDeath(bool IsDead)
    {
        TriggerStateChangeOnHit();
    }

    protected void TriggerStateChangeOnHit()
    {
        if(this is EnemyPatrol)
        {
            enemyRef.GetComponent<EnemyPatrol>().SetCurrentState(EnemyPatrol.EnemyState.Hit);
        }
        else if(this is AI_Cat)
        {
            enemyRef.GetComponent<AI_Cat>().SetCurrentState(AI_Cat.EnemyState.Hit);
        }
        else if (this is AI_Dog)
        {
            enemyRef.GetComponent<AI_Dog>().SetCurrentState(AI_Dog.EnemyState.Hit);
        }
        else if (this is AI_Hawk)
        {
            enemyRef.GetComponent<AI_Hawk>().SetCurrentState(AI_Hawk.EnemyState.Hit);
        }
        else if (this is AI_Carlos)
        {
            //enemyRef.GetComponent<AI_Carlos>().
        }
        else if(this is AI_Raccoon)
        {
            enemyRef.GetComponent<AI_Raccoon>().SetCurrentState(AI_Raccoon.EnemyState.Hit);
        }
    }


}
