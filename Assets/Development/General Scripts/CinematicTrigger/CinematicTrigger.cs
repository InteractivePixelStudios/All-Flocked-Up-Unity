using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cinematicPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cinematicPrefab.SetActive(true);
        }
    }
}
