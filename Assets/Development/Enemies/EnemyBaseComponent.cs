using NUnit.Framework.Constraints;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseComponent : MonoBehaviour, I_EnemyBase
{
    [SerializeField] private Q_KillComponent questKillComponent;
    public bool isDeadLocal;
    public int currentHealth = 10;
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
        TriggerStateChangeOnHit();
        //currentHealth -= damage;
        //if (currentHealth <= 0)
        //{
        //    isDeadLocal = true;
        //    OnDeath(isDeadLocal);
        //    Debug.Log("Enemy Is Dead");

        //}
    }

    public void OnDeath(bool IsDead)
    {
        TriggerStateChangeOnHit();
    }
    public void TriggerStateChangeOnHit()
    {
        OnHit();
    }

    public virtual void OnHit()
    {
        Debug.Log("CallBaseOnHit");
    }



}
