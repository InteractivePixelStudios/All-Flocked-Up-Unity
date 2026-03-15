using UnityEngine;

public class EnemyBase_HitDetect : EnemyBaseComponent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            TriggerStateChangeOnHit();
            Debug.Log("Enemy Hit by POOP");
        }
    }
}
