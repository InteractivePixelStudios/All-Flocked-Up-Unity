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
        if(spawnedArrow!= null)spawnedArrow.transform.position = arrowSpawnPoint.transform.position;
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

    public void SetEnabled(bool value)
    {
        isEnabled = value;
    }

    public GameObject SetDestination(GameObject location)
    {
        destination = location;
        return destination;
    }

    public void EnablePointerArrow(GameObject destination)
    {
        isEnabled = true;
        SpawnArrow(destination);
    }

    private void SpawnArrow(GameObject destination)
    {
        spawnedArrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position,arrowSpawnPoint.transform.rotation);
        //spawnedArrow.transform.SetParent(arrowSpawnPoint.transform,true);
        SetDestination(destination);
    }

    void RotateArrow()
    {
        Vector3 direction = destination.transform.position - this.transform.position;
        spawnedArrow.transform.rotation = Quaternion.LookRotation(direction);
    }

    public void DestroyArrow()
    {
        Destroy(spawnedArrow.gameObject);
    }
}
