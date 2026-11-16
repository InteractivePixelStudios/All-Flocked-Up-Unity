using UnityEngine;
public class ObjectResizer : MonoBehaviour
{
    [SerializeField] private Transform mesh;
    [SerializeField] private float detectRadius = 1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float distance;
    void Start()
    {
        mesh = GetComponent<Transform>();
        mesh.localScale = Vector3.one * (distance*2);

    }
    private void CheckForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, detectRadius, playerLayer);

        if (colliders.Length > 0)
        {
            Debug.Log("Contacted");

            foreach (var collider in colliders)
            {
                distance = Vector3.Distance(this.transform.position, collider.transform.position);

                if (distance >= 0)
                {

                    mesh.localScale = Vector3.one / distance;

                }
                else
                {
                    mesh.localScale = Vector3.one * distance;
                }
            }
        }
    }
}
