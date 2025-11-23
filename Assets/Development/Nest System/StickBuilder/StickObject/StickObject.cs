using UnityEngine;

public class StickObject : MonoBehaviour
{
    public StickBuilder stickBuilder;

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            stickBuilder.CollectStick();
            Destroy(gameObject);
        }

    }
}
