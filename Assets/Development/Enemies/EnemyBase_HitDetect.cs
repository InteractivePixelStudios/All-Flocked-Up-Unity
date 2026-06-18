using UnityEngine;

public class EnemyBase_HitDetect : MonoBehaviour
{
    private EnemyBaseComponent enemyBase;
    PoopType currentPoopType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemyBase = GetComponentInParent<EnemyBaseComponent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            enemyBase.TakeDamage(10,currentPoopType);
            
        }
    }
}
