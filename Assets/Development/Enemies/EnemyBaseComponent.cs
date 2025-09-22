using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseComponent : MonoBehaviour, I_EnemyBase
{
    [SerializeField] private Q_KillComponent questKillComponent;
    public bool isDeadLocal;
    public int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        questKillComponent = GetComponent<Q_KillComponent>();
    }


    void Update()
    {
       if( Input.GetKey(KeyCode.I))
        {
         TakeDamage(currentHealth);
        }
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
        Debug.Log("OnDeath Triggered");
        questKillComponent.KillComplete();
    }


}
