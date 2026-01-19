using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    private int trashCount;
    [SerializeField] private GameObject trash1;
    [SerializeField] private GameObject trash2;
    [SerializeField] private GameObject trash3;

    private void Start()
    {
        ClearTrash();
    }

    private void AddTrash(int count)
    {
        if (count ==1)
        {
            trash1.SetActive(true);
        }
        if(count == 2)
        {
            trash2.SetActive(true);
        }
        if (count == 3)
        {
            trash3 .SetActive(true);
        }
        else return;

    }

    public void ClearTrash()
    {
        trash1.SetActive(false);
        trash2.SetActive(false);
        trash3.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            var trashComp = other.gameObject.GetComponent<Q_Garbage>();
            if ( trashComp != null)
            {
                trashComp.GarbageInTrash();
                trashCount++;
                AddTrash(trashCount);
            }
            Destroy(other.gameObject);
        }
    }
}
