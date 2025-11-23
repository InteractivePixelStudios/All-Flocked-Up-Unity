using Unity.VisualScripting;
using UnityEngine;

public class ObjectResizer : MonoBehaviour
{
    [SerializeField] private Transform mesh;
    [SerializeField] private float detectRadius = 1f;
    [SerializeField] private float maxDistance =1f;
    [SerializeField] private bool contacted = false;
    [SerializeField] private LayerMask playerLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

            CheckForPlayer();
        

    }

    private void CheckForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, detectRadius, playerLayer);

        if (colliders.Length > 0)
        {
            contacted = true;
            Debug.Log("Contacted");

            foreach (var collider in colliders)
            {
                float distance = Vector3.Distance(this.transform.position, collider.transform.position);

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
        else
        {
            contacted = false;
        }
    }
}
