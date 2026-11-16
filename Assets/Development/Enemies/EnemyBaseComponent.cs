using UnityEngine;
public enum ReactionState { Normal,Fire,Bomb,Confetti,Glow}
public class EnemyBaseComponent : MonoBehaviour, I_EnemyBase
{
    [SerializeField] private Q_KillComponent questKillComponent;
    public GameObject enemyRef;

    void Awake()
    {
        questKillComponent = GetComponent<Q_KillComponent>();
        enemyRef = this.gameObject;
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
    }
}
