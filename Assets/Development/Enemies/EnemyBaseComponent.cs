using NUnit.Framework.Constraints;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public enum ReactionState { Normal,Fire,Bomb,Confetti,Glow}

public class EnemyBaseComponent : MonoBehaviour, I_EnemyBase
{
    [SerializeField] private Q_KillComponent questKillComponent;
    public GameObject enemyRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        questKillComponent = GetComponent<Q_KillComponent>();
        enemyRef = this.gameObject;
    }

    private void VisionConeSearch()
    {

    }


    public void TakeDamage(int damage, PoopType type)
    {
        TriggerStateChangeOnHit(type);
    }

    public void TriggerStateChangeOnHit(PoopType type)
    {
        OnHit(type);
    }

    public virtual void OnHit(PoopType type)
    {
        Debug.Log("CallBaseOnHit");
    }



}
