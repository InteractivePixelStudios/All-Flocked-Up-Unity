using Unity.VisualScripting;
using UnityEngine;

public class PlayerNavArrow : MonoBehaviour
{
    public GameObject destination;
    [SerializeField]private GameObject arrowPrefab;
    private GameObject spawnedArrow;
    [SerializeField] private GameObject arrowSpawnPoint;
    [SerializeField]bool isEnabled;

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            RotateArrow();
        }
        else if (!isEnabled && spawnedArrow != null)
        {
            DestroyArrow();
        }
        else return;
    }

    public GameObject SetDestination(GameObject location)
    {
        destination = location;
        return destination;
    }

    public void EnablePointerArrow()
    {
        isEnabled = true;
        SpawnArrow();
    }

    private void SpawnArrow()
    {
        spawnedArrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position,arrowSpawnPoint.transform.rotation);
        spawnedArrow.transform.SetParent(arrowSpawnPoint.transform,true);
    }

    void RotateArrow()
    {
        Vector3 direction = destination.transform.position - this.transform.position;
        spawnedArrow.transform.rotation = Quaternion.LookRotation(direction);
    }

    public void DestroyArrow()
    {
        isEnabled = false;
        Destroy(spawnedArrow.gameObject);
    }
}
